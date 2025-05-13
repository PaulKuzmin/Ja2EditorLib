using System.Collections;

namespace Ja2EditorLib;

public class ChoiceField : Field
{
    private Field baseField;
    private Hashtable table;

    public ChoiceField(Field baseField, Hashtable table)
    {
        this.baseField = baseField;
        this.table = table;
    }

    public string get(byte[] data)
    {
        int value = baseField.getInt(data);
        //return table[new Integer(value)] as string; // кастуем к string, если найдено
        return table[value] as string;
    }

    public int getInt(byte[] data)
    {
        return baseField.getInt(data);
    }

    public void set(byte[] data, string str)
    {
        if (table[str] is int value)
        {
            setInt(data, value);
        }
    }

    public void setInt(byte[] data, int value)
    {
        baseField.setInt(data, value);
    }
}