public class CollectionGoal : Goal
{
    public string itemID { get; set; }
    void OnDestroy(){
        UIEventHandler.OnItemAddedToInventory -= itemPickedUp;
    }

    public CollectionGoal(Quest quest, string itemID, string description, bool completed, int currentAmount, int requiredAmount){
        this.quest = quest;
        this.itemID = itemID;
        this.description = description;
        this.completed = completed;
        this.currentAmount = currentAmount;
        this.requiredAmount = requiredAmount;
    }

    public override void init(){
        base.init();
        UIEventHandler.OnItemAddedToInventory += itemPickedUp;
    }

    void itemPickedUp(Item item){
        if(item.objectSlug == this.itemID){
            this.currentAmount++;
            checkCompletion();
            this.evaluate();
            UIEventHandler.goalProgressHap(quest.description, this.currentAmount, this.requiredAmount);
        }
    }

    void checkCompletion(){
        if(currentAmount >= requiredAmount){
            completed = true;
        }
    }
}
    