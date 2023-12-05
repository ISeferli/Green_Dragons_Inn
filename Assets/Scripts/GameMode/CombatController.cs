using System.Collections;
using System.Collections.Generic;
using GDI.WorldInteraction;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CombatController : MonoBehaviour {
    public static CombatController instance;
    public Dictionary<Transform, int> initiativeOrder;
    public List<KeyValuePair<Transform, int>> sortedInitiative;
    public GameObject playerInventory;
    public int currentTurn;
    private GameObject chosenEnemy;

    // Movement Action
    private bool attackBtnPressed;
    private Vector3 currentPosition;
    private bool movementAction;
    private int numberOfActions;
    public bool actionTaken;
    private bool dashActionTaken;

    // Action button
    public Button attackBtn;
    public Button dashBtn;
    public Button skipBtn;
    public string loadPrompt;
    private bool allEnemyKilled;
    private bool allCharacterKilled;

    // Panels
    public GameObject actionPanel;

    void OnDestroy(){
        CombatEvent.OnVariableChange -= rollInitiative;
        CombatEvent.OnVariableChangeOff -= endOfBattle;
        CombatEvent.OnTurnChange -= startOfTurn;
        CombatEvent.OnEnemyDeath -= deleteFromInitiative;
        CombatEvent.OnCharacterDeath -= deleteCharacterFromInitiative;
    }

    void Awake(){
        instance = this;
        initiativeOrder = new Dictionary<Transform, int>();
        sortedInitiative = new List<KeyValuePair<Transform, int>>();
        currentTurn = 0;
        actionTaken = false;
        dashActionTaken = false;
        CombatEvent.OnVariableChange += rollInitiative;
        CombatEvent.OnVariableChangeOff += endOfBattle;
        CombatEvent.OnTurnChange += startOfTurn;
        CombatEvent.OnEnemyDeath += deleteFromInitiative;
        CombatEvent.OnCharacterDeath += deleteCharacterFromInitiative;
        movementAction = false;
        attackBtnPressed = false;
        allEnemyKilled = true;
        allCharacterKilled = true;
        numberOfActions = 1;
        actionPanel.SetActive(false);
    }

    void Update(){
        if(Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() && WorldInteraction.instance.combatMode==1){
            if(sortedInitiative[currentTurn].Key.tag == "Character"){
                moveAction(sortedInitiative[currentTurn].Key.gameObject);
            }
        }
        
        if(actionTaken){
            attackBtn.interactable = false;
            dashBtn.interactable = false;
        }

        if(actionTaken && movementAction){
            changeTurn();
        }
    }

    private void startOfTurn(){
        attackBtn.interactable = true;
        dashBtn.interactable = true;
        skipBtn.interactable = true;   
        this.currentPosition = sortedInitiative[currentTurn].Key.transform.position;
        this.movementAction = false;
        this.attackBtnPressed = false;
        this.numberOfActions = 1;
        this.actionTaken = false;
        this.loadPrompt = "";

        if(sortedInitiative[currentTurn].Key.tag == "Character"){
            sortedInitiative[currentTurn].Key.GetComponent<NavMeshAgent>().isStopped = false;
            sortedInitiative[currentTurn].Key.GetComponent<CharacterPlayer>().moveRemain = sortedInitiative[currentTurn].Key.GetComponent<CharacterStat>().moveRange;
            GridGenerator.instance.highlightCharacter(sortedInitiative[currentTurn].Key);

            if(sortedInitiative[currentTurn].Key.GetComponent<CharacterPlayer>().isWebbed){
                attackBtn.interactable=false;
                this.actionTaken = true;
                dashBtn.interactable = false;
                sortedInitiative[currentTurn].Key.GetComponent<CharacterPlayer>().isWebbed = false;
                UIEventHandler.sendMessageToUI("" + sortedInitiative[currentTurn].Key.GetComponent<CharacterStat>().name + " is webbed, loses one turn.");
            }
        }

        // Enemy's Turn
        if(sortedInitiative[currentTurn].Key.tag == "Enemy"){
            attackBtn.interactable=false;
            skipBtn.interactable = false;
            dashBtn.interactable = false;
            sortedInitiative[currentTurn].Key.GetComponent<EnemyInterface>().moveRemain = sortedInitiative[currentTurn].Key.GetComponent<EnemyInterface>().moveRange;
            StartCoroutine(enemyTurn());
        }
    }

    public GameObject whoseTurn(){
        return sortedInitiative[currentTurn].Key.gameObject;
    }

    public void rollInitiative(){
        actionPanel.SetActive(true);
        foreach(Transform child in WorldInteraction.instance.player.transform){
            foreach(Transform typeCharacter in child){
                initiativeOrder.Add(typeCharacter, typeCharacter.GetComponent<CharacterPlayer>().rollInitiative());
            }
        }

        findSpecificEnemies();
        foreach(Transform childEnemy in WorldInteraction.instance.enemy.transform){
            if(childEnemy.GetComponent<EnemyInterface>().hasAggro==1){
                initiativeOrder.Add(childEnemy, childEnemy.GetComponent<EnemyInterface>().rollInitiative());
            }
        }
        sortedInitiative = initiativeOrder.ToList();
        sortedInitiative.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));
    }

    private void findSpecificEnemies(){
        EnemySpider[] restEnemy = FindObjectsOfType<EnemySpider>();
        foreach(EnemySpider enemy in restEnemy){
            if(GridGenerator.instance.IsTileInRangeForEnemy(GridGenerator.instance.FindNearestTile(enemy.transform.position), WorldInteraction.instance.triggeredEnemy.gameObject, 7)){
                enemy.hasAggro = 1;
                enemy.SnapToNearestTile();
                enemy.playerTarget = WorldInteraction.instance.triggeredEnemy.GetComponent<EnemyInterface>().playerTarget;
            }
        }

        EnemyShaman[] skeletonEnemy = FindObjectsOfType<EnemyShaman>();
        foreach(EnemyShaman enemy in skeletonEnemy){
            if(GridGenerator.instance.IsTileInRangeForEnemy(GridGenerator.instance.FindNearestTile(enemy.transform.position), WorldInteraction.instance.triggeredEnemy.gameObject, 10)){
                enemy.hasAggro = 1;
                enemy.SnapToNearestTile();
                enemy.playerTarget = WorldInteraction.instance.triggeredEnemy.GetComponent<EnemyInterface>().playerTarget;
            }
        }
    }

    private void endOfBattle(){
        GridGenerator.instance.ClearTiles();
        sortedInitiative.Clear();
        initiativeOrder.Clear();
        actionPanel.SetActive(false);
    }

    // Battle Actions
    public void buttonAttack(){
        attackBtnPressed = true;
        attackButtonCombat(sortedInitiative[currentTurn].Key.gameObject, currentTurn);
    }

    private void attackButtonCombat(GameObject whoPlays, int initiativeTurn){
        if(whoPlays.layer == 6){
            StartCoroutine(findEnemyForPlayerToAct(whoPlays));
        }
    }

    private IEnumerator findEnemyForPlayerToAct(GameObject whoPlays){
        int layerMask = LayerMask.GetMask("Enemy");
        bool foundEnemy = false;
        chosenEnemy = null;
        while(!foundEnemy){
            if(Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()){
                Ray interactionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if(Physics.Raycast(interactionRay, out hit, Mathf.Infinity, layerMask)){
                    chosenEnemy = hit.collider.gameObject;
                    if(chosenEnemy.tag == "Enemy"){
                        attackBtnPressed = false;
                        foundEnemy = true;
                        actionTaken = true;
                        if(whoPlays.GetComponent<CharacterPlayer>().isNearEnemyRange(chosenEnemy.transform, false)){
                            whoPlays.GetComponent<PlayerWeaponController>().performWeaponAttack(chosenEnemy, whoPlays);
                        } else {
                            GridTile bestTile = GridGenerator.instance.FindBestMoveTile(chosenEnemy, whoPlays.GetComponent<CharacterPlayer>().moveRemain, whoPlays);
                            whoPlays.GetComponent<CharacterPlayer>().movePlayerCombat(bestTile);
                            whoPlays.GetComponent<CharacterPlayer>().moveRemain -= (int)Vector3.Distance(whoPlays.transform.position, bestTile.transform.position);
                            if(whoPlays.GetComponent<CharacterPlayer>().isNearEnemyRange(chosenEnemy.transform, true)){
                                whoPlays.GetComponent<PlayerWeaponController>().performWeaponAttack(chosenEnemy, whoPlays);
                            }
                            movementAction = true;
                        }
                        this.numberOfActions--;
                        break;
                    }
                }
            }
            yield return null;
        }
    }

    private void moveAction(GameObject whoPlays){
        if(!attackBtnPressed){
            if(!movementAction || whoPlays.GetComponent<CharacterPlayer>().moveRemain != 0){
                Ray interactionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                LayerMask mask = LayerMask.GetMask("Tile");
                if(Physics.Raycast(interactionRay, out hit, Mathf.Infinity, mask)){
                    GridTile tile = hit.collider.gameObject.GetComponent<GridTile>();
                    if(GridGenerator.instance.IsTileInRange(tile, whoPlays)){
                        whoPlays.GetComponent<CharacterPlayer>().moveRemain -= (int)Vector3.Distance(whoPlays.transform.position, tile.transform.position);
                        whoPlays.GetComponent<CharacterPlayer>().movePlayerCombat(tile);
                        if(whoPlays.GetComponent<CharacterPlayer>().moveRemain == 0){
                            movementAction = true;
                        }

                        if(dashActionTaken){
                            dashActionTaken = false;
                            changeTurn();
                        }
                    }
                }
            }
        }
    }

    public void buttonDash(){
        dashButtonAction();
    }

    private void dashButtonAction(){
        if(sortedInitiative[currentTurn].Key.tag == "Character"){
            sortedInitiative[currentTurn].Key.GetComponent<CharacterPlayer>().moveRemain = sortedInitiative[currentTurn].Key.GetComponent<CharacterStat>().moveRange*2;
            GridGenerator.instance.highlightCharacter(sortedInitiative[currentTurn].Key.transform);
        } else {
            sortedInitiative[currentTurn].Key.GetComponent<EnemyInterface>().moveRange = sortedInitiative[currentTurn].Key.GetComponent<CharacterStat>().moveRange*2;
        }
        dashActionTaken = true;
    }
    
    public void buttonSkip(){
        skipButtonCombat();
    }

    private void skipButtonCombat(){
        changeTurn();
    }

    private void changeTurn(){
        sortedInitiative[currentTurn].Key.Find("Highlight").gameObject.SetActive(false);
        currentTurn++;
        if(currentTurn > sortedInitiative.Count-1){
            currentTurn = 0;
        }
        CombatEvent.turnChangeInitiative();
    }

    // Enemy Combat Logic
    private IEnumerator enemyTurn(){
        yield return new WaitForSeconds(3);
        enemyAttackLogic();
    }

    private void enemyAttackLogic(){
        EnemyInterface enemyInt = sortedInitiative[currentTurn].Key.transform.GetComponent<EnemyInterface>();
        if(enemyInt.checkForClosestEnemyTargets()!=null){
            CharacterPlayer target = enemyInt.playerTarget;
            if(enemyInt.isNearRange(target.transform)){
                if(!enemyInt.SpecialAttack()) enemyInt.performAttack();
            } else {
                float totalDis = Vector3.Distance(enemyInt.currentTile.transform.position, target.currentTile.transform.position);
                if(totalDis >= 2*enemyInt.moveRemain) enemyInt.moveRemain = enemyInt.moveRemain*2;
                GridTile bestTile = GridGenerator.instance.FindBestMoveTile(target.gameObject, enemyInt.moveRemain, sortedInitiative[currentTurn].Key.gameObject);
                enemyInt.moveTowardsPlayer(bestTile, () => 
                    {   if(enemyInt.isNearRange(target.transform) && enemyInt.moveRemain==enemyInt.moveRange){
                             if(!enemyInt.SpecialAttack()) enemyInt.performAttack();
                        }
                    });
            }
        }
        changeTurn();
    }

    private void deleteFromInitiative(EnemyInterface enemy){
        if(currentTurn == sortedInitiative.Count-1){
            currentTurn--;
        }
        KeyValuePair<Transform, int> enemyPair = new KeyValuePair<Transform, int>(enemy.thisEnemy.transform, initiativeOrder[enemy.thisEnemy.transform]);
        sortedInitiative.Remove(enemyPair);

        for(int i=0; i<sortedInitiative.Count; i++){
            if(sortedInitiative[i].Key.tag == "Enemy"){
                allEnemyKilled = false;
                break;
            }
            allEnemyKilled = true;
        }

        if(allEnemyKilled){
            WorldInteraction.instance.combatMode = 0;
            WorldInteraction.instance.hasBattleStarted = 0;
            WorldInteraction.instance.triggeredEnemy = null;
            this.actionTaken = false;
            CombatEvent.variableChangeOff();
        }
    }

    private void deleteCharacterFromInitiative(CharacterPlayer player){
        if(currentTurn == sortedInitiative.Count-1){
            currentTurn--;
        }

        KeyValuePair<Transform, int> characterPair = new KeyValuePair<Transform, int>(player.transform, initiativeOrder[player.transform]);
        sortedInitiative.Remove(characterPair);

        for(int i=0; i<sortedInitiative.Count; i++){
            if(sortedInitiative[i].Key.tag == "Character"){
                allCharacterKilled = false;
                break;
            }
            allCharacterKilled = true;
        }

        if(allCharacterKilled){
            SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
        }        
    }
}
