using UnityEngine;
using GDI.WorldInteraction;

public class InventoryUI : MonoBehaviour {
    public RectTransform inventoryPanel;
    public RectTransform scrollViewContent;
    InventoryUIItem itemContainer { get; set; }
    public bool menuIsActive { get; set; }
    Item currentSelectedItem { get; set; }
    
    void Start() {
        itemContainer = Resources.Load<InventoryUIItem>("UI/ItemContainer");
        UIEventHandler.OnItemAddedToInventory += itemAdded;
        inventoryPanel.gameObject.SetActive(false);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.I) && WorldInteraction.instance.whoIsSelected()!=null) {
            menuIsActive = !menuIsActive;
            inventoryPanel.gameObject.SetActive(menuIsActive);
        }
    }

    void itemAdded(Item item){
        InventoryUIItem emptyItem = Instantiate(itemContainer);
        emptyItem.setItem(item);
        emptyItem.transform.SetParent(scrollViewContent);
    }
}
