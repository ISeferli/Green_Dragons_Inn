using UnityEngine;
using UnityEngine.UI;

public class LevelUpAttributeButton : MonoBehaviour {
    private Button button;

    void Start(){
        this.button = GetComponent<Button>();
        button.onClick.AddListener(AllocateButtonClicked);
    }

    void AllocateButtonClicked(){
        this.GetComponentInParent<LevelUpUI>().OnAttributeButtonPressed(button.name);
    }
}
