using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GDI.CharacterClasses;

public class CurrentCharacterUI : MonoBehaviour
{
    public static CurrentCharacterUI instance;
    public Image characterClassPrefab;
    public TMP_Text characterName, characterClass;
    public RectTransform dropdownClass;

    private TMP_Dropdown dropDown;
    [SerializeField] private CharacterClasses fighter, archer,  mage, cleric;

    void Start(){
        instance = this;
        dropDown = dropdownClass.GetComponent<TMP_Dropdown>();
        dropDown.onValueChanged.AddListener(delegate {DropdownValueChanged(dropDown);});
        initializePanel();
    }

    private void initializePanel(){
        string className = GetSelectedOption();
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
                Debug.Log($"Item type: {characterClass} could not be found or does not exist!");
                break;
        }

        if(posClass!=null){
            characterName.text = posClass.name;
            characterClassPrefab.sprite = posClass.icon;
            characterClass.text = posClass.avClass.ToString();
        }
    }

    public string GetSelectedOption(){
        int selectedIndex = dropDown.value;
        string selectedOption = dropDown.options[selectedIndex].text;
        return selectedOption;
    }

    void DropdownValueChanged(TMP_Dropdown change){
        initializePanel();
    }
}
