using UnityEngine;
using UnityEngine.AI;

public class Interactable : MonoBehaviour {
    public NavMeshAgent playerAgent;
    private bool hasInteracted;

    public virtual void moveToInteraction(NavMeshAgent playerAgent){
        hasInteracted = false;
        this.playerAgent = playerAgent;
        playerAgent.stoppingDistance = 3f;
        playerAgent.destination = this.transform.position;
    }

    public virtual void interact(){
        Debug.Log("Interacting with base class");
    }

    void Update(){
        if(!hasInteracted && playerAgent!=null && !playerAgent.pathPending){
            if(playerAgent.remainingDistance <= playerAgent.stoppingDistance){
                hasInteracted = true;
                interact();
            }
        }
    }
}
