using System.Collections.Generic;
using UnityEngine;
using GDI.ItemsScript;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase instance;
    private List<Item> items { get; set; }

    [SerializeField]
    private ItemScript healthPotion, sword, bow, lightPotion;

    void Awake(){
        if (instance!=null && instance!=this) {
            Destroy(gameObject);
        } else {
            instance = this;
        }
        items = new List<Item>();
    }

    public (string itemName, string item_Slug, string description, string actionName, int itemModifier, int baseValue, string statName, string itemType) getItemStats (string itemSlug){
        ItemScript posItem;
        switch (itemSlug) {
            case "Sword":
                posItem = sword;
                break;
            case "Potion":
                posItem = healthPotion;
                break;
            case "Bow":
                posItem = bow;
                break;
            case "LightPotion": 
                posItem = lightPotion;
                break;
            default:
                Debug.Log($"Item type: {itemSlug} could not be found or does not exist!");
                return (null, itemSlug, null, null, 0, 0, null, null);
        }
        return (posItem.itName, itemSlug, posItem.description, posItem.actionName, posItem.itemModifier, posItem.baseValue, posItem.statName, posItem.avItem.ToString());
    }


    // When a new item needs to be addressed
    public void setItemStats(Transform type){
        foreach(Transform child in type){
            foreach (Transform item in child){
                string itemName = child.name.Substring(0, child.name.Length - 1);
                var itemScriptInfo = getItemStats(itemName);
                Item itemToGive = item.GetComponent<Item>();

                //Set stats for the new item
                itemToGive.itemName = itemScriptInfo.itemName;
                itemToGive.objectSlug = itemScriptInfo.item_Slug;
                itemToGive.description = itemScriptInfo.description;
                itemToGive.actionName = itemScriptInfo.actionName;
                itemToGive.itemModifier = itemScriptInfo.itemModifier;
                itemToGive.baseValue = itemScriptInfo.baseValue;
                itemToGive.statName = itemScriptInfo.statName;
                itemToGive.itemType = itemScriptInfo.itemType;
                items.Add(itemToGive);
            } 
        }
    }


    public Item getItem(string itemSlug){
        foreach(Item item in items){
            if(item.objectSlug == itemSlug){
                return item;
            }
        }
        return null;
    }
}
