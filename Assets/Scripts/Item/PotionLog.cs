using UnityEngine;

public class PotionLog : MonoBehaviour, ConsumablesInterface
{
    public void consume(Transform player)
    {
        UIEventHandler.sendMessageToUI("" + player.GetComponent<CharacterStat>().name + " drank a swig of the potion");
    }

    public void consume(Transform player, int baseValue, string statName){
        int intelligenceBonus = 0;
        if(player.GetComponent<CharacterStat>().attributes.ContainsKey("Intelligence")){
            intelligenceBonus = player.GetComponent<CharacterStat>().attributes["Intelligence"].getCalculatedStatValue();
        }

        if(statName.Equals("Constitution")){
            player.gameObject.GetComponent<CharacterPlayer>().healDamage((baseValue+intelligenceBonus));
            UIEventHandler.sendMessageToUI("" + player.GetComponent<CharacterStat>().name + " healed " + (baseValue+intelligenceBonus) + " points of HP");
        }
    }
}
