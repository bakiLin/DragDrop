using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    [SerializeField] private TextMeshProUGUI _dropCountText;

    private Transform _itemDropWindow;

    private void Awake()
    {
        _itemDropWindow = _slider.transform.parent;
        _slider.onValueChanged.RemoveAllListeners();
        _slider.onValueChanged.AddListener(delegate { ChangeValue(); });
    }

    public void InitializeSlider(int itemCount)
    {
        _slider.maxValue = itemCount;
        _slider.value = 0;
        _dropCountText.text = "0";
        _itemDropWindow.gameObject.SetActive(true);
    }

    public int ConfirmItemDrop()
    {
        _itemDropWindow.gameObject.SetActive(false);
        return (int)_slider.value;
    }

    private void ChangeValue()
    {
        _dropCountText.text = _slider.value.ToString();
    }
}
