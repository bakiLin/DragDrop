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
        string[] options = new string[_itemTypes.Length];
        for (int i = 0; i < options.Length; i++) options[i] = _itemTypes[i].Name;
        _presenter.Init(options);

        _presenter.OnValueChanged += FillItemDropDown;
        _presenter.OnButtonClicked += CreateDropdownItem;
    }

    private void FillItemDropDown(int index)
    {
        string[] options = new string[_itemTypes[index].Items.Length];
        for (int i = 0; i < options.Length; i++) options[i] = _itemTypes[index].Items[i].Name;
        _presenter.FillItemDropdown(options);
    }

    private void CreateDropdownItem(int typeIndex, int itemIndex)
    {
        if (typeIndex != -1 && itemIndex != -1)
            _inventoryData.AddItem(_itemTypes[typeIndex].Items[itemIndex]);
    }
}
