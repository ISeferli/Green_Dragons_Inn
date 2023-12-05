using System.Collections.Generic;
using GDI.CharacterClasses;
using TMPro;
using UnityEngine;

public class DropDownHandler : MonoBehaviour {
    [SerializeField] private CharacterClasses fighter, archer, mage, cleric;
    private TMP_Dropdown dropDown;
    
    void Awake(){
        dropDown = transform.GetComponent<TMP_Dropdown>();
        dropDown.options.Clear();

        List<TMP_Dropdown.OptionData> possibleClasses = new List<TMP_Dropdown.OptionData>();
        possibleClasses.Add(new TMP_Dropdown.OptionData() { text = fighter.avClass.ToString() });
        possibleClasses.Add(new TMP_Dropdown.OptionData() { text = archer.avClass.ToString() });
        // possibleClasses.Add(new TMP_Dropdown.OptionData() { text = mage.avClass.ToString() });
        // possibleClasses.Add(new TMP_Dropdown.OptionData() { text = cleric.avClass.ToString() });


        dropDown.AddOptions(possibleClasses);
    }
   
}