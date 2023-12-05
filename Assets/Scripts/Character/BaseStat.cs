using System.Collections.Generic;

public class BaseStat {
    public enum BaseStatType{
        Constitution,
        Dexterity,
        Strength,
        Intelligence,
        Wisdom
    }
    
    public List<BonusStat> baseAdditives{ get; set; }
    public BaseStatType statType { get; set; }
    public int baseValue { get; set; }
    public string statName { get; set; }
    public string statDescription { get; set; }
    public int finalValue { get; set; }

    public BaseStat(int baseValue, string statName, string statDescription) {
        this.baseAdditives = new List<BonusStat>();
        foreach (BaseStatType value in System.Enum.GetValues(typeof(BaseStatType))){
            if (value.ToString() == statName){
                statType = value;
                break;
            }
        }
        this.statType = statType;
        this.baseValue = baseValue;
        this.statName = statName;
        this.statDescription = statDescription;
    }

    public BaseStat(int baseValue, string statName){
        this.baseAdditives = new List<BonusStat>();
        foreach (BaseStatType value in System.Enum.GetValues(typeof(BaseStatType))){
            if (value.ToString() == statName){
                statType = value;
                break;
            }
        }
        this.statType = statType;
        this.baseValue = baseValue;
        this.statName = statName;
        this.statDescription = "";
    }

    public void addStatBonus(BonusStat statBonus) {
        this.baseAdditives.Add(statBonus);
    }

    public void removeStatBonus(BonusStat statBonus) {
        this.baseAdditives.Remove(baseAdditives.Find(x => x.bonusValue == statBonus.bonusValue));
    }

    public int getCalculatedStatValue() {
        int helpFindFinal = 0;
        this.baseAdditives.ForEach(x => helpFindFinal += x.bonusValue);
        finalValue = helpFindFinal;
        finalValue += baseValue;
        return finalValue;
    }

}
