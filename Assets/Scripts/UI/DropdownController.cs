using System.Linq;
using UnityEngine;

public class DropdownController : MonoBehaviour
{
    [SerializeField]
    private InventorySO _inventoryData;

    [SerializeField]
    private ItemTypeSO[] _itemTypes;

    [SerializeField]
    private DropdownPresenter _presenter;

    private void Start()
    {
        _presenter.Init(
            _itemTypes.Select(x => x.Name).ToArray());
        _presenter.OnValueChanged += FillItemDropDown;
        _presenter.OnButtonClicked += CreateDropdownItem;
    }

    private void FillItemDropDown(int index)
    {
        _presenter.FillItemDropdown(
            _itemTypes[index].Items.Select(x => x.Name).ToArray());
    }

    private void CreateDropdownItem(int typeIndex, int itemIndex)
    {
        if (typeIndex != -1 && itemIndex != -1)
            _inventoryData.AddItem(_itemTypes[typeIndex].Items[itemIndex]);
    }
}
