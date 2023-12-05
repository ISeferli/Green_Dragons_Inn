using UnityEngine;
using UnityEngine.UI;

public class PointAllocateButton : MonoBehaviour {
    private bool red;

    void Start(){
        Button button = GetComponent<Button>();
        button.onClick.AddListener(ButtonClicked);
    }

    void ButtonClicked(){
        Button button = GetComponent<Button>();
        if(this.GetComponentInParent<CharacterAttributesUI>().nonChangeableAttribute()!=button.name){
            if(button.image.color != Color.red){
                button.image.color = Color.red;
                red = true;
            } else {
                button.image.color = Color.white;
                red = false;
            }
            this.GetComponentInParent<CharacterAttributesUI>().OnButtonColorChange(button.name, red);
        }
    }

}
