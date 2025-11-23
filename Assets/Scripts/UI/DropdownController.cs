using UnityEngine;
using UnityEngine.UI;

public class DropdownController : MonoBehaviour
{
    [SerializeField] 
    private AdvancedDropdown _itemTypeDropdown;

    [SerializeField]
    private AdvancedDropdown _itemDropdown;

    [SerializeField]
    private Button _createButton;

    [SerializeField]
    private InventorySO _inventoryData;

    [SerializeField]
    private ItemTypeSO[] _itemTypes;

    private void Start()
    {
        _itemTypeDropdown.DeleteAllOptions();
        for (int i = 0; i < _itemTypes.Length; i++)
            _itemTypeDropdown.AddOptions(_itemTypes[i].Name);

        _itemTypeDropdown.onChangedValue += FillItemDropDown;  
        _createButton.onClick.RemoveAllListeners();
        _createButton.onClick.AddListener(CreateDropdownItem);
    }

    private void FillItemDropDown(int index)
    {
        _itemDropdown.DeleteAllOptions();
        for (int i = 0; i < _itemTypes[index].Items.Length; i++)
            _itemDropdown.AddOptions(_itemTypes[index].Items[i].Name);
        _itemDropdown.SelectOption(0);
    }

    private void CreateDropdownItem()
    {
        if (_itemTypeDropdown.value != -1 && _itemDropdown.value != -1)
        {
            ItemSO item = _itemTypes[_itemTypeDropdown.value].Items[_itemDropdown.value];
            _inventoryData.AddItem(item);
        }
    }
}
