using UnityEngine;

public class ConsumableController : MonoBehaviour {
    CharacterStat stats;

    void Start() {
        stats = GetComponent<CharacterStat>();
    }

    public void consumeItem(Transform player, Item item){
        GameObject itemToSpawn = Instantiate(Resources.Load<GameObject>("Items/" + item.objectSlug));
        if (item.itemModifier==1) {
            itemToSpawn.GetComponent<ConsumablesInterface>().consume(player, item.baseValue, item.statName);
        } else {
            itemToSpawn.GetComponent<ConsumablesInterface>().consume(player);
        }
        Destroy(itemToSpawn);
    }

}