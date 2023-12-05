using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace GDI.WorldInteraction {
    public class WorldInteraction : MonoBehaviour {
            public static WorldInteraction instance;
            public GameObject player;
            private List<Transform> selectedCharacter = new List<Transform>();
            private GameObject previousSelected;
            public int hasBattleStarted;
            bool isSelected;
            public GameObject enemy;
            private RaycastHit hit;
            public int combatMode;
            public Transform triggeredEnemy;

            void Awake(){
                instance = this;
                isSelected = false;
                combatMode = 0;
                hasBattleStarted = 0;
            }

            void Update(){
                if(Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() && combatMode == 0){
                    getInteraction();
                }

                if(Input.GetMouseButtonDown(1)){
                    deselectCharacter();
                }

            }

            public void beginCombatMode(int hasAggro, Transform enemy) {
                if(hasAggro==1 && hasBattleStarted==0){
                    hasBattleStarted = 1;
                    GridGenerator.instance.GenerateMap();
                    for (int i=0; i<selectedCharacter.Count; i++) {
                        selectedCharacter[i].GetComponent<NavMeshAgent>().isStopped = true;
                        selectedCharacter[i].GetComponent<NavMeshAgent>().destination = selectedCharacter[i].transform.position;
                    }
                    triggeredEnemy = enemy;
                    CombatEvent.variableChange();
                    UIEventHandler.sendMessageToUI("Combat Begins");
                    CombatEvent.turnChangeInitiative();
                }
                deselectCharacter();
            }

            void getInteraction(){
                Ray interactionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                if(Physics.Raycast(interactionRay, out hit, Mathf.Infinity)){
                    GameObject interactedObject = hit.collider.gameObject;
                    if(interactedObject.tag == "Interactable Object"){
                        if(isSelected){
                            for (int i=0; i<selectedCharacter.Count; i++) {
                                interactedObject.GetComponent<Interactable>().moveToInteraction(selectedCharacter[i].GetComponent<NavMeshAgent>());
                            }
                        }
                    } else if (interactedObject.tag == "Character") {
                        isSelected = true;
                        selectCharacter(hit.transform, Input.GetKey(KeyCode.LeftShift));
                    } else {
                        if (isSelected){
                            for (int i=0; i<selectedCharacter.Count; i++) {
                                selectedCharacter[i].GetComponent<CharacterPlayer>().movePlayerFree(hit.point);
                            }
                        }
                    }
                }
            }

            private void selectCharacter(Transform character, bool canMultiSelect=false){
                if (!canMultiSelect) {
                    deselectCharacter();
                }
                selectedCharacter.Add(character);
                UIEventHandler.playerSelectedChanged(character.gameObject);
                character.Find("Highlight").gameObject.SetActive(true);
            }

            private void deselectCharacter(){
                for (int i=0; i<selectedCharacter.Count; i++) {
                    selectedCharacter[i].Find("Highlight").gameObject.SetActive(false);
                }
                UIEventHandler.playerDeselectedChanged();
                selectedCharacter.Clear();
            }

            public Transform whoIsSelected(){
                if(hasBattleStarted==0){
                    if (selectedCharacter.Count > 0) {
                        previousSelected = selectedCharacter[0].gameObject;
                        return selectedCharacter[0];
                    }
                } else {
                    Transform whoPlays = null;
                    if(CombatController.instance.sortedInitiative.Count == 0) {
                        whoPlays = previousSelected.transform;
                    }else {
                        whoPlays = (CombatController.instance.sortedInitiative[CombatController.instance.currentTurn].Key);
                    }
                
                    GridGenerator.instance.highlightCharacter(whoPlays);
                    if(whoPlays.tag=="Character"){
                        UIEventHandler.playerSelectedChanged(whoPlays.gameObject);
                    }
                    whoPlays.Find("Highlight").gameObject.SetActive(true);
                    return whoPlays;
                }
                return null;
            }

        }
}
