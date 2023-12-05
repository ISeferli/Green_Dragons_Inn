using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    [SerializeField] private GameState startingState;
    public GameState gameState { get; private set; }
    public PlayerManager playerManager;

    void Awake(){
        if(instance!=null && instance!=this){
            Destroy(gameObject);
            return;
        } else {
            instance = this;
            DontDestroyOnLoad(this);
        }

        // Set up game state
        gameState = Instantiate(startingState);
        playerManager.GameState = gameState;
    }

}
