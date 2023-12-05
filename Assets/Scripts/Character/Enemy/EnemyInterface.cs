using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface EnemyInterface {
    int hasAggro { get; set; }
    int combatInitiative {get;set;}
    int experience {get;set;}
    int ID { get; set; }
    int maxHealth { get; set; }
    int armor { get; set; }

    GridTile currentTile { get; set; }
    CharacterPlayer playerTarget { get; set; }
    NavMeshAgent navAgent { get; set; }
    List<BaseStat> stats { get; set; }
    int moveRange { get; set; }
    int moveRemain { get; set; }
    int attackRange { get; set; }
    GameObject thisEnemy { get; set; }
    bool specCounter { get; set; }

    void die();
    void takeDamage(int amount);
    void performAttack();
    bool SpecialAttack();
    void moveTowardsPlayer(GridTile playerTile, System.Action onDestinationReached);
    CharacterPlayer checkForClosestEnemyTargets();
    void SnapToNearestTile();

    bool isNearRange(Transform target){
        if(target!=null && Vector3.Distance(currentTile.transform.position, target.GetComponent<CharacterPlayer>().currentTile.transform.position)<=this.attackRange){
            return true;
        }
        return false;
    }

    public int rollInitiative() {
        SnapToNearestTile();
        int initiative = Random.Range(1, 21);
        combatInitiative = initiative + stats[1].baseValue;
        UIEventHandler.sendMessageToUI("" + this.thisEnemy.name + " rolled initiative " + combatInitiative);
        return combatInitiative;
    }
    
}
