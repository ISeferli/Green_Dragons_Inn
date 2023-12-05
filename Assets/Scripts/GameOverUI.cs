using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public void pressedButtonOver(){
        SceneManager.LoadScene("GameMenu", LoadSceneMode.Single);
    }
}
