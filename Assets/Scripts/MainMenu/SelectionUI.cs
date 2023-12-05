using System.Collections.Generic;
using GDI.CharacterClasses;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectionUI : MonoBehaviour {
    public RectTransform currentPlayersPanel;
    public RectTransform buttonPanel;
    public RectTransform attributesPanel;
    public TMP_Dropdown toRemoveCharacter;
    [SerializeField] private CharacterClasses fighter, archer, mage, cleric;
    [SerializeField] private TMP_Dropdown dropDown;

    public Dictionary<CharacterClasses, Dictionary<string, int>> charactersToPlay;

    void Start(){
        charactersToPlay = new Dictionary<CharacterClasses, Dictionary<string, int>>();
        toRemoveCharacter.options.Clear();
    }

    public void startGame(){
        string levelName = MainMenuUI.instance.levelName;
        if(levelName == ""){
            MainMenuUI.instance.characterSelectionPanel.gameObject.SetActive(false);
            MainMenuUI.instance.sceneSelectionPanel.gameObject.SetActive(true);
        }
        
        GameManager.instance.gameState.PlayerSpawnLocation = "CharacterSpawner";
        GameManager.instance.playerManager.charactersToPlay = charactersToPlay;
        SceneManager.LoadScene(levelName, LoadSceneMode.Single);
    }

    public void backToMenu(){
        MainMenuUI.instance.mainMenuPanel.gameObject.SetActive(true);
        MainMenuUI.instance.characterSelectionPanel.gameObject.SetActive(false);
    }

    public void createCharacter(){
        Dictionary<string, int> addAttributes = attributesPanel.GetComponent<CharacterAttributesUI>().getAllocatedAttrReady();
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

        if(addAttributes == null){
            return;
        }
        
        charactersToPlay.Add(posClass, addAttributes);
        if(addAttributes!=null){    
            List<TMP_Dropdown.OptionData> addedHeroes = new List<TMP_Dropdown.OptionData>();
            addedHeroes.Add(new TMP_Dropdown.OptionData() { text = posClass.name });
            toRemoveCharacter.AddOptions(addedHeroes);
            toRemoveCharacter.RefreshShownValue();
        }
    }

    public void removeCharacter(){
        CharacterClasses delClass = null;
        string charName = toRemoveCharacter.options[toRemoveCharacter.value].text;
        toRemoveCharacter.options.RemoveAt(toRemoveCharacter.value);
        toRemoveCharacter.RefreshShownValue();
        if(charName == fighter.name){
            delClass = fighter;
            charactersToPlay.Remove(delClass);
        } else if(charName == archer.name){
            delClass = archer;
        }
    }

}
