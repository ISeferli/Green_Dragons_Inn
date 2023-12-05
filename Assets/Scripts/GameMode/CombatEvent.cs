using UnityEngine;

public class CombatEvent : MonoBehaviour
{
    public delegate void EnemyEventHandler(EnemyInterface enemy);
    public static event EnemyEventHandler OnEnemyDeath;

    public delegate void OnVariableChangeDelegate();
    public static event OnVariableChangeDelegate OnVariableChange;
    public static event OnVariableChangeDelegate OnVariableChangeOff;

    public delegate void OnChangeTurnHandler();
    public static event OnChangeTurnHandler OnTurnChange;

    public delegate void CharacterDeathHandler(CharacterPlayer player);
    public static event CharacterDeathHandler OnCharacterDeath;

    public static void enemyDied(EnemyInterface enemy){
        if(OnEnemyDeath!=null){
            OnEnemyDeath(enemy);
        }
    }

    public static void characterDied(CharacterPlayer character){
        OnCharacterDeath(character);
    }

    public static void variableChange(){
        OnVariableChange();
    }

    public static void variableChangeOff(){
        OnVariableChangeOff();
    }

    public static void turnChangeInitiative(){
        OnTurnChange();
    }
}
