using System.Collections;

namespace Ja2EditorLib;

public abstract class BasicStructure : Structure
{
    public Hashtable fields;
    public byte[] data;
    public Structure nextStructure;

    public BasicStructure(Hashtable classFields)
    {
        this.fields = classFields;
    }

    public void chain(Structure nextStructure)
    {
        this.nextStructure = nextStructure;
    }

    public string get(string name)
    {
        Field field = (Field)this.fields[name];
        if (field != null)
        {
            return field.get(this.data);
        }

        if (this.nextStructure != null)
        {
            return this.nextStructure.get(name);
        }

        return null;
    }

    public int getInt(string name)
    {
        Field field = (Field)this.fields[name];
        if (field != null)
        {
            return field.getInt(this.data);
        }

        if (this.nextStructure != null)
        {
            return this.nextStructure.getInt(name);
        }

        return 0;
    }

    public void set(string name, string value)
    {
        Field field = (Field)this.fields[name];
        if (field != null)
        {
            field.set(this.data, value);
        }

        if (this.nextStructure != null)
        {
            this.nextStructure.set(name, value);
        }
    }

    public void setInt(string name, int value)
    {
        Field field = (Field)this.fields[name];
        if (field != null)
        {
            field.setInt(this.data, value);
        }

        if (this.nextStructure != null)
        {
            this.nextStructure.setInt(name, value);
        }
    }
}