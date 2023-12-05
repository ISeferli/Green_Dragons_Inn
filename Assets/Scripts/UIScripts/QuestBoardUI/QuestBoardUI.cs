using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestBoardUI : MonoBehaviour
{
    List<Message> messageList = new List<Message>();
    public GameObject chatPanel, textObject;

    public void Start(){
        UIEventHandler.OnWriteQuestToBoard += SendQuestTrigger;
        UIEventHandler.OnCompleteQuest += RemoveQuestTrigger;
        UIEventHandler.OnProgressGoal += IncreaseTheGoal;
    }

    public void SendQuestTrigger(string text, int cur, int req) {
        SendQuestToChat(text, cur, req);
    }

    public void RemoveQuestTrigger(string text, int cur, int req) {
        RemoveMessageFromBoard(text, cur, req);
    }

    public void SendQuestToChat(string text, int cur, int req){
        Message newMessage = new Message();
        newMessage.text = text;
        GameObject newText = Instantiate(textObject, chatPanel.transform);
        newMessage.textObject = newText.GetComponent<TMP_Text>();
        newMessage.textObject.text = newMessage.text;
        messageList.Add(newMessage);
    }

    public void RemoveMessageFromBoard(string text, int cur, int req){
        for (int i = messageList.Count - 1; i >= 0; i--){
            Message mes = messageList[i];
            if (mes.text.Equals(text)) {
                Destroy(mes.textObject.gameObject);
                messageList.RemoveAt(i);
            }
        }
    }

    public void IncreaseTheGoal(string text, int cur, int req){
        foreach(Message mes in messageList){
            if(mes.text.Equals(text)){
                UIEventHandler.sendMessageToUI("Goal: (" + cur + "/ " + req + ")");
            }
        }
    }
}

[System.Serializable]
public class QuestMessage{
    public string text;
    public TMP_Text textObject;
}
