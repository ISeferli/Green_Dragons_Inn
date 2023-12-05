using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    public void pressBackToMenu(){
        MainMenuUI.instance.mainMenuPanel.gameObject.SetActive(true);
        MainMenuUI.instance.tutorialPanel.gameObject.SetActive(false);
    }
}

