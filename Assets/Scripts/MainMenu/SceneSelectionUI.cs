using UnityEngine;
using UnityEngine.UI;

public class SceneSelectionUI : MonoBehaviour
{
    [SerializeField] private Image level1;
    [SerializeField] private Image level2;
    [SerializeField] private Image level3;

    public void level1Selected(){
        MainMenuUI.instance.levelName = "FirstScene";
        PlayerManager.levelManagerName = MainMenuUI.instance.levelName;
        MainMenuUI.instance.mainMenuPanel.gameObject.SetActive(true);
        MainMenuUI.instance.sceneSelectionPanel.gameObject.SetActive(false);
    }

    public void level2Selected(){
        MainMenuUI.instance.levelName = "SecondScene";
        PlayerManager.levelManagerName = MainMenuUI.instance.levelName;
        MainMenuUI.instance.mainMenuPanel.gameObject.SetActive(true);
        MainMenuUI.instance.sceneSelectionPanel.gameObject.SetActive(false);
    }

    public void level3Selected(){
        MainMenuUI.instance.levelName = "ThirdScene";
        PlayerManager.levelManagerName = MainMenuUI.instance.levelName;
        MainMenuUI.instance.mainMenuPanel.gameObject.SetActive(true);
        MainMenuUI.instance.sceneSelectionPanel.gameObject.SetActive(false);
    }

}
