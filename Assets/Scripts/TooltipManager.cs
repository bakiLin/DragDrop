using TMPro;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _tooltipText;

    [SerializeField]
    private Vector2 _offset;

    public static TooltipManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void EnableTooltip(string text, Vector2 position)
    {
        _tooltipText.text = text;

        var parent = _tooltipText.transform.parent;
        parent.position = position + _offset;
        parent.gameObject.SetActive(true);
    }

    public void DisableTooltip()
    {
        var parent = _tooltipText.transform.parent;
        parent.gameObject.SetActive(false);
    }
}
