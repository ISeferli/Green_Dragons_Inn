using UnityEngine;

public class MainMenuUI : MonoBehaviour {
    public static MainMenuUI instance;
    public RectTransform mainMenuPanel;
    public RectTransform characterSelectionPanel;
    public RectTransform creditsPanel;
    public RectTransform sceneSelectionPanel;
    public RectTransform tutorialPanel;
    public string levelName;

    void Start(){
        instance = this;
        levelName = "";
        mainMenuPanel.gameObject.SetActive(true);
        characterSelectionPanel.gameObject.SetActive(false);
    }

    public void startButton(){
        if(levelName!=""){
            mainMenuPanel.gameObject.SetActive(false);
            characterSelectionPanel.gameObject.SetActive(true);
        } else {
            mainMenuPanel.gameObject.SetActive(false);
            sceneSelectionPanel.gameObject.SetActive(true);
        }
    }

    public void creditsButton(){
        mainMenuPanel.gameObject.SetActive(false);
        creditsPanel.gameObject.SetActive(true);
    }

    public void tutorialButton(){
        mainMenuPanel.gameObject.SetActive(false);
        tutorialPanel.gameObject.SetActive(true);
    }

    public void levelButton(){
        mainMenuPanel.gameObject.SetActive(false);
        sceneSelectionPanel.gameObject.SetActive(true);
    }

    public void exitButton(){
        Application.Quit();
    }
}
