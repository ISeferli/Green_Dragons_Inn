public class NPC : Interactable {
    public string[] dialogue;
    public string npcName;

    public override void interact(){
        DialogueManager.instance.addDialogue(dialogue, npcName);
    }
}
