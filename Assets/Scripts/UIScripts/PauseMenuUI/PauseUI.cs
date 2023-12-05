using UnityEngine;

public class PauseUI : MonoBehaviour {
    public RectTransform pausePanel;
    public bool pauseIsActive { get; set; }

    void onDestroy(){
        PauseEvent.OnResumeButtonTrigger -= changeBool;
    }

    void Start() {
        PauseEvent.OnResumeButtonTrigger += changeBool;
        pauseIsActive = false;
        pausePanel.gameObject.SetActive(pauseIsActive);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if(!pauseIsActive){
                pauseIsActive = !pauseIsActive;
            }
            pausePanel.gameObject.SetActive(pauseIsActive);
            PauseEvent.pauseTriggered();
        }
    }

    private void changeBool(){
        pauseIsActive = !pauseIsActive;
    }

}
