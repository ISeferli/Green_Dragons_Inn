using System.Collections.Generic;
using UnityEngine;

public class CharacterStat : MonoBehaviour {
    public Dictionary<string, BaseStat> attributes = new Dictionary<string, BaseStat>();

    public string mainAttribute;
    public string avClass;
    public new string name;

    public int healthPoints;
    public int armorClass;
    public int attackRange;
    public int moveRange;

    public int addSingleStatBonus(string statName, int bonusValue) {
        int addedNew = 0;
        if(!attributes.ContainsKey(statName)){
            attributes.Add(statName, new BaseStat(0, statName, ""));
            addedNew = 1;
        }
        attributes[statName].addStatBonus(new BonusStat(bonusValue));
        return addedNew;
    }

    public int removeStatBonus(string statName, int bonusValue){
        int removedZero = 0;
        attributes[statName].removeStatBonus(new BonusStat(bonusValue));
        if(attributes[statName].getCalculatedStatValue() == 0 && !statName.Equals("Dexterity")){
            attributes.Remove(statName);
            removedZero = 2;
        }
        return removedZero;
    }

    public void characterCreation(List<BaseStat> characterStats){
        for(int i=0; i<characterStats.Count; i++){
            attributes.Add(characterStats[i].statName, characterStats[i]);
        }
    }

    public int getSpecificStatValue(string statName){
        for(int i=0; i<attributes.Count; i++){
            if(attributes[statName]==null){
                return attributes[statName].finalValue;
            }
        }
        return 0;
    }
}
