using UnityEngine;
using UnityEngine.AI;

public class CharacterPlayer : MonoBehaviour
{
    private Animator animator;
    private Grid grid;
    public CharacterStat characterStats;
    public CharacterLevel charLevel {get;set;}
    public int currentHealth;
    public int combatInitiative;
    public int moveRemain;

    // For combat only
    public GridTile destinationTile;
    public GridTile currentTile;
    public Color moveColor = Color.red;

    // Disadvantages
    public bool isWebbed;

    void OnDestroy(){
        CombatEvent.OnVariableChange -= initializeTiles;
    }

    void Start() {
        animator = GetComponent<Animator>();
        isWebbed = false;
        CombatEvent.OnVariableChange += initializeTiles;
        grid = FindObjectOfType<Grid>();
        charLevel = GetComponent<CharacterLevel>();
        characterStats = GetComponent<CharacterStat>();
        this.currentHealth = characterStats.healthPoints;
        this.moveRemain = characterStats.moveRange;
    }

    void Update(){
        if(destinationTile!=null && this.GetComponent<NavMeshAgent>().hasPath && this.GetComponent<NavMeshAgent>().pathStatus==NavMeshPathStatus.PathComplete){
            ReachDestination();
        }
    }

    private void ReachDestination(){
        currentTile.removeHighlight();
        currentTile = destinationTile;
        currentTile.Highlight(moveColor);
        destinationTile = null;
        GridGenerator.instance.highlightCharacter(this.transform);
    }

    private void initializeTiles(){
        currentTile = FindObjectOfType<GridGenerator>().FindNearestTile(transform.position);
        currentTile.isAnyoneOn = this.gameObject;
    }

    public void performAttack() {
       animator.SetTrigger("BaseAttack");
    }

    public void takeDamage(int amount){
        currentHealth -= amount;
        if(currentHealth<=0){
            Die();
        }
        UIEventHandler.sendMessageToUI("" + this.GetComponent<CharacterStat>().name + " took " + amount + " points of damage");
        UIEventHandler.healthChanged(this.characterStats.healthPoints, currentHealth);
    }

    public void healDamage(int amount){
        currentHealth += amount;
        if(currentHealth > characterStats.healthPoints){
            currentHealth = characterStats.healthPoints;
        }
       UIEventHandler.healthChanged(this.characterStats.healthPoints, currentHealth);
    }

    private void Die(){
        CombatEvent.characterDied(this);
        UIEventHandler.sendMessageToUI("" + this.name + " died.");
        Destroy(gameObject);
    }

    public int rollInitiative() {
        int initiative = Random.Range(1, 21);
        combatInitiative = initiative + characterStats.attributes["Dexterity"].baseValue;
        UIEventHandler.sendMessageToUI("" + this.GetComponent<CharacterStat>().name + " rolled for initiative " + combatInitiative);
        return combatInitiative;
    }

    public void movePlayerFree(Vector3 targetPos){
        this.GetComponent<NavMeshAgent>().stoppingDistance = 0f;
        this.GetComponent<NavMeshAgent>().SetDestination(grid.GetComponent<GridGenerator>().getPlacementNear(targetPos));
    }

    public void movePlayerCombat(GridTile tile){
        currentTile.isAnyoneOn = null;
        if(destinationTile!=null){
            destinationTile.removeHighlight();
        }

        destinationTile = tile;
        this.GetComponent<NavMeshAgent>().destination = destinationTile.transform.position;
        destinationTile.isAnyoneOn = this.gameObject;
        destinationTile.Highlight(moveColor);
        currentTile.Highlight(moveColor);
    }

    public bool isNearEnemyRange(Transform enemy, bool hasDest){
        if(hasDest){
            if(enemy!=null && Vector3.Distance(destinationTile.transform.position, enemy.GetComponent<EnemyInterface>().currentTile.transform.position)<=this.characterStats.attackRange){
                return true;
            }
        } else {
            if(enemy!=null && Vector3.Distance(currentTile.transform.position, enemy.GetComponent<EnemyInterface>().currentTile.transform.position)<=this.characterStats.attackRange){
                return true;
            }
        }
        return false;
    }
}
