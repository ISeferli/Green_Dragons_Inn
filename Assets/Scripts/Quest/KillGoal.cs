public class KillGoal : Goal
{
    public int enemyID { get; set; }

    void OnDestroy(){
        CombatEvent.OnEnemyDeath -= enemyDied;
    }

    public KillGoal(Quest quest, int enemyID, string description, bool completed, int currentAmount, int requiredAmount){
        this.quest = quest;
        this.enemyID = enemyID;
        this.description = description;
        this.completed = completed;
        this.currentAmount = currentAmount;
        this.requiredAmount = requiredAmount;
    }

    public override void init(){
        base.init();
        CombatEvent.OnEnemyDeath += enemyDied;
    }

    void enemyDied(EnemyInterface enemy){
        if(enemy.ID == this.enemyID){
            this.currentAmount++;
            checkCompletion();
            this.evaluate();
            UIEventHandler.goalProgressHap(quest.description, this.currentAmount, this.requiredAmount);
        }
    }

    void checkCompletion(){
        if(currentAmount >= requiredAmount){
            completed = true;
        }
    }
}
