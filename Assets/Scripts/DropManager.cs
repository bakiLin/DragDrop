using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DropManager : MonoBehaviour
{
    [SerializeField]
    private Slider _slider;

    [SerializeField]
    private TextMeshProUGUI _dropCountText;

    public void InitializeSlider(int itemCount)
    {
        _slider.maxValue = itemCount;
        _slider.value = 0;
        _dropCountText.text = "0";

        var dropWindow = _slider.transform.parent;
        dropWindow.gameObject.SetActive(true);
    }

    public void ChangeValue()
    {
        _dropCountText.text = _slider.value.ToString();
    }

    public int GetSliderValue() 
    {
        var dropWindow = _slider.transform.parent;
        dropWindow.gameObject.SetActive(false);

        return (int)_slider.value;
    }
}
