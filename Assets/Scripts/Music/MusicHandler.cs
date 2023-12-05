using UnityEngine;

public class MusicHandler : MonoBehaviour
{
    [SerializeField] AudioSource audioHandler;

    void OnDestroy(){
        CombatEvent.OnVariableChange -= battleMusic;
        CombatEvent.OnVariableChangeOff -= backgroundMusic;   
    }

    void Awake(){
        CombatEvent.OnVariableChange += battleMusic;
        CombatEvent.OnVariableChangeOff += backgroundMusic;   
    }

    private void backgroundMusic(){
        audioHandler.Stop();
        audioHandler.clip = Resources.Load<AudioClip>("Music/Pillars of Eternity - Elmshore");
        audioHandler.Play();
    }

    private void battleMusic(){
        audioHandler.Stop();
        audioHandler.clip = Resources.Load<AudioClip>("Music/Volatile Reaction_fight");
        audioHandler.Play();
    }
}
