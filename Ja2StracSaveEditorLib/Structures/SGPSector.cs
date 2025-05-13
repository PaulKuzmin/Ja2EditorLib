// ReSharper disable InconsistentNaming

using System.Runtime.InteropServices;

namespace Ja2StracSaveEditorLib.Structures;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SGPSector : IEquatable<SGPSector>, IComparable<SGPSector>
{
    public short x;
    public short y;
    public sbyte z;

    // Конструктор по умолчанию (в C# поля и так инициализируются нулями)
    public SGPSector(short a, short b, sbyte c = 0)
    {
        x = a;
        y = b;
        z = c;
    }

    // Конструктор из сектора (если s = sector ID как UINT32)
    public SGPSector(uint s)
    {
        x = (short)(s % 16 + 1); // Пример декодирования
        y = (short)(s / 16 + 1);
        z = 0;
    }

    public static SGPSector FromSectorID(uint s, sbyte h = 0)
    {
        short x = (short)(s % 16 + 1);
        short y = (short)(s / 16 + 1);
        return new SGPSector(x, y, h);
    }

    public static SGPSector FromStrategicIndex(ushort idx)
    {
        short x = (short)(idx % 16 + 1);
        short y = (short)(idx / 16 + 1);
        return new SGPSector(x, y, 0);
    }

    public byte AsByte()
    {
        return (byte)((y - 1) * 16 + (x - 1) & 0xFF);
    }

    public ushort AsStrategicIndex()
    {
        return (ushort)((y - 1) * 16 + (x - 1));
    }

    public string AsShortString()
    {
        // Пример: "A1" или "B13"
        char letter = (char)('A' + y - 1);
        return $"{letter}{x}";
    }

    public string AsLongString(bool file = false)
    {
        return $"Sector {AsShortString()} (z={z})";
    }

    public bool IsValid()
    {
        return x > 0 && y > 0;
    }

    public static SGPSector operator +(SGPSector a, SGPSector b) =>
        new SGPSector((short)(a.x + b.x), (short)(a.y + b.y), (sbyte)(a.z + b.z));

    public static SGPSector operator -(SGPSector a, SGPSector b) =>
        new SGPSector((short)(a.x - b.x), (short)(a.y - b.y), (sbyte)(a.z - b.z));

    public static bool operator ==(SGPSector a, SGPSector b) =>
        a.x == b.x && a.y == b.y && a.z == b.z;

    public static bool operator !=(SGPSector a, SGPSector b) =>
        !(a == b);

    public static bool operator <(SGPSector a, SGPSector b) =>
        a.AsStrategicIndex() < b.AsStrategicIndex();

    public static bool operator >(SGPSector a, SGPSector b) =>
        a.AsStrategicIndex() > b.AsStrategicIndex();

    public SGPSector Add(SGPSector other)
    {
        x += other.x;
        y += other.y;
        z += other.z;
        return this;
    }

    public SGPSector Subtract(SGPSector other)
    {
        x -= other.x;
        y -= other.y;
        z -= other.z;
        return this;
    }

    public override bool Equals(object obj) =>
        obj is SGPSector other && this == other;

    public bool Equals(SGPSector other) =>
        this == other;

    public int CompareTo(SGPSector other) =>
        AsStrategicIndex().CompareTo(other.AsStrategicIndex());

    public override int GetHashCode() =>
        HashCode.Combine(x, y, z);

    public static SGPSector FromShortString(string s, sbyte h = 0)
    {
        if (string.IsNullOrWhiteSpace(s) || s.Length < 2)
            throw new ArgumentException("Invalid sector string.", nameof(s));

        char rowChar = char.ToUpperInvariant(s[0]);
        if (rowChar < 'A' || rowChar > 'Z')
            throw new ArgumentException("Invalid row character in sector string.", nameof(s));

        if (!int.TryParse(s.Substring(1), out int column) || column < 1 || column > 16)
            throw new ArgumentException("Invalid column number in sector string.", nameof(s));

        short y = (short)(rowChar - 'A' + 1); // A -> 1, B -> 2, ...
        short x = (short)column;

        return new SGPSector(x, y, h);
    }

}
