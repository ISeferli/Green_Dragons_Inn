using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using GDI.WorldInteraction;

public class EnemySpider : MonoBehaviour, EnemyInterface
{
    public CharacterPlayer player;
    public NavMeshAgent navAgent { get; set; }
    private Collider[] withinAggro;
    private Collider[] restPack;

    public LayerMask aggroLayer;
    public GridTile currentTile { get; set; }
    public GameObject thisEnemy { get; set; }
    private Animator animator;

    // Enemy Stats
    public List<BaseStat> stats { get; set; }
    public int maxHealth { get; set; }
    public int moveRange { get; set; }
    public int moveRemain { get; set; }
    public int attackRange { get; set; }
    public int armor { get; set; }
    public int ID { get; set; }
    public int health { get; set; }
    public int enemyMoveRemain;

    // Combat Variables
    public int combatInitiative { get; set; }
    public int hasAggro { get; set; }
    public CharacterPlayer playerTarget { get; set; }
    public int experience {get;set;}

    // Drops
    public DropTable dropTable { get;set; }
    public PickupItem pickupItem;

    // Special Attack
    public bool specCounter { get; set; }
    private int webDC = 14;

    void Start(){
        thisEnemy = this.gameObject;
        animator = GetComponent<Animator>();
        animator.Play("Idle");
        specCounter = false;
        navAgent = GetComponent<NavMeshAgent>();
        stats = new List<BaseStat>();
        stats.Add(new BaseStat(3, "Strength"));
        stats.Add(new BaseStat(3, "Dexterity"));
        stats.Add(new BaseStat(4, "Constitution"));
        dropTable = new DropTable();
        dropTable.loot = new List<LootDrop>(){
            new LootDrop("Sword", 10),
            new LootDrop("Bow", 10),
            new LootDrop("Potion", 25),
            new LootDrop("LightPotion", 45)
        };
        this.maxHealth = 10 + stats[2].baseValue;
        this.armor = 10 + stats[1].baseValue;
        this.enemyMoveRemain = moveRange;
        moveRange = 5;
        moveRemain = moveRange;
        attackRange = 1;
        health = maxHealth;
        this.ID = 0;
        experience = 20;
        hasAggro = 0;
    }

    void FixedUpdate(){
        withinAggro = Physics.OverlapSphere(transform.position, 6, aggroLayer);
        if(withinAggro.Length > 0){
            hasAggro = 1;
            if(WorldInteraction.instance.combatMode==0){
                playerTarget = withinAggro[0].GetComponent<CharacterPlayer>();
            } else {
                playerTarget = this.checkForClosestEnemyTargets();
            }
            player = playerTarget;
            WorldInteraction.instance.combatMode = 1;
            WorldInteraction.instance.beginCombatMode(hasAggro, this.transform);
        }
    }

    void EnemyInterface.takeDamage(int amount) {
        UIEventHandler.sendMessageToUI("" + this.name + " took " + amount + " points of damage");
        health -= amount;
        if(health<=0){
            die();
        }
    }

    public void die(){
        CombatEvent.enemyDied(this);
        dropLoot();
        UIEventHandler.sendMessageToUI("" + this.name + " died.");
        Destroy(gameObject);
    }

    void EnemyInterface.performAttack() {
        int attackRoll = Random.Range(1, 21);
        UIEventHandler.sendMessageToUI("" + this.name + " rolled attack " + attackRoll);
        if((attackRoll+stats[0].baseValue) >= playerTarget.GetComponent<CharacterStat>().armorClass){
            animator.SetTrigger(Animator.StringToHash("Attack"));
            playerTarget.GetComponent<CharacterPlayer>().takeDamage(stats[0].baseValue);
        }
    }

    bool EnemyInterface.SpecialAttack(){
        if(!player.isWebbed && !specCounter){
            UIEventHandler.sendMessageToUI("" + this.name + " throws web on " + playerTarget.GetComponent<CharacterStat>().name);
            int savingThrow = Random.Range(1, 21);
            if((savingThrow + playerTarget.characterStats.attributes["Dexterity"].getCalculatedStatValue()) < webDC){
                playerTarget.isWebbed = true;
                UIEventHandler.sendMessageToUI("" + playerTarget.GetComponent<CharacterStat>().name + " rolled " + savingThrow + " dexterity saving throw.");
                UIEventHandler.sendMessageToUI("" + playerTarget.GetComponent<CharacterStat>().name + " fails saving throw.");
            } else {
                UIEventHandler.sendMessageToUI("" + playerTarget.GetComponent<CharacterStat>().name + " rolled " + savingThrow + " dexterity saving throw.");
                UIEventHandler.sendMessageToUI("" + playerTarget.GetComponent<CharacterStat>().name + " succeeds saving throw.");
            }
            specCounter = true;
            return true;
        }
        return false;
    }

    void dropLoot(){
        Item item = dropTable.getDrop();
        if(item!=null){
            PickupItem instance = Instantiate(pickupItem, transform.position, Quaternion.identity);
            instance.GetComponent<AudioSource>().clip = instance.audioWhenAppear;
            instance.GetComponent<AudioSource>().Play();
            instance.itemDrop = item;
        }
    }

    // Returns the closest player character from the enemy that is playing
    public CharacterPlayer checkForClosestEnemyTargets(){
        int minDistance = int.MaxValue;
        foreach(Transform child in WorldInteraction.instance.player.transform){
            foreach(Transform typeCharacter in child){
                int distance = (int)Vector3.Distance(currentTile.transform.position, typeCharacter.GetComponent<CharacterPlayer>().currentTile.transform.position);
                if (distance < minDistance){
                    minDistance = distance;
                    playerTarget = typeCharacter.GetComponent<CharacterPlayer>();
                }
            }
        }
        return playerTarget;
    }

    public void SnapToNearestTile(){
        var tile = FindObjectOfType<GridGenerator>().FindNearestTile(transform.position);
        currentTile = tile;
        currentTile.isAnyoneOn = this.gameObject;
        transform.position = currentTile.transform.position;
    }

    public void moveTowardsPlayer(GridTile playerTile, System.Action onDestinationReached){
        StartCoroutine(MoveToDestination(playerTile, onDestinationReached));
    }

    private IEnumerator MoveToDestination(GridTile destinationTile, System.Action onDestinationReached){
        animator.SetBool(Animator.StringToHash("IsMoving"), true);
        currentTile.isAnyoneOn = null;
        navAgent.SetDestination(destinationTile.transform.position);

        while (navAgent.pathPending || navAgent.remainingDistance > navAgent.stoppingDistance){
            yield return null;
        }

        animator.SetBool(Animator.StringToHash("IsMoving"), false);
        currentTile = destinationTile;
        currentTile.isAnyoneOn = this.gameObject;
        // Invoke the callback when the enemy reaches its destination
        if(onDestinationReached != null){
            onDestinationReached?.Invoke();
        }
    }

}
