using System.Collections.Generic;
using UnityEngine;

public interface WeaponInterface {
    public List<BaseStat> stats { get; set; }
    void performAttack(GameObject personToHit, GameObject whoPlays);
    int criticalAttack(int baseDamage, int wisdom, CharacterStat whoPlays);
}
