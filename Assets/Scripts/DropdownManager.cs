using UnityEngine;

public class DropdownManager : MonoBehaviour
{
    [SerializeField]
    private AdvancedDropdown _typeDropdown, _itemDropdown;

    [SerializeField]
    private ItemTypeGroup[] _itemType;

    private EnumToString _formatter;

    private void Awake()
    {
        _formatter = new EnumToString();
    }

    private void Start()
    {
        _typeDropdown.DeleteAllOptions();
        for (int i = 0; i < _itemType.Length; i++)
            _typeDropdown.AddOptions(_formatter.Format(_itemType[i].ItemType.ToString()));
        _typeDropdown.onChangedValue += RefreshItemDropdown;
    }

    private void RefreshItemDropdown(int value)
    {
        _itemDropdown.DeleteAllOptions();
        for (int i = 0; i < _itemType[value].Items.Length; i++)
            _itemDropdown.AddOptions(_itemType[value].Items[i].name);
        _itemDropdown.SelectOption(0);
    }

    public ItemSO GetSelectedItem()
    {
        if (_itemDropdown.optionsList.Count > 0)
            return _itemType[_typeDropdown.value].Items[_itemDropdown.value];
        return null;
    }
}

[System.Serializable]
public class ItemTypeGroup
{
    public ItemType ItemType;
    public ItemSO[] Items;
}
