using UnityEngine;

public class PlayerWeaponController : MonoBehaviour{
    CharacterStat characterStat;
    public Item currentlyEquippedWeapon;
    private bool hasWeaponEquipped;

    void Start(){
        this.characterStat = GetComponent<CharacterStat>();
        this.hasWeaponEquipped = false;
    }

    public void equipWeapon(Item itemToEquip){
        if(hasWeapon()){
            UIEventHandler.itemUnequip();
            unequipWeapon();
        }
        currentlyEquippedWeapon = itemToEquip;
        hasWeaponEquipped = true;
        int addedNewStat = characterStat.addSingleStatBonus(itemToEquip.statName, itemToEquip.baseValue);
        UIEventHandler.itemEquipped(itemToEquip);
        UIEventHandler.statsChanged(characterStat, addedNewStat);
    }

    public void performWeaponAttack(GameObject personToHit, GameObject whoPlays){
        if(currentlyEquippedWeapon==null){
            UIEventHandler.sendMessageToUI("" + characterStat.name + " does not have a weapon equipped.");
        } else {
            currentlyEquippedWeapon.GetComponent<WeaponInterface>().performAttack(personToHit, whoPlays);
        }
    }

    public void unequipWeapon(){
        hasWeaponEquipped = false;
        InventoryManager.instance.addItemInInventory(currentlyEquippedWeapon);
        int removedStat = characterStat.removeStatBonus(currentlyEquippedWeapon.statName, currentlyEquippedWeapon.baseValue);
        currentlyEquippedWeapon = null;
        UIEventHandler.statsChanged(characterStat, removedStat);
    }

    public bool hasWeapon(){
        return hasWeaponEquipped;
    }
}
