using System.Text;

namespace Ja2EditorLib;

class StringField : Field
{
    private int offset;
    private int length;

    public StringField(int offset, int length)
    {
        this.offset = offset;
        this.length = length;
    }

    public string get(byte[] data)
    {
        var sb = new StringBuilder();
        for (int i = 0; i < this.length / 2; ++i)
        {
            byte b0 = data[this.offset + i * 2];
            byte b2 = data[this.offset + i * 2 + 1];
            char c = (char)((b0 & 0xFF) | (b2 & 0xFF) << 8);
            if (c == '\0')
            {
                break;
            }

            sb.Append(c);
        }

        return sb.ToString();
    }

    public int getInt(byte[] data)
    {
        return int.Parse(this.get(data));
    }

    public void set(byte[] data, string value)
    {
        for (int i = 0; i < this.length / 2; ++i)
        {
            char c = '\0';
            if (i < value.Length)
            {
                c = value[i];
            }

            data[this.offset + i * 2] = (byte)(c & 0xFF);
            data[this.offset + i * 2 + 1] = (byte)((c >> 8) & 0xFF);
        }
    }

    public void setInt(byte[] data, int value)
    {
        this.set(data, value.ToString());
    }
}