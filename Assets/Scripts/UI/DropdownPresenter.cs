using System;
using UnityEngine;
using UnityEngine.UI;

public class DropdownPresenter : MonoBehaviour
{
    public event Action<int> OnValueChanged;

    public event Action<int, int> OnButtonClicked;

    [SerializeField]
    private AdvancedDropdown _itemTypeDropdown;

    [SerializeField]
    private AdvancedDropdown _itemDropdown;

    [SerializeField]
    private Button _createButton;

    public void Init(string[] options)
    {
        _itemTypeDropdown.DeleteAllOptions();
        for (int i = 0; i < options.Length; i++) _itemTypeDropdown.AddOptions(options[i]);
        _itemTypeDropdown.onChangedValue += (int index) => OnValueChanged?.Invoke(index);

        _createButton.onClick.RemoveAllListeners();
        _createButton.onClick.AddListener(delegate { 
            OnButtonClicked.Invoke(_itemTypeDropdown.value, _itemDropdown.value); 
        });
    }

    public void FillItemDropdown(string[] options)
    {
        _itemDropdown.DeleteAllOptions();
        for (int i = 0; i < options.Length; i++) _itemDropdown.AddOptions(options[i]);
        _itemDropdown.SelectOption(0);
    }
}
