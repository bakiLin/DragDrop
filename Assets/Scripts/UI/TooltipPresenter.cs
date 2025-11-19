using TMPro;
using UnityEngine;

public class TooltipPresenter : MonoBehaviour
{
    [SerializeField] 
    private TextMeshProUGUI _tooltipText;

    private void Awake()
    {
        ResetTooltipData();
    }

    public void ResetTooltipData()
    {
        _tooltipText.text = string.Empty;
        gameObject.SetActive(false);
    }

    public void SetTooltipData(string description)
    {
        if (description != string.Empty)
        {
            _tooltipText.text = description;
            gameObject.SetActive(true);
        }
    }
}
