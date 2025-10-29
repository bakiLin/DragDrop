using System.Text;

public class EnumToString
{
    public string Format(string value)
    {
        var builder = new StringBuilder();
        builder.Append(value[0]);
        for (int i = 1; i < value.Length; i++)
        {
            if (char.IsUpper(value[i])) builder.Append(" ");
            builder.Append(value[i]);
        }
        return builder.ToString();
    }
}
