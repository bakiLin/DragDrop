using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDropView : MonoBehaviour
{
    public int SliderValue => (int)_slider.value;

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
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}