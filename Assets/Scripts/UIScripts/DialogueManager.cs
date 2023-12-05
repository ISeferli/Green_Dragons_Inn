using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour {
    public static DialogueManager instance { get; set;}
    public GameObject dialoguePanel;
    public string npcName;
    public List<string> dialogueLine = new List<string>();
    public GameObject riddleAnswer;

    Button contButton;
    TMP_Text dialogueText, nameText;
    TMP_InputField answerText;
    Button answerButton;
    int dialogueInd;

    void Awake(){
        contButton = dialoguePanel.transform.GetChild(0).GetComponent<Button>();
        dialogueText = dialoguePanel.transform.GetChild(1).GetComponent<TMP_Text>();
        nameText = dialoguePanel.transform.GetChild(2).GetChild(0).GetComponent<TMP_Text>();
        contButton.gameObject.SetActive(true);
        contButton.onClick.AddListener(delegate {continueDialogue();});

        answerText = riddleAnswer.transform.GetChild(0).GetComponent<TMP_InputField>();
        answerButton = riddleAnswer.transform.GetChild(1).GetComponent<Button>();
        answerButton.onClick.AddListener(delegate {answerDialogue();});
        riddleAnswer.gameObject.SetActive(false);
        dialoguePanel.SetActive(false);
        if(instance != null && instance != this){
            Destroy(gameObject);
        } else {
            instance = this;
        }
    }

    public void addDialogue(string[] dialogue, string npcName){
        dialogueInd = 0;
        dialogueLine = new List<string>(dialogue.Length);
        dialogueLine.AddRange(dialogue);
        this.npcName = npcName;
        createDialogue();
    }

    public void createDialogue(){
        dialoguePanel.SetActive(true);
        dialogueText.text = dialogueLine[dialogueInd];
        nameText.text = npcName;
    }

    public void continueDialogue(){
        if(dialogueInd < dialogueLine.Count-1){
            dialogueInd++;
            dialogueText.text = dialogueLine[dialogueInd];
        } else {
            dialoguePanel.SetActive(false);
        }
    }

    public void answerDialogue(){
        if(answerText.text != ""){
            UIEventHandler.checkRiddleQuest(answerText.text);
            answerText.text = "";
        }
        dialoguePanel.SetActive(false);
    }

    public void isRiddle(bool riddle){
        if(riddle){
            contButton.gameObject.SetActive(false);
            riddleAnswer.gameObject.SetActive(true);
        } else {
            contButton.gameObject.SetActive(true);
            riddleAnswer.gameObject.SetActive(false);
        }
    }
}
