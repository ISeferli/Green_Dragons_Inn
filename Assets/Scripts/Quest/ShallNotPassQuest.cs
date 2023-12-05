public class ShallNotPassQuest : Quest {
    public void Awake(){
        questName = "Answer Riddle";
        description = "Answer the riddle of the Gatekeeper";
        itemReward = ItemDatabase.instance.getItem("Potion");
        experienceReward = 0;
        questRiddle = true;
        questAnswer = "e";

        goals.Add(new AnswerGoal(this, questAnswer, "Answer Riddle", false));
        goals.ForEach(g => g.init());

        startDialogue = new string[] {"Greetings.", "My name is the Gatekeeper.", "To get passage and fight the Orc Shaman, you must solve my riddle, to prove your worth."};
        inProgressDialogue = new string[] {"I am the beginning of everything, the end of everywhere. I'm the beginning of eternity, the end of time & space. What am I?"};
        rewardDialogue = new string[] {"You have proven your worth, adventurer.", "Here's your reward."};
        completedDialogue = new string[] {"Good luck fighting the Orc."};
    }
}
