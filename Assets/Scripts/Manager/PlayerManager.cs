using System.Collections.Generic;
using GDI.CharacterClasses;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "PlayerManager", menuName = "PlayerManager")]
public class PlayerManager : ScriptableObject {
    public GameState GameState { get; set; }
    public GameObject ActivePlayer;
    public string spawnTag = "SpawnCharacter";
    public static string levelManagerName;
    public Dictionary<CharacterClasses, Dictionary<string, int>> charactersToPlay;

    void OnEnable(){
       SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        if (scene.name == levelManagerName){
            SpawnPlayer();
        }
    }

    protected void SpawnPlayer(){
        if(GameState.PlayerSpawnLocation != ""){
            GameObject[] spawns = GameObject.FindGameObjectsWithTag(spawnTag);
            bool foundSpawn = false;
            foreach(GameObject spawn in spawns){
                if(spawn.name == GameState.PlayerSpawnLocation){
                    Vector3 count = new Vector3(0, 0, 0);
                    foundSpawn = true;
                    GameObject[] parents = GameObject.FindGameObjectsWithTag("Character");
                    foreach(KeyValuePair<CharacterClasses, Dictionary<string, int>> player in charactersToPlay){
                        ActivePlayer = Instantiate(player.Key.classPrefab, spawn.transform.position + count, Quaternion.identity);
                        ActivePlayer.transform.SetParent(parents[0].transform.Find((player.Key.avClass+"s").ToLower()));
                        CharacterDatabase.instance.setCharacterAttributes(ActivePlayer.transform, player.Key.avClass.ToString(), player.Value);
                        count.x += 5;
                    }
                  break;
                }
            }

            if(!foundSpawn){
                throw new MissingReferenceException("Could not find the player spawn");
            }
        } 
    }

    void OnDisable(){
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

}
