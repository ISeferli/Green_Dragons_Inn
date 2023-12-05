public class Goal {
    public Quest quest { get; set; }
    public string description { get; set; }
    public bool completed { get; set; }
    public int currentAmount { get; set; }
    public int requiredAmount { get; set; }
    public string riddleAnswer { get; set; }

    public virtual void init(){
    }

    public virtual void evaluate(){
        if(currentAmount >= requiredAmount){
            complete(); 
        }
    }

    public void complete(){
        quest.checkGoals();
        completed = quest.completed;
    }
}
