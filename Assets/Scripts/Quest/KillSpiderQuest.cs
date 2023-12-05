
public class KillSpiderQuest : Quest {
    public void Awake(){
        questName = "Kill Spiders";
        description = "Kill the spiders that terrorized the citizen";
        itemReward = ItemDatabase.instance.getItem("Bow");
        experienceReward = 50;
        questRiddle = false;

        goals.Add(new KillGoal(this, 0, "Kill Spiders", false, 0, 9));
        goals.ForEach(g => g.init());

        startDialogue = new string[] {"Oh, no, what am I going to do.", "I need to head to Sowtale, the first town after the woods but there are spiders in the way.", "Can you handle them?"};
        inProgressDialogue = new string[] {"Please, help me and my brother."};
        rewardDialogue = new string[] {"Thank you, THANK YOU.", "Here's your reward as promised."};
        completedDialogue = new string[] {"Thanks for that one time, it's time for me to head out soon"};
    }
}
