using System.Linq;
using UnityEngine;

public class DropdownController : MonoBehaviour
{
    [SerializeField]
    private InventorySO _inventoryData;

    [SerializeField]
    private DropdownView _view;

    private ItemTypeSO[] _itemTypes => _inventoryData.ItemTypes;

    private void Start()
    {
        _view.Init(_itemTypes.Select(x => x.Name).ToArray());
        _view.OnValueChanged += FillItemDropDown;
        _view.OnButtonClicked += CreateDropdownItem;
    }

    private void FillItemDropDown(int index)
    {
        _view.FillItemDropdown(
            _itemTypes[index].Items.Select(x => x.Name).ToArray());
    }

    private void CreateDropdownItem(int typeIndex, int itemIndex)
    {
        if (typeIndex != -1 && itemIndex != -1)
            _inventoryData.AddItem(_itemTypes[typeIndex].Items[itemIndex]);
    }
}
