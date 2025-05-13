using System.Text;

namespace Ja2StracSaveEditorLib.Utilities;

public class BinaryReaderOffset : BinaryReader
{
    public Dictionary<string, BinaryReaderOffsetItem> Offsets { get; } =
        new Dictionary<string, BinaryReaderOffsetItem>();

    public BinaryReaderOffset(Stream input) : base(input)
    {
    }

    public BinaryReaderOffset(Stream input, Encoding encoding) : base(input, encoding)
    {
    }

    public BinaryReaderOffset(Stream input, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen)
    {
    }

    private T ReadData<T>(string propertyName, Func<T> func)
    {
        var start = BaseStream.Position;
        var value = func();
        if (!string.IsNullOrWhiteSpace(propertyName) && !Offsets.ContainsKey(propertyName))
        {
            Offsets.Add(propertyName, new BinaryReaderOffsetItem
            {
                Type = value.GetType(),
                Start = start,
                End = BaseStream.Position
            });
        }

        return value;
    }

    public byte ReadByte(string propertyName) => ReadData(propertyName, base.ReadByte);
    public sbyte ReadSByte(string propertyName) => ReadData(propertyName, base.ReadSByte);

    public short ReadInt16(string propertyName) => ReadData(propertyName, base.ReadInt16);
    public ushort ReadUInt16(string propertyName) => ReadData(propertyName, base.ReadUInt16);

    public bool ReadBoolean(string propertyName) => ReadData(propertyName, base.ReadBoolean);

    public uint ReadUInt32(string propertyName) => ReadData(propertyName, base.ReadUInt32);
}

public class BinaryReaderOffsetItem
{
    public Type Type { get; set; }
    public long Start { get; set; }
    public long End { get; set; }
}