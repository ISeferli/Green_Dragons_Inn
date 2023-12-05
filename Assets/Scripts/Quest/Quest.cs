using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Quest : MonoBehaviour {
    public List<Goal> goals { get; set; } = new List<Goal>();
    public string[] startDialogue, inProgressDialogue, rewardDialogue, completedDialogue;
    public string questName { get; set; }
    public string description { get; set; }
    public int experienceReward { get; set; }
    public Item itemReward { get; set; }
    public bool completed { get; set; }
    public bool questRiddle { get; set; }
    public string questAnswer { get; set; }
    public GameObject wallDestroy { get; set; }

    public void checkGoals(){
        this.completed = goals.All(g => g.completed);
    }

    public void giveReward(){
        if(itemReward!=null){
            InventoryManager.instance.addItemInInventory(itemReward);
        }
    }

    public Goal findSpecificGoal(string questName){
        foreach(Goal goal in goals){
            if(goal.quest.questName.Equals(questName)){
                return goal;
            }
        }
        return null;
    }
}
