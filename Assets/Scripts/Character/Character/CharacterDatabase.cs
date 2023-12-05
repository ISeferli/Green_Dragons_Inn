using System.Collections;
using System.Collections.Generic;
using GDI.CharacterClasses;
using UnityEngine;

public class CharacterDatabase : MonoBehaviour {
    public static CharacterDatabase instance;
    [SerializeField]
    private CharacterClasses fighter, archer, mage, cleric;

    void Awake(){
        if (instance!=null && instance!=this) {
            Destroy(gameObject);
        } else {
            instance = this;
        }
    }

    public (string charClass, string name, int hp, int ac, int attackRange, int moveRange, string mainAttribute) getCharacterAttributes (string characterClass) {
        CharacterClasses posClass;
        switch (characterClass) {
            case "fighter":
                posClass = fighter;
                break;
            case "archer":
                posClass = archer;
                break;
            default:
                Debug.Log($"Item type: {characterClass} could not be found or does not exist!");
                return (characterClass, null, 0, 0, 0, 0, null);
        }
        return (posClass.avClass.ToString(), posClass.name, posClass.healthPoints, posClass.armorClass, posClass.attackRange, posClass.moveRange, posClass.mainAttribute);
    }

    public void setCharacterAttributes(Transform character, string charClass, Dictionary<string, int> charStats){
        var scriptCharacterInfo = getCharacterAttributes(charClass);
        CharacterStat characterToGive = character.GetComponent<CharacterStat>();

        List<BaseStat> toGiveAttributes = new List<BaseStat>();
        foreach(KeyValuePair<string, int> stat in charStats){
            toGiveAttributes.Add(new BaseStat(stat.Value, stat.Key, ""));
            if(stat.Key.Equals("Dexterity")){
                characterToGive.armorClass = scriptCharacterInfo.ac + stat.Value;
            } else if (stat.Key.Equals("Constitution")) {
                characterToGive.healthPoints = scriptCharacterInfo.hp + stat.Value;
            }
        }

        characterToGive.name = scriptCharacterInfo.name;
        characterToGive.attackRange = scriptCharacterInfo.attackRange;
        characterToGive.moveRange = scriptCharacterInfo.moveRange;
        characterToGive.avClass = scriptCharacterInfo.charClass;
        characterToGive.mainAttribute = scriptCharacterInfo.mainAttribute;
        characterToGive.characterCreation(toGiveAttributes);
    }
        
}
