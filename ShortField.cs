namespace Ja2EditorLib;
class ShortField : Field
    {
        private int offset;

        public ShortField(int offset)
        {
            this.offset = offset;
        }

        public string get(byte[] data)
        {
            return getInt(data).ToString();
        }

        public int getInt(byte[] data)
        {
            return (data[this.offset] & 0xFF) | ((data[this.offset + 1] & 0xFF) << 8);
        }

        public void set(byte[] data, string str)
        {
            int value = int.Parse(str);
            setInt(data, value);
        }

        public void setInt(byte[] data, int value)
        {
            data[this.offset] = (byte)(value & 0xFF);
            data[this.offset + 1] = (byte)((value >> 8) & 0xFF);
        }
    }