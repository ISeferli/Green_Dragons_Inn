using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GDI.WorldInteraction;

public class InventoryUIDetails : MonoBehaviour {
    Item item;
    Button selectedItemButton, itemInteractButton;
    TMP_Text itemNameText, itemDescriptionText, itemInteractButtonText;

    public TMP_Text statText;

    void Start(){
        itemNameText = transform.GetChild(0).GetComponent<TMP_Text>();
        itemDescriptionText = transform.GetChild(1).GetComponent<TMP_Text>();
        itemInteractButton = transform.GetChild(2).GetComponent<Button>();
        itemInteractButton.onClick.AddListener(OnItemInteract);
        itemInteractButtonText = itemInteractButton.transform.GetChild(0).GetComponent<TMP_Text>();
        gameObject.SetActive(false);
    }

    public void setItem(Item item, Button selectedButton){
        gameObject.SetActive(true);
        statText.text = "";
        this.item = item;
        if (item.itemModifier != 0) {
            statText.text += item.statName + ": " + item.baseValue + "\n";
        }
        selectedItemButton = selectedButton;
        itemNameText.text = item.itemName;
        if(WorldInteraction.instance.combatMode==0) {
            itemInteractButton.interactable = true;
        }
        if(WorldInteraction.instance.gameObject.GetComponent<CombatController>().actionTaken==false){
            itemInteractButton.interactable = true;
        } else {
            itemInteractButton.interactable = false;
        }
        itemDescriptionText.text = item.description;
        itemInteractButtonText.text = item.actionName;
    }

    public void OnItemInteract(){
        if (item.itemType == "Potion" || item.itemType == "LightPotion") {
            if(WorldInteraction.instance.combatMode==1){
                if(WorldInteraction.instance.gameObject.GetComponent<CombatController>().actionTaken==false){
                    InventoryManager.instance.consumeItem(item);
                    WorldInteraction.instance.gameObject.GetComponent<CombatController>().actionTaken = true;
                    Destroy(selectedItemButton.gameObject);
                }
            } else {
                InventoryManager.instance.consumeItem(item);
                Destroy(selectedItemButton.gameObject);
            }
        } else if (item.itemType == "Sword" || item.itemType == "Bow") {
            if(WorldInteraction.instance.combatMode==1){
                if(WorldInteraction.instance.gameObject.GetComponent<CombatController>().actionTaken==false){
                    InventoryManager.instance.equipItem(item);
                    WorldInteraction.instance.gameObject.GetComponent<CombatController>().actionTaken = true;
                    Destroy(selectedItemButton.gameObject);
                }
            } else {
                InventoryManager.instance.equipItem(item);
                Destroy(selectedItemButton.gameObject);
            }
        }
        gameObject.SetActive(false);
    }
}
