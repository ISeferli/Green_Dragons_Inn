using UnityEngine;
using UnityEngine.UI;

public class AttributeButton : MonoBehaviour {
    private Button button;

    void Start(){
        this.button = GetComponent<Button>();
        button.onClick.AddListener(AllocateButtonClicked);
    }

    void AllocateButtonClicked(){
        this.GetComponentInParent<CharacterAttributesUI>().OnAttributeButtonPressed(button.name);
    }
}
