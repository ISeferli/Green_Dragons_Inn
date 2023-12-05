using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GDI.CharacterClasses;

public class CharacterAttributesUI : MonoBehaviour {
    public RectTransform attributesInfo;
    public RectTransform attributesAlloc;
    public TMP_Text attributesTotalPoints;

    [SerializeField] private CharacterClasses fighter, archer, mage, cleric;
    [SerializeField] private TMP_Dropdown dropDown;
    private string mainAttribute;
    private int remainingPoints;
    private List<string> curAllocatedAttribute;

    void Start(){
        dropDown.onValueChanged.AddListener(delegate {DropdownValueChanged(dropDown);});
        curAllocatedAttribute = new List<string>();
        mainAttribute = nonChangeableAttribute();
        updateAttributePanel(true);
    }

    private void updateAttributePanel(bool first){
        if(!first){
            clearAttributes();
        }

        if(first){
            //The first two children will always be Constitution and Dexterity
            attributesAlloc.transform.GetChild(0).GetComponent<Image>().color = Color.red;
            curAllocatedAttribute.Add(attributesAlloc.transform.GetChild(0).name);
            attributesAlloc.transform.GetChild(1).GetComponent<Image>().color = Color.red;
            curAllocatedAttribute.Add(attributesAlloc.transform.GetChild(1).name);
        }
        
        for(int i=2; i<attributesAlloc.transform.childCount; i++){
            if(attributesAlloc.transform.GetChild(i).name.Equals(mainAttribute)){
                attributesAlloc.transform.GetChild(i).GetComponent<Image>().color = Color.red;
                if(!curAllocatedAttribute.Contains(attributesAlloc.transform.GetChild(i).name)){
                    curAllocatedAttribute.Add(attributesAlloc.transform.GetChild(i).name);
                }
            }
        }
        changeAttributePoint(mainAttribute, 3);
        remainingPoints = calculateMaxAvailablePoints();
        attributesTotalPoints.text = "Available Points: " + remainingPoints;
    }

    public void clearAttributes(){
        curAllocatedAttribute.Clear();
        mainAttribute = nonChangeableAttribute();
        for(int i=0; i<attributesAlloc.transform.childCount; i++){
            changeAttributePoint(attributesAlloc.transform.GetChild(i).name, 0);
            if(!attributesAlloc.transform.GetChild(i).name.Equals("Constitution") && !attributesAlloc.transform.GetChild(i).name.Equals("Dexterity")){
                attributesAlloc.transform.GetChild(i).GetComponent<Image>().color = Color.white;
            }

            if(attributesAlloc.transform.GetChild(i).name.Equals(mainAttribute)){
                attributesAlloc.transform.GetChild(i).GetComponent<Image>().color = Color.red;
                if(!curAllocatedAttribute.Contains(attributesAlloc.transform.GetChild(i).name)){
                    curAllocatedAttribute.Add(attributesAlloc.transform.GetChild(i).name);
                    changeAttributePoint(mainAttribute, 3);
                }
            }
        }

        if(!curAllocatedAttribute.Contains(attributesAlloc.transform.GetChild(0).name)){
            curAllocatedAttribute.Add(attributesAlloc.transform.GetChild(0).name);
        }

        if(!curAllocatedAttribute.Contains(attributesAlloc.transform.GetChild(1).name)){
            curAllocatedAttribute.Add(attributesAlloc.transform.GetChild(1).name);
        }
        remainingPoints = calculateMaxAvailablePoints();
        attributesTotalPoints.text = "Available Points: " + remainingPoints;
    }

    private int calculateMaxAvailablePoints(){
        int sumOfAttributes;
        sumOfAttributes = ((curAllocatedAttribute.Count - 1) * 4)+2-3; // -3 because each time the main attribute will have at least 3
        return sumOfAttributes;
    }

    public void OnButtonColorChange(string buttonName, bool red){
        int prevMaxAlloc = calculateMaxAvailablePoints();        
        int availAlloc = returnNumberFromString(attributesTotalPoints.text);
        int specAttr = 0;
        if(!red){
            int index = curAllocatedAttribute.IndexOf(buttonName);
            specAttr = returnNumberFromString(attributesInfo.GetChild(returnChildIndex(buttonName)).GetComponent<TMP_Text>().text);
            changeAttributePoint(buttonName, 0);
            curAllocatedAttribute.RemoveAt(index);
        } else {
            if(!curAllocatedAttribute.Contains(buttonName)){
                curAllocatedAttribute.Add(buttonName);
            }
        }

        int nowAvailPoints = 0;
        nowAvailPoints = calculateMaxAvailablePoints() - prevMaxAlloc + availAlloc + specAttr;
        changeAvailablePoints(calculateMaxAvailablePoints()-prevMaxAlloc + specAttr);
        if(nowAvailPoints < 0){
            clearAttributes();
        } else {
            attributesTotalPoints.text = "Available Points: " + nowAvailPoints;
        }
    }

    private int returnChildIndex(string sameValue){
        for(int i=0; i<attributesInfo.childCount; i++){
            if(attributesInfo.GetChild(i).name.Equals(sameValue)){
                return i;
            }
        }
        return 0;
    }

    public void OnAttributeButtonPressed(string buttonName){
        int pointsUsed = returnNumberFromString(attributesInfo.Find(buttonName).GetComponent<TMP_Text>().text);
        if(checkIfAllocated(buttonName)){
            if(remainingPoints-1<0){
                clearAttributes();
            } else {
                changeAttributePoint(buttonName, (pointsUsed+1));
                attributesTotalPoints.text = "Available Points: " + changeAvailablePoints(-1);
            }
        }
    }

    private int changeAvailablePoints(int changeNumber){
        remainingPoints = remainingPoints + changeNumber;
        return remainingPoints;
    }

    private void changeAttributePoint(string name, int points){
        for(int i=0; i<attributesInfo.childCount; i++){
            if(attributesInfo.GetChild(i).name.Equals(name)){
                attributesInfo.GetChild(i).GetComponent<TMP_Text>().text = "" + name + ": " + points;
                break;
            }
        }
    }

    private bool checkIfAllocated(string buttonName){
        for(int i=0; i<curAllocatedAttribute.Count; i++){
            if(curAllocatedAttribute[i].Equals(buttonName)){
                return true;
            }
        }
        return false;
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

    public string nonChangeableAttribute(){
        string className = CurrentCharacterUI.instance.GetSelectedOption();
        CharacterClasses posClass = null;
        switch (className) {
            case "fighter":
                posClass = fighter;
                break;
            case "archer":
                posClass = archer;
                break;
            case "mage":
                posClass = mage;
                break;
            case "cleric":
                posClass = cleric;
                break;
            default:
                Debug.Log($"Item type: {className} could not be found or does not exist!");
                break;
        }

        string main=null;
        if(posClass!=null){
            main = posClass.mainAttribute;
        }
        return main;
    }

    private void DropdownValueChanged(TMP_Dropdown change){
        updateAttributePanel(false);
    }

    public Dictionary<string, int> getAllocatedAttrReady(){
        Dictionary<string, int> allocAttr = new Dictionary<string, int>();
        if(remainingPoints!=0){
            return allocAttr = null;
        } else {
            for(int i=0; i<attributesAlloc.childCount; i++){
                if(attributesAlloc.GetChild(i).GetComponent<Button>().image.color == Color.red){
                    int value = returnNumberFromString(attributesInfo.GetChild(i).GetComponent<TMP_Text>().text);
                    allocAttr.Add(attributesInfo.GetChild(i).name ,value);
                }
            }
        }
        return allocAttr;
    }
}