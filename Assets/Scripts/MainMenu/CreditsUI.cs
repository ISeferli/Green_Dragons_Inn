using UnityEngine;

public class CreditsUI : MonoBehaviour
{
    public void pressBackToMenu(){
        MainMenuUI.instance.mainMenuPanel.gameObject.SetActive(true);
        MainMenuUI.instance.creditsPanel.gameObject.SetActive(false);
    }
}
