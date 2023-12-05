using UnityEngine;

public class Item : MonoBehaviour {
    public string objectSlug { get; set; }
    public string description { get; set; }
    public string actionName { get; set; }
    public string itemName { get; set; }
    public int itemModifier { get; set; }
    public int baseValue { get; set; }
    public string statName { get; set; }
    public string itemType { get; set; }

    public Item (string _objectSlug) {
        this.objectSlug = _objectSlug;
    }

    public Item (string _objectSlug, string _description, string _actionName, string _itemName, int _modifier) {
        this.objectSlug = _objectSlug;
        this.description = _description;
        this.actionName = _actionName;
        this.itemName =  _itemName;
        this.itemModifier = _modifier;
    }
}
