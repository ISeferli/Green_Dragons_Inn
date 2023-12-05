
public class AnswerGoal : Goal {
    public string qAnswer { get; set; }

    void OnDestroy(){
        UIEventHandler.OnCheckRiddleQuest -= answerCheck;
    }

    public AnswerGoal(Quest quest, string Answer, string description, bool completed){
        this.quest = quest;
        this.qAnswer = Answer;
        this.description = description;
        this.completed = completed;
    }

    public override void init(){
        base.init();
        UIEventHandler.OnCheckRiddleQuest += answerCheck;
    }

    void answerCheck(string answer){
        if(this.qAnswer.Equals(answer.ToLower())){
            this.completed = true;
            complete();
        }
    }

}
