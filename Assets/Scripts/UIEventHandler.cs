using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEventHandler : MonoBehaviour {
    public delegate void ItemEvenHandler(Item item);
    public static event ItemEvenHandler OnItemAddedToInventory;
    public static event ItemEvenHandler OnItemEquipped;

    public delegate void ItemEventUnequipHandler();
    public static event ItemEventUnequipHandler OnItemUnequippedAfter;

    public delegate void PlayerHealthEventHandler(int maxHealth, int currentHealth);
    public static event PlayerHealthEventHandler OnPlayerHealthChanged;

    public delegate void PlayerStatsEventHandler(CharacterStat playerStats, int added);
    public static event PlayerStatsEventHandler OnStatsChanged;

    public delegate void PlayerExperienceHandler(int maxExp, int currentExp, int level);
    public static event PlayerExperienceHandler OnPlayerEXP;

    public delegate void LevelUpHandler();
    public static event LevelUpHandler OnLevelUpTime;
    public static event LevelUpHandler OnCompletedLevelUp;

    public delegate void LevelUpUIHandler(CharacterStat player);
    public static event LevelUpUIHandler OnLevelUpUIActivated;

    public delegate void PlayerSelectedChanged(GameObject player);
    public static event PlayerSelectedChanged OnPlayerSelectedChanged;
    public static event PlayerSelectedChanged OnPlayerSelectedTile;

    public delegate void PlayerDeselectedChanged();
    public static event PlayerDeselectedChanged OnPlayerDeselectedChanged;

    public delegate void WriteMessageToBoard(string text);
    public static event WriteMessageToBoard OnWriteMessageToBoard;

    public delegate void WriteQuestToBoard(string text, int curAmount, int reqAmount);
    public static event WriteQuestToBoard OnWriteQuestToBoard;
    public static event WriteQuestToBoard OnCompleteQuest;

    public delegate void ProgressOnGoalHandler(string text, int cur, int req);
    public static event ProgressOnGoalHandler OnProgressGoal;

    public delegate void CheckRiddleAnswerHandler(string text);
    public static event CheckRiddleAnswerHandler OnCheckRiddleQuest;


    public static void itemAddedToInventory(Item item){
        OnItemAddedToInventory(item);
    }

    public static void itemEquipped(Item item){
        OnItemEquipped(item);
    }

    public static void itemUnequip(){
        OnItemUnequippedAfter();
    }

    public static void healthChanged(int maxHealth, int currentHealth){
        OnPlayerHealthChanged(maxHealth, currentHealth);
    }

    public static void statsChanged(CharacterStat playerStats, int added){
       OnStatsChanged(playerStats, added);
    }

    public static void experienceRaise(int maxExp, int currentExp, int level){
        OnPlayerEXP(maxExp, currentExp, level);
    }

    public static void levelUpPanelAct(CharacterStat player){
        OnLevelUpUIActivated(player);
    }

    public static void levelUpTime(){
        OnLevelUpTime();
    }

    public static void levelUpCompleted(){
        OnCompletedLevelUp();
    }

    public static void playerSelectedChanged(GameObject player){
        OnPlayerSelectedChanged(player);
    }

    public static void playerDeselectedChanged(){
        OnPlayerDeselectedChanged();
    }

    public static void playerChangedTile(GameObject player){
        OnPlayerSelectedTile(player);
    }

    public static void sendMessageToUI(string text){
        OnWriteMessageToBoard(text);
    }

    public static void writeQuestToUI(string text, int curAmount, int reqAmount){
        OnWriteQuestToBoard(text, curAmount, reqAmount);
    }

    public static void removeQuestFromUI(string text, int curAmount, int reqAmount){
        OnCompleteQuest(text, curAmount, reqAmount);
    }

    public static void goalProgressHap(string text, int cur, int req){
        OnProgressGoal(text, cur, req);
    }

    public static void checkRiddleQuest(string text){
        OnCheckRiddleQuest(text);
    }
}
