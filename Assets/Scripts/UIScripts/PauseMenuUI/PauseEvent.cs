using UnityEngine;

public class PauseEvent : MonoBehaviour {
    public delegate void PauseEventHandler();
    public static event PauseEventHandler OnPauseTrigger;
    public static event PauseEventHandler OnResumeButtonTrigger;

    public static void pauseTriggered(){
        OnPauseTrigger();
    }

    public static void resumeFromPause(){
        OnResumeButtonTrigger();
    }
}
