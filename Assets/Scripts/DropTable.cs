using System.Collections.Generic;
using UnityEngine;

public class DropTable {
    public List<LootDrop> loot;

    public Item getDrop(){
        int roll = Random.Range(0, 101);
        int weightSum = 0;
        foreach(LootDrop drop in loot){
            weightSum += drop.weight;
            if(roll < weightSum){
                return ItemDatabase.instance.getItem(drop.itemSlug);
            }
        }
        return null;
    }
}

public class LootDrop{
    public string itemSlug { get; set; }
    public int weight { get; set; }

    public LootDrop(string itemSlug, int weight){
        this.itemSlug = itemSlug;
        this.weight = weight;
    }
}
