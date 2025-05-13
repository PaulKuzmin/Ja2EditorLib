namespace Ja2EditorLib;

class IntField : Field
{
    private int offset;

    public IntField(int offset)
    {
        this.offset = offset;
    }

    public string get(byte[] data)
    {
        return getInt(data).ToString();
    }

    public int getInt(byte[] data)
    {
        return (data[offset] & 0xFF)
               | ((data[offset + 1] & 0xFF) << 8)
               | ((data[offset + 2] & 0xFF) << 16)
               | ((data[offset + 3] & 0xFF) << 24);
    }

    public void set(byte[] data, string str)
    {
        int value = int.Parse(str);
        setInt(data, value);
    }

    public void setInt(byte[] data, int value)
    {
        data[offset] = (byte)(value & 0xFF);
        data[offset + 1] = (byte)((value >> 8) & 0xFF);
        data[offset + 2] = (byte)((value >> 16) & 0xFF);
        data[offset + 3] = (byte)((value >> 24) & 0xFF);
    }
}