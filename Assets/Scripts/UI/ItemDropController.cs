using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDropController : MonoBehaviour
{
    public int SliderValue => (int)_slider.value;

    [SerializeField]
    private RectTransform _dropWindow;

    [SerializeField]
    private Slider _slider;

    [SerializeField]
    private TextMeshProUGUI _dropCountText;

    private void Start()
    {
        _slider.onValueChanged.AddListener(
            delegate {
                _dropCountText.text = _slider.value.ToString();
            });
    }

    public void SetSlider(int stackSize)
    {
        _slider.value = 0;
        _slider.maxValue = stackSize;
        _dropWindow.gameObject.SetActive(true);
    }

    public void Disable()
    {
        _dropWindow.gameObject.SetActive(false);
    }
}