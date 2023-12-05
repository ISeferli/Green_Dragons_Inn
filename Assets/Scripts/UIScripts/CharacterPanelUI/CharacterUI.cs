using GDI.WorldInteraction;
using UnityEngine;

public class CharacterUI : MonoBehaviour {
    public RectTransform characterUIPanel;
    public bool characterIsActive { get; set; }
    
    void Start() {
        characterIsActive = false;
        characterUIPanel.gameObject.SetActive(characterIsActive);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.U) && WorldInteraction.instance.whoIsSelected()!=null) {
            characterIsActive = !characterIsActive;
            characterUIPanel.gameObject.SetActive(characterIsActive);
        }
    }
}
