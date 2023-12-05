using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryUIItem : MonoBehaviour {
    public Item item;

    public TMP_Text itemText;
    public Image itemIcon;

    public void setItem(Item item){
        this.item = item;
        setUpItemValues();
    }

    void setUpItemValues(){
        itemText.text = item.itemName;
        itemIcon.sprite = Resources.Load<Sprite>("Icons/" + item.objectSlug);
    }

    public void OnSelectItemButton(){
        InventoryManager.instance.setItemDetails(item, GetComponent<Button>());
    }
}
