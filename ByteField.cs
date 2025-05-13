namespace Ja2EditorLib;

class ByteField : Field
{
    private int offset;

    public ByteField(int offset)
    {
        this.offset = offset;
    }

    public string get(byte[] data)
    {
        return getInt(data).ToString();
    }

    public int getInt(byte[] data)
    {
        return data[this.offset] & 0xFF;
    }

    public void set(byte[] data, string str)
    {
        int value = int.Parse(str);
        setInt(data, value);
    }

    public void setInt(byte[] data, int value)
    {
        data[this.offset] = (byte)value;
    }
}