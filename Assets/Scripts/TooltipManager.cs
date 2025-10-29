using TMPro;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _tooltipText;

    [SerializeField]
    private Canvas _canvas;

    [SerializeField]
    private Vector2 _offset;

    private Transform _tooltip;

    private void Awake()
    {
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
