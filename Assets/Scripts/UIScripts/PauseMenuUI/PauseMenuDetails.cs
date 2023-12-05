using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuDetails : MonoBehaviour {
    public static bool GameIsPaused = false;

    void OnDestroy(){
        PauseEvent.OnPauseTrigger -= pauseHandlerFunction;
    }

    void Start(){
        PauseEvent.OnPauseTrigger += pauseHandlerFunction;
    }

    void pauseHandlerFunction(){
        if(GameIsPaused){
            Resume();
        } else {
            Pause();
        }
    }

    public void Resume(){
        Time.timeScale = 1f;
        GameIsPaused = false;
        this.gameObject.SetActive(false);
        PauseEvent.resumeFromPause();
    }

    void Pause(){
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu(){
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameMenu");
    }

    public void ExitGame(){
        Application.Quit();
    }
}
