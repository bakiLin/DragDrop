using TMPro;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    [SerializeField] 
    private TextMeshProUGUI _tooltipText;

    [SerializeField]
    private Vector2 _offset;

    private Canvas _canvas;

    public void ResetTooltipData()
    {
        _tooltipText.text = string.Empty;
        gameObject.SetActive(false);
    }

    public void SetTooltipData(string description, Vector2 position)
    {
        _canvas ??= transform.root.GetComponent<Canvas>();

        if (description != string.Empty)
        {
            _tooltipText.text = description;
            transform.position = position + _offset * _canvas.scaleFactor;
            gameObject.SetActive(true);
        }
    }
}
