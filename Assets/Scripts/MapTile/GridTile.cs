using UnityEngine;

public class GridTile : MonoBehaviour
{
    [SerializeField] SpriteRenderer meshRenderer;
    [SerializeField] Color highlight = Color.blue;
    private Color normalColor;
    public GameObject isAnyoneOn;

    void Awake() {
        meshRenderer = GetComponent<SpriteRenderer>();
        normalColor = meshRenderer.material.color;
        isAnyoneOn = null;
    }

    void OnValidate() => meshRenderer = GetComponent<SpriteRenderer>();

    public void Highlight(Color selectTileColor)
    {
        meshRenderer.material.color = selectTileColor;
    }

    public void removeHighlight()
    {
        meshRenderer.material.color = normalColor;
    }
}
