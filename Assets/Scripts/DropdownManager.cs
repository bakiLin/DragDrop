using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class DropdownManager : MonoBehaviour
{
    [SerializeField]
    private AdvancedDropdown _dropdown;

    private void Start()
    {
        _dropdown.DeleteAllOptions();

        string[] names = System.Enum.GetNames(typeof(ItemType));
        foreach (string name in names)
            _dropdown.AddOptions(ToFormattedText(name));
    }

    private string ToFormattedText(string value)
    {
        var bld = new StringBuilder();
        bld.Append(value[0]);
        for (int i = 1; i < value.Length; i++)
        {
            if (char.IsUpper(value[i]))
                bld.Append(" ");
            bld.Append(value[i]);
        }
        return bld.ToString();
    }
}
