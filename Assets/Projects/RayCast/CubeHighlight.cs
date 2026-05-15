using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class CubeHighlight : MonoBehaviour
{
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color highlightColor = Color.green;

    private Renderer _renderer;
    private Material _material;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _material = _renderer.material;

        SetDefaultColor();
    }

    public void Highlight()
    {
        _material.color = highlightColor;
    }

    public void SetDefaultColor()
    {
        _material.color = defaultColor;
    }
}