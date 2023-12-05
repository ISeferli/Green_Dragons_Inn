using UnityEngine;

public class PickupItem : Interactable {
    public Item itemDrop {get;set;}
    public AudioClip audioWhenAppear;

    public override void interact()
    {
        InventoryManager.instance.addItemInInventory(itemDrop);
        Destroy(gameObject);
    }
}
