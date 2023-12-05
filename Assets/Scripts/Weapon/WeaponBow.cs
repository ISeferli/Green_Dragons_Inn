using System.Collections.Generic;
using UnityEngine;

public class WeaponBow : MonoBehaviour, WeaponInterface {
    public List<BaseStat> stats { get; set; }
    
    public void performAttack(GameObject personToHit, GameObject whoPlays){
        int attackRoll = Random.Range(1, 21);
        CharacterStat playerStats = whoPlays.GetComponent<CharacterStat>();
        whoPlays.GetComponent<CharacterPlayer>().performAttack();
        UIEventHandler.sendMessageToUI("" + playerStats.name + " rolled attacked of " + attackRoll);
        if((attackRoll+playerStats.attributes[playerStats.mainAttribute].getCalculatedStatValue()) >= personToHit.GetComponent<EnemyInterface>().armor){
            int damage = playerStats.attributes[playerStats.mainAttribute].getCalculatedStatValue();
            if(playerStats.attributes.ContainsKey("Wisdom")){
                damage = criticalAttack(damage, playerStats.attributes["Wisdom"].getCalculatedStatValue(), playerStats);
            }
            personToHit.GetComponent<EnemyInterface>().takeDamage(damage);
        }
    }

    public int criticalAttack(int baseDamage, int wisdom, CharacterStat whoPlays){
        int crit = 0;
        if(Random.Range(1, 101) < wisdom*5){
            crit = baseDamage*2;
            UIEventHandler.sendMessageToUI("" + whoPlays.name + " rolled critical damage of " + crit);
        } else {
            crit = baseDamage;
        }
        return crit;
    }
}
