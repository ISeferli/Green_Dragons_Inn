using UnityEngine;

namespace GDI.ItemsScript{
    [CreateAssetMenu(fileName = "New Item", menuName = "New Item")]
    public class ItemScript : ScriptableObject {
        public enum availableItems {
            Potion, 
            Sword,
            Bow,
            LightPotion
        }

        public string objectSlug;

        public availableItems avItem;
        public string itName;
        public GameObject classPrefab;

        public string description;
        public string actionName;
        public int itemModifier;
        public int baseValue;
        public string statName;
    }
}
