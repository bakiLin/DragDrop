using TMPro;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _tooltipText;

    [SerializeField]
    private Vector2 _offset;

    [SerializeField]
    private Canvas _canvas;

    private Transform _tooltip;

    public static TooltipManager Instance;

    private void Awake()
    {
        Instance = this;
        _tooltip = _tooltipText.transform.parent;
    }

    public void EnableTooltip(string text, Vector2 position)
    {
        _tooltipText.text = text;
        _tooltip.position = position + _offset * _canvas.scaleFactor;
        _tooltip.gameObject.SetActive(true);
    }

    public void DisableTooltip()
    {
        _tooltip.gameObject.SetActive(false);
    }
}
