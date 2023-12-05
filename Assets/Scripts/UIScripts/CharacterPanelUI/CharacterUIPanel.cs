using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CharacterUIPanel : MonoBehaviour {
    [SerializeField] private TMP_Text health, level;
    [SerializeField] private Image healthFill, levelFill;
    [SerializeField] private GameObject player;
    [SerializeField] private Button levelUpButton;
    [SerializeField] private RectTransform levelUpPanel;

    //Stats
    private List<TMP_Text> playerStatTexts = new List<TMP_Text>();
    [SerializeField] private TMP_Text playerStatPrefab;
    [SerializeField] private Transform playerStatPanel;
    private bool panelIsActive { get; set; }
    private GameObject playerSelected;

    //Equipped Weapon
    private PlayerWeaponController playerWeaponController;
    [SerializeField] private Sprite defaultWeaponIcon;
    [SerializeField] private TMP_Text weaponStatPrefab;
    [SerializeField] private Transform weaponStatPanel;
    [SerializeField] private TMP_Text weaponNameText;
    [SerializeField] private Image weaponIcon;
    [SerializeField] private TMP_Text weaponStatTexts;

    void OnDestroy(){
        UIEventHandler.OnPlayerHealthChanged -= updateHealth;
        UIEventHandler.OnStatsChanged -= updateStats;
        UIEventHandler.OnItemEquipped -= updateItemEquipped;
        UIEventHandler.OnPlayerEXP -= updateLevel;
        UIEventHandler.OnLevelUpTime -= activateButtonLevel;
        UIEventHandler.OnItemUnequippedAfter -= clearWeaponText;
        UIEventHandler.OnPlayerSelectedChanged -= playerChanged;
        UIEventHandler.OnCompletedLevelUp -= levelUpCompleted;
    }

    void Awake(){
        UIEventHandler.OnPlayerHealthChanged += updateHealth;
        UIEventHandler.OnStatsChanged += updateStats;
        UIEventHandler.OnItemEquipped += updateItemEquipped;
        UIEventHandler.OnPlayerEXP += updateLevel;
        UIEventHandler.OnLevelUpTime += activateButtonLevel;
        UIEventHandler.OnItemUnequippedAfter += clearWeaponText;
        UIEventHandler.OnPlayerSelectedChanged += playerChanged;
        UIEventHandler.OnCompletedLevelUp += levelUpCompleted;
        levelUpPanel.gameObject.SetActive(false);
        levelUpButton.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }

    void Start(){
        weaponIcon.sprite = defaultWeaponIcon;
    }

    void playerChanged(GameObject player){
        playerSelected = player;
        playerWeaponController = playerSelected.GetComponent<PlayerWeaponController>();
        initializeStats();
        levelUpCompleted();
        updateHealth(playerSelected.GetComponent<CharacterStat>().healthPoints, playerSelected.GetComponent<CharacterPlayer>().currentHealth);
        updateLevel(playerSelected.GetComponent<CharacterLevel>().requiredExperience, playerSelected.GetComponent<CharacterLevel>().currentExperience, playerSelected.GetComponent<CharacterLevel>().level);
        if(playerSelected.GetComponent<PlayerWeaponController>().hasWeapon()){
            clearWeaponText();
            updateItemEquipped(playerSelected.GetComponent<PlayerWeaponController>().currentlyEquippedWeapon);
        } else {
            clearWeaponText();
        }
    }

    void activateButtonLevel(){
        levelUpButton.gameObject.SetActive(true);
    }

    void updateHealth(int maxHealth, int currentHealth){
        this.health.text = currentHealth.ToString();
        this.healthFill.fillAmount = (float)currentHealth/(float)maxHealth;
    }

    void updateLevel(int maxExp, int currentExp, int level){
        this.level.text = level.ToString();
        this.levelFill.fillAmount = (float)currentExp/(float)maxExp;
    }

    void initializeStats(){
        // Clear the existing playerStatTexts list
        playerStatTexts.Clear();

        // Destroy any existing UI elements
        for (int i = 0; i < playerStatPanel.childCount; i++){
            Destroy(playerStatPanel.GetChild(i).gameObject);
        }
        
        for(int i=0; i< playerSelected.GetComponent<CharacterStat>().attributes.Count; i++){
            playerStatTexts.Add(Instantiate(playerStatPrefab));
            playerStatTexts[i].transform.SetParent(playerStatPanel);
        }
        updateStats(playerSelected.GetComponent<CharacterStat>(), 0);
    }

    void updateStats(CharacterStat playerPanel, int added){
        int i = 0;
        int size = 0;
        if(added == 1){
            size = playerStatTexts.Count;
            playerStatTexts.Add(Instantiate(playerStatPrefab));
            playerStatTexts[size].transform.SetParent(playerStatPanel);
        } else if (added == 2) {
            int j = 0;
            foreach(TMP_Text text in playerStatTexts){
                if(!playerPanel.attributes.ContainsKey(returnString(text.ToString()))){
                    playerStatTexts.Remove(text);
                    Destroy(playerStatPanel.GetChild(j).gameObject);
                    break;
                }
                j++;
            }
        }

        foreach(var content in playerPanel.attributes){
            playerStatTexts[i].text = playerPanel.attributes[content.Key].statName + ": " + playerPanel.attributes[content.Key].getCalculatedStatValue().ToString();
            i++;
        }
    }

    private string returnString(string text){
        string inputString = text;
        string[] parts = inputString.Split(": ");
        string wantedString = parts[0].Trim();
        return wantedString;
    }

    void updateItemEquipped(Item item){
        weaponIcon.sprite = Resources.Load<Sprite>("Icons/" + item.objectSlug);
        weaponNameText.text = item.itemName;
        weaponStatTexts = Instantiate(weaponStatPrefab);
        weaponStatTexts.transform.SetParent(weaponStatPanel);
        weaponStatTexts.text = item.statName + ": " + item.baseValue;
    }

    public void unequipWeapon(){
        if(playerWeaponController.hasWeapon()){
            clearWeaponText();
            playerWeaponController.unequipWeapon();
        }
    }

    public void levelUpButtonPressed(){
        levelUpPanel.gameObject.SetActive(true);
        UIEventHandler.levelUpPanelAct(playerSelected.GetComponent<CharacterStat>());
    }

    public void levelUpCompleted(){
        if(!playerSelected.GetComponent<CharacterLevel>().leveledUp){
            levelUpButton.gameObject.SetActive(false);
        } else {
            levelUpButton.gameObject.SetActive(true);
        }
    }

    private void clearWeaponText(){
        weaponNameText.text = "";
        weaponIcon.sprite = defaultWeaponIcon;
        if(weaponStatTexts!=null){
            Destroy(weaponStatTexts.gameObject);
        }
    }

}
