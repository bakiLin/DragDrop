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

    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();

        _slider.onValueChanged.AddListener(
            delegate {
                _dropCountText.text = _slider.value.ToString();
            });
    }

    public void SetSlider(int stackSize)
    {
        _slider.value = 0;
        _slider.maxValue = stackSize;
        ToggleVisibility(true);
    }

    public void Disable()
    {
        ToggleVisibility(false);
    }

    private void ToggleVisibility(bool isActive)
    {
        _canvasGroup.alpha = isActive ? 1f : 0f;
        _canvasGroup.interactable = isActive;
        _canvasGroup.blocksRaycasts = isActive;
    }
}