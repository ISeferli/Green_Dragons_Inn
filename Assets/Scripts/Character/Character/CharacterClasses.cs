using UnityEngine;
using UnityEngine.UI;

namespace GDI.CharacterClasses{
    [CreateAssetMenu(fileName = "New Class", menuName = "New Class")]
    public class CharacterClasses : ScriptableObject {
        public enum availableClasses {
            fighter, 
            archer,
            mage,
            cleric
        }
        public Sprite icon;
        public availableClasses avClass;
        public new string name;
        public GameObject classPrefab;

        public string mainAttribute;

        public int healthPoints;
        public int armorClass;
        public int attackRange;
        public int moveRange;
    }

}