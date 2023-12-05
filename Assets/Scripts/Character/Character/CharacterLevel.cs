using UnityEngine;

public class CharacterLevel : MonoBehaviour {
    public int level {get;set;}
    public int currentExperience {get;set;}
    public int requiredExperience {get {return level*25;} }
    public int extraAttributePoints;
    public bool leveledUp;

    void OnDestroy(){
        CombatEvent.OnEnemyDeath -= getExperienceFromEnemy;
    }

    void Start(){
        CombatEvent.OnEnemyDeath += getExperienceFromEnemy;
        extraAttributePoints = 0;
        currentExperience = 0;
        level = 1;
        leveledUp = false;
    }

    public void getExperienceFromEnemy(EnemyInterface enemy){
        grantExperience(enemy.experience);
    }

    public void grantExperience(int amount){
        currentExperience += amount;
        while(currentExperience >= requiredExperience){
            currentExperience -= requiredExperience;
            level++;
            checkLevelUpgrades();
            UIEventHandler.levelUpTime();
            leveledUp = true;
            UIEventHandler.sendMessageToUI("" + this.GetComponent<CharacterStat>().name + " leveled up!");
        }
        UIEventHandler.experienceRaise(requiredExperience, currentExperience, level);
    }

    private void checkLevelUpgrades(){
        if(level == 2 || level == 3){
            extraAttributePoints += 1;
        } else if(level == 4){
            extraAttributePoints += 2;
        }
    }
}
