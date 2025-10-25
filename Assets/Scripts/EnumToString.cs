using System.Text;

public class EnumToString
{
    public string Format(string value)
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
