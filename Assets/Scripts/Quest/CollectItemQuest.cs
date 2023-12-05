public class CollectItemQuest : Quest {
    public void Awake() {
        questName = "Collect Sword";
        description = "Collect the Sword that Sakumi lost in the desert and kill the Shaman Orc.";
        itemReward = ItemDatabase.instance.getItem("Potion");
        experienceReward = 100;

        goals.Add(new KillGoal(this, 1, "Collect Sword", false, 0, 1));
        goals.Add(new CollectionGoal(this, "Sword", "Collect Sword", false, 0, 1));
        goals.ForEach(g => g.init());

        startDialogue = new string[] {"Hello fellow adventurer.", "I lost my second sword in the desert and I'm afraid to go get it.", "Can you help me retrieve it? I think one of the monsters around here has it."};
        inProgressDialogue = new string[] {"How is the search going?"};
        rewardDialogue = new string[] {"Thank you."};
        completedDialogue = new string[] {"Thanks for the help that one time."};
    }
}
