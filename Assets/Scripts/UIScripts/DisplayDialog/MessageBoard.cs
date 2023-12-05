using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageBoard : MonoBehaviour{
    public int maxMessages = 25;
    List<Message> messageList = new List<Message>();
    public GameObject chatPanel, textObject;
    public ScrollRect chatRect;

    void OnDestroy(){
        UIEventHandler.OnWriteMessageToBoard -= SendMessageTrigger;
    }

    public void Start(){
        UIEventHandler.OnWriteMessageToBoard += SendMessageTrigger;
    }

    public void SendMessageTrigger(string text) {
        SendMessageToChat(text);
    }

    public void SendMessageToChat(string text){
        if(messageList.Count >= maxMessages){
            Destroy(messageList[0].textObject.gameObject);
            messageList.Remove(messageList[0]);
        }
        Message newMessage = new Message();
        newMessage.text = text;
        GameObject newText = Instantiate(textObject, chatPanel.transform);
        newMessage.textObject = newText.GetComponent<TMP_Text>();
        newMessage.textObject.text = newMessage.text;
        messageList.Add(newMessage);
    }
}


[System.Serializable]
public class Message{
    public string text;
    public TMP_Text textObject;
}

