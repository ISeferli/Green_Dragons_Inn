using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GDI.WorldInteraction;

public class InventoryManager : MonoBehaviour {
    public static InventoryManager instance { get; set; }

    public Transform itemInMap;
    private PlayerWeaponController playerWeaponController;
    private ConsumableController consumableController;
    public InventoryUIDetails inventoryDetailsPanel;

    public List<Item> playerItems = new List<Item>();

    void Start(){
        if (instance!=null && instance!=this) {
            Destroy(gameObject);
        } else {
            instance = this;
        }
        ItemDatabase.instance.setItemStats(itemInMap);
        addItemInInventory("Sword");
        addItemInInventory("Bow");
        addItemInInventory("Potion");
        addItemInInventory("LightPotion");
    }

    public void addItemInInventory(string itemSlug) {
        Item item = ItemDatabase.instance.getItem(itemSlug);
        playerItems.Add(item);
        UIEventHandler.itemAddedToInventory(item);
    }

    public void addItemInInventory(Item item) {
        playerItems.Add(item);
        UIEventHandler.itemAddedToInventory(item);
    }

    public void equipItem(Item itemToEquip) {
        if (WorldInteraction.instance.whoIsSelected()!=null) {
            playerWeaponController = WorldInteraction.instance.whoIsSelected().GetComponent<PlayerWeaponController>();
        }
        playerItems.Remove(itemToEquip);
        playerWeaponController.equipWeapon(itemToEquip);
    }

    public void consumeItem(Item itemToConsume) {
        if (WorldInteraction.instance.whoIsSelected()!=null) {
            consumableController = WorldInteraction.instance.whoIsSelected().GetComponent<ConsumableController>();
        }
        consumableController.consumeItem(WorldInteraction.instance.whoIsSelected(), itemToConsume);
    }

    public void setItemDetails(Item item, Button selectedButton){
        inventoryDetailsPanel.gameObject.SetActive(false);
        inventoryDetailsPanel.setItem(item, selectedButton);
    }
}
