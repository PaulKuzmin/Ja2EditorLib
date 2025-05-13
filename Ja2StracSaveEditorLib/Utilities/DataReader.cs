using System.Text;

namespace Ja2StracSaveEditorLib.Utilities;

public static class DataReader
{
    public static string ReadUtf16Le(this BinaryReader fileReader, int numChars)
    {
        var byteCount = numChars * 2;
        var bytes = fileReader.ReadBytes(byteCount);

        var utf16Le = Encoding.Unicode; // UTF-16 LE
        var result = utf16Le.GetString(bytes, 0, byteCount);
        
        return result;
    }

    public static void Skip(this BinaryReader reader, uint n)
    {
        reader.BaseStream.Seek(n, SeekOrigin.Current);
    }

    public static object Ptr(this BinaryReader reader, bool needValue = false)
    {
        if (needValue)
            throw new NotImplementedException();

        reader.BaseStream.Seek(4, SeekOrigin.Current);
        return null;
    }

    public static string ReadUtf8(this BinaryReader reader, int numChars)
    {
        // Чтение байтов по количеству символов (если 1 байт на символ предполагается)
        var buffer = reader.ReadBytes(numChars);

        // Преобразуем в строку (предполагаем, что validation происходит в Decode)
        return Encoding.UTF8.GetString(buffer).TrimEnd('\0');
    }

    public static sbyte[] ReadSbytes(this BinaryReader reader, int n)
    {
        var array = new sbyte[n];

        for (var i = 0; i < n; i++)
        {
            array[i] = reader.ReadSByte();
        }

        return array;
    }

    public static ushort[] ReadUshorts(this BinaryReader reader, int n)
    {
        var array = new ushort[n];

        for (var i = 0; i < n; i++)
        {
            array[i] = reader.ReadUInt16();
        }

        return array;
    }

    public static short[] ReadShorts(this BinaryReader reader, int n)
    {
        var array = new short[n];

        for (var i = 0; i < n; i++)
        {
            array[i] = reader.ReadInt16();
        }

        return array;
    }

    public static object GetValue<T>(this T target, string propertyName)
    {
        if (target == null) return null;

        var type = target.GetType();
        var property = type.GetProperties().FirstOrDefault(f => f.Name.Equals(propertyName));

        return property != null ? property.GetValue(target) : null;
    }
}