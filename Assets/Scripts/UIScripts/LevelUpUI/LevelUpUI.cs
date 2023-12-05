using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelUpUI : MonoBehaviour {
    public RectTransform attributesInfo;
    public TMP_Text attributesTotalPoints;
    private CharacterStat playerLeveled;

    private TMP_Text attributeText;
    private int remainingPoints;
    private Dictionary<string, int> upgradedAttributes;

    void onDestroy(){
        UIEventHandler.OnLevelUpUIActivated -= playerInitiated;
    }

    void Awake(){
        UIEventHandler.OnLevelUpUIActivated += playerInitiated;
        upgradedAttributes = new Dictionary<string, int>();
    }

    private void playerInitiated(CharacterStat player){
        playerLeveled = player;
        createAttributePanel();
    }

    private void createAttributePanel(){
        for(int i=0; i<attributesInfo.childCount; i++){
            attributeText = attributesInfo.GetChild(i).GetComponent<TMP_Text>();
            attributeText.text = "" + attributesInfo.GetChild(i).name + ": 0";
            foreach(KeyValuePair<string, BaseStat> attribute in playerLeveled.attributes){
                if(attributeText.name.Equals(attribute.Key)){
                    attributeText.text = "" + attribute.Key + ": " + attribute.Value.baseValue;
                }
            }
        }
        remainingPoints = playerLeveled.GetComponent<CharacterLevel>().extraAttributePoints;
        attributesTotalPoints.text = "Available Points: " + remainingPoints;
    }


    public void OnAttributeButtonPressed(string buttonName){
        int pointsUsed = returnNumberFromString(attributesInfo.Find(buttonName).GetComponent<TMP_Text>().text);
        if(remainingPoints-1<0){
            clearAttributes();
        } else {
            changeAttributePoint(buttonName, (pointsUsed+1));
            if(!upgradedAttributes.ContainsKey(buttonName)) upgradedAttributes.Add(buttonName, (pointsUsed+1));
            attributesTotalPoints.text = "Available Points: " + changeAvailablePoints(-1);
        }
    }

    private void changeAttributePoint(string name, int points){
        for(int i=0; i<attributesInfo.childCount; i++){
            if(attributesInfo.GetChild(i).name.Equals(name)){
                attributesInfo.GetChild(i).GetComponent<TMP_Text>().text = "" + name + ": " + points;
                break;
            }
        }
    }

    private int changeAvailablePoints(int changeNumber){
        remainingPoints = remainingPoints + changeNumber;
        return remainingPoints;
    }

    public void clearAttributes(){
        for(int i=2; i<attributesInfo.childCount; i++){
            if(playerLeveled.attributes.ContainsKey(attributesInfo.GetChild(i).name)){
                changeAttributePoint(attributesInfo.GetChild(i).name, playerLeveled.attributes[attributesInfo.GetChild(i).name].baseValue);
            } else {
                if(upgradedAttributes.ContainsKey(attributesInfo.GetChild(i).name)){
                    changeAttributePoint(attributesInfo.GetChild(i).name, 0);
                }
            }
        }
        upgradedAttributes.Clear();
        remainingPoints = playerLeveled.GetComponent<CharacterLevel>().extraAttributePoints;
        attributesTotalPoints.text = "Available Points: " + remainingPoints;
    }

    public void addAttribute(){
        int added = 0;
        for(int i=2; i<attributesInfo.childCount; i++){
            int allocPoint = returnNumberFromString(attributesInfo.GetChild(i).GetComponent<TMP_Text>().text);
            if(playerLeveled.attributes.ContainsKey(attributesInfo.GetChild(i).name)){
                added = 0;
                if(allocPoint > playerLeveled.attributes[attributesInfo.GetChild(i).name].baseValue){
                    playerLeveled.attributes[attributesInfo.GetChild(i).name].baseValue = allocPoint;
                    if(attributesInfo.GetChild(i).name.Equals("Constitution")){
                        playerLeveled.healthPoints = 10 + allocPoint;
                        UIEventHandler.healthChanged(playerLeveled.healthPoints, playerLeveled.GetComponent<CharacterPlayer>().currentHealth);
                    } else if (attributesInfo.GetChild(i).name.Equals("Dexterity")){
                        playerLeveled.armorClass = 10 + allocPoint;
                    }
                }
            } else {
                if(allocPoint > 0){
                    added = 1;
                    playerLeveled.attributes.Add(attributesInfo.GetChild(i).name, new BaseStat(allocPoint, attributesInfo.GetChild(i).name));
                }
            }
        }
        UIEventHandler.statsChanged(playerLeveled, added);
        playerLeveled.GetComponent<CharacterLevel>().extraAttributePoints = 0;
        playerLeveled.GetComponent<CharacterLevel>().leveledUp = false;
        UIEventHandler.levelUpCompleted();
        this.gameObject.SetActive(false);
    }

    private int returnNumberFromString(string text){
        string inputString = text;
        string[] parts = inputString.Split(": ");
        string lastNumberString = parts[1].Trim();

        int number = 0;
        if (int.TryParse(lastNumberString, out number)){
        } else {
            Debug.Log("Invalid number format");
        }
        return number;
    }
}
