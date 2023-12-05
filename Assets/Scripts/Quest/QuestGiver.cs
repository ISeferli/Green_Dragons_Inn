using UnityEngine;

public class QuestGiver : NPC {
    public bool AssignedQuest { get; set; }
    public bool HelpedQuest { get; set; }
    public Quest Quest { get; set; }
    public GameObject destroyWall;
    [SerializeField] private GameObject quests;
    [SerializeField] private string questType;

    public void Start(){
        AssignedQuest = false;
        HelpedQuest = false;
    }

    public override void interact() {
        if(!AssignedQuest && !HelpedQuest){
            AssignQuest();
        } else if (AssignedQuest && !HelpedQuest) {
            CheckQuest();
        } else {
            DialogueManager.instance.addDialogue(Quest.completedDialogue, npcName);
        }
    }

    void AssignQuest(){
        AssignedQuest = true;
        Quest = (Quest)quests.AddComponent(System.Type.GetType(questType));
        DialogueManager.instance.addDialogue(Quest.startDialogue, npcName);
        UIEventHandler.writeQuestToUI(Quest.description, Quest.findSpecificGoal(Quest.questName).currentAmount, Quest.findSpecificGoal(Quest.questName).requiredAmount);
    }

    void CheckQuest(){
        if(Quest.completed){
            if(Quest.questRiddle) {
                Destroy(destroyWall.gameObject);
            }
            Quest.giveReward();
            HelpedQuest = true;
            AssignedQuest = false;
            if(Quest.questRiddle) DialogueManager.instance.isRiddle(false);
            DialogueManager.instance.addDialogue(Quest.rewardDialogue, npcName);
            UIEventHandler.removeQuestFromUI(Quest.description, Quest.findSpecificGoal(Quest.questName).currentAmount, Quest.findSpecificGoal(Quest.questName).requiredAmount);
        } else {
            if(Quest.questRiddle) DialogueManager.instance.isRiddle(Quest.questRiddle);
            DialogueManager.instance.addDialogue(Quest.inProgressDialogue, npcName);
        }
    }
}
