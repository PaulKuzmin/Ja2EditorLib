using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
// ReSharper disable InconsistentNaming

namespace Ja2EditorLib.notWorking;

/// <summary>
/// Class for serializing data.
/// Assumes the endianness is correct.
/// </summary>
public class DataWriter
{
    private byte[] _buffer;
    private int _offset;
    private int _startOffset;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="buf">Pointer to the buffer for writing data.</param>
    public DataWriter(byte[] buf) : this(buf, 0) { }

    /// <summary>
    /// Constructor with specified starting offset in the buffer.
    /// </summary>
    /// <param name="buf">Buffer for writing data.</param>
    /// <param name="startIndex">Initial position in the buffer to start writing.</param>
    public DataWriter(byte[] buf, int startIndex)
    {
        _buffer = buf;
        _offset = startIndex;
        _startOffset = startIndex;
    }

    /// <summary>
    /// Write UTF-8 encoded string.
    /// </summary>
    /// <param name="str">String to write.</param>
    /// <param name="numChars">Number of `char` characters to write.</param>
    public void WriteUTF8(string str, int numChars)
    {
        // Encode string to UTF-8 bytes
        byte[] utf8Bytes = Encoding.UTF8.GetBytes(str);
        // Determine how many bytes to write
        int bytesToWrite = utf8Bytes.Length;
        if (bytesToWrite > numChars) bytesToWrite = numChars;
        // Copy the bytes to the buffer
        Array.Copy(utf8Bytes, 0, _buffer, _offset, bytesToWrite);
        // Pad remaining space with 0 bytes if string is shorter than numChars
        for (int i = bytesToWrite; i < numChars; i++)
        {
            _buffer[_offset + i] = 0;
        }
        // Advance the write pointer
        _offset += numChars;
    }

    /// <summary>
    /// Write UTF-16 encoded string.
    /// </summary>
    /// <param name="str">String to write.</param>
    /// <param name="numChars">Number of `char16_t` characters to write.</param>
    public void WriteUTF16(string str, int numChars)
    {
        // Encode string to UTF-16 (little-endian) bytes
        byte[] utf16Bytes = Encoding.Unicode.GetBytes(str);
        int bytesToWrite = utf16Bytes.Length;
        int requiredBytes = numChars * 2;
        if (bytesToWrite > requiredBytes) bytesToWrite = requiredBytes;
        Array.Copy(utf16Bytes, 0, _buffer, _offset, bytesToWrite);
        // Pad with zero bytes if needed
        for (int i = bytesToWrite; i < requiredBytes; i++)
        {
            _buffer[_offset + i] = 0;
        }
        _offset += requiredBytes;
    }

    /// <summary>Write uint8_t (8-bit unsigned int).</summary>
    public void WriteU8(byte value)
    {
        _buffer[_offset] = value;
        _offset += 1;
    }

    /// <summary>Write uint16_t (16-bit unsigned int).</summary>
    public void WriteU16(ushort value)
    {
        // Write in little-endian order
        _buffer[_offset] = (byte)(value & 0xFF);
        _buffer[_offset + 1] = (byte)(value >> 8 & 0xFF);
        _offset += 2;
    }

    /// <summary>Write uint32_t (32-bit unsigned int).</summary>
    public void WriteU32(uint value)
    {
        _buffer[_offset] = (byte)(value & 0xFF);
        _buffer[_offset + 1] = (byte)(value >> 8 & 0xFF);
        _buffer[_offset + 2] = (byte)(value >> 16 & 0xFF);
        _buffer[_offset + 3] = (byte)(value >> 24 & 0xFF);
        _offset += 4;
    }

    /// <summary>
    /// Write a value of type T.
    /// </summary>
    /// <typeparam name="T">Trivially copyable (blittable) type.</typeparam>
    /// <param name="value">Value to write.</param>
    public void Write<T>(T value) where T : unmanaged
    {
        // Copy the raw bytes of the value into the buffer
        int size = Unsafe.SizeOf<T>();
        Span<byte> dest = new Span<byte>(_buffer, _offset, size);
        //MemoryMarshal.Write(dest, ref value);
        MemoryMarshal.Write(dest, value);
        _offset += size;
    }

    /// <summary>
    /// Write an array of values.
    /// </summary>
    /// <typeparam name="T">Trivially copyable (blittable) element type.</typeparam>
    /// <param name="arr">Array of values to write.</param>
    /// <param name="length">Number of elements from the array to write.</param>
    public void WriteArray<T>(T[] arr, int length) where T : unmanaged
    {
        if (typeof(T) == typeof(bool))
        {
            // Booleans are stored as 1 byte (0 or 1)
            bool[] boolArray = arr as bool[];
            for (int i = 0; i < length; i++)
            {
                // ReSharper disable once PossibleNullReferenceException
                _buffer[_offset + i] = boolArray[i] ? (byte)1 : (byte)0;
            }
            _offset += length;
        }
        else
        {
            Span<T> sourceSpan = new Span<T>(arr, 0, length);
            Span<byte> byteSpan = MemoryMarshal.AsBytes(sourceSpan);
            byteSpan.CopyTo(new Span<byte>(_buffer, _offset, byteSpan.Length));
            _offset += byteSpan.Length;
        }
    }

    /// <summary>
    /// Write zeroed bytes (skip forward).
    /// </summary>
    /// <param name="numBytes">Number of bytes to skip (write as zero).</param>
    public void Skip(int numBytes)
    {
        // Set the specified number of bytes to zero
        Array.Clear(_buffer, _offset, numBytes);
        _offset += numBytes;
    }

    /// <summary>
    /// Get number of the consumed bytes during writing.
    /// </summary>
    /// <returns>Number of bytes written so far.</returns>
    public int GetConsumed()
    {
        return _offset - _startOffset;
    }

    // (Protected move pointer in C++; here kept private)
    // ReSharper disable once UnusedMember.Local
    private void Move(int numBytes)
    {
        _offset += numBytes;
    }
}

/// <summary>
/// Class for reading serialized data.
/// Assumes the endianness is correct.
/// </summary>
public class DataReader
{
    private byte[] buffer;
    private int offset;
    private int startOffset;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="buf">Pointer to the buffer for reading data.</param>
    public DataReader(byte[] buf) : this(buf, 0) { }

    /// <summary>
    /// Constructor with specified starting offset in the buffer.
    /// </summary>
    public DataReader(byte[] buf, int startIndex)
    {
        buffer = buf;
        offset = startIndex;
        startOffset = startIndex;
    }

    /// <summary>
    /// Read UTF-8 encoded string.
    /// </summary>
    /// <param name="numChars">Number of `char` characters to read.</param>
    /// <param name="validation">What happens with invalid character sequences.</param>
    /// <returns>Decoded string.</returns>
    public string ReadUTF8(int numChars, UtfValidation validation = UtfValidation.Default)
    {
        // Decode specified number of bytes as UTF-8
        string result = Encoding.UTF8.GetString(buffer, offset, numChars);
        offset += numChars;
        // (Invalid sequences are replaced with the Unicode replacement character by default)
        return result;
    }

    /// <summary>
    /// Read UTF-16 encoded string.
    /// </summary>
    /// <param name="numChars">Number of `char16_t` characters to read.</param>
    /// <param name="isCorrectlyEncoded">Used for fixing incorrectly encoded text (if false).</param>
    /// <param name="validation">What happens with invalid character sequences.</param>
    /// <returns>Decoded string.</returns>
    public string ReadUTF16(int numChars, bool isCorrectlyEncoded = true, UtfValidation validation = UtfValidation.Default)
    {
        int byteCount = numChars * 2;
        // Decode as UTF-16 (little-endian)
        string result = Encoding.Unicode.GetString(buffer, offset, byteCount);
        offset += byteCount;
        // (If isCorrectlyEncoded is false, one would handle any necessary corrections here)
        return result;
    }

    /// <summary>
    /// Read UTF-32 encoded string.
    /// </summary>
    /// <param name="numChars">Number of `char32_t` characters to read.</param>
    /// <param name="validation">What happens with invalid character sequences.</param>
    /// <returns>Decoded string.</returns>
    public string ReadUTF32(int numChars, UtfValidation validation = UtfValidation.Default)
    {
        int byteCount = numChars * 2;
        // Decode as UTF-16 little-endian
        Encoding utf16LE = Encoding.Unicode; // UTF-16 LE
        string result = utf16LE.GetString(buffer, offset, byteCount);
        offset += byteCount;
        return result;
    }

    /// <summary>
    /// Read a string that is either UTF-32 or UTF-16 encoded.
    /// </summary>
    /// <param name="numChars">Number of `char32_t` characters to read.</param>
    /// <param name="stracLinuxFormat">
    /// True if data was written using the short-lived format where strings were UTF-32 encoded on Unix,
    /// or false for the current Windows-like format with UTF-16 encoded strings.
    /// </param>
    public string ReadString(int numChars, bool stracLinuxFormat)
    {
        if (stracLinuxFormat)
        {
            // ReSharper disable once RedundantArgumentDefaultValue
            return ReadUTF32(numChars, UtfValidation.Default);
        }
        // ReSharper disable once RedundantArgumentDefaultValue
        // ReSharper disable once RedundantArgumentDefaultValue
        return ReadUTF16(numChars, true, UtfValidation.Default);
    }

    /// <summary>Read uint8_t (8-bit unsigned int).</summary>
    public byte ReadU8()
    {
        byte value = buffer[offset];
        offset += 1;
        return value;
    }

    /// <summary>Read uint16_t (16-bit unsigned int).</summary>
    public ushort ReadU16()
    {
        // Assemble 2 bytes in little-endian order
        ushort value = (ushort)(buffer[offset] | buffer[offset + 1] << 8);
        offset += 2;
        return value;
    }

    /// <summary>Read uint32_t (32-bit unsigned int).</summary>
    public uint ReadU32()
    {
        uint value = (uint)(
            buffer[offset] |
            buffer[offset + 1] << 8 |
            buffer[offset + 2] << 16 |
            buffer[offset + 3] << 24
        );
        offset += 4;
        return value;
    }

    /// <summary>
    /// Read raw bytes into a provided buffer.
    /// </summary>
    /// <param name="dest">Destination array for the bytes.</param>
    /// <param name="numBytes">Number of bytes to read.</param>
    public void ReadBytes(byte[] dest, int numBytes)
    {
        Array.Copy(buffer, offset, dest, 0, numBytes);
        offset += numBytes;
    }

    /// <summary>
    /// Read a value of type T.
    /// </summary>
    /// <typeparam name="T">Trivially copyable (blittable) type.</typeparam>
    /// <returns>The value read.</returns>
    public T Read<T>() where T : unmanaged
    {
        int size = Unsafe.SizeOf<T>();
        ReadOnlySpan<byte> src = new ReadOnlySpan<byte>(buffer, offset, size);
        T value = MemoryMarshal.Read<T>(src);
        offset += size;
        return value;
    }

    /// <summary>
    /// Read an array of values.
    /// </summary>
    /// <typeparam name="T">Trivially copyable (blittable) element type.</typeparam>
    /// <param name="dest">Destination array to fill.</param>
    /// <param name="length">Number of elements to read into the array.</param>
    public void ReadArray<T>(T[] dest, int length) where T : unmanaged
    {
        if (typeof(T) == typeof(bool))
        {
            bool[] boolArray = dest as bool[];
            for (int i = 0; i < length; i++)
            {
                // ReSharper disable once PossibleNullReferenceException
                boolArray[i] = buffer[offset + i] != 0;
            }
            offset += length;
        }
        else
        {
            Span<T> destSpan = new Span<T>(dest, 0, length);
            Span<byte> destBytes = MemoryMarshal.AsBytes(destSpan);
            new ReadOnlySpan<byte>(buffer, offset, destBytes.Length).CopyTo(destBytes);
            offset += destBytes.Length;
        }
    }

    /// <summary>
    /// Skip (read and discard) a number of bytes.
    /// </summary>
    /// <param name="numBytes">Number of bytes to skip.</param>
    public void Skip(int numBytes)
    {
        offset += numBytes;
    }

    /// <summary>
    /// Get number of the consumed bytes during reading.
    /// </summary>
    /// <returns>Number of bytes read so far.</returns>
    public int GetConsumed()
    {
        return offset - startOffset;
    }

    // (Protected move pointer in C++; private here)
    // ReSharper disable once UnusedMember.Local
    private void Move(int numBytes)
    {
        offset += numBytes;
    }
}

/// <summary>
/// UTF validation mode (placeholder for behavior on invalid sequences).
/// </summary>
public enum UtfValidation
{
    Default  // Use default validation behavior
}

/// <summary>
/// Static helper methods corresponding to serialization macros (INJ/EXTR).
/// </summary>
public static class LoadSaveMacros
{
    // INJ (inject/write) macros as static methods:
    public static void INJ_STR(DataWriter D, byte[] S, int Size) { D.WriteArray(S, Size); }
    public static void INJ_BOOLA(DataWriter D, bool[] S, int Size) { D.WriteArray(S, Size); }
    public static void INJ_I8A(DataWriter D, sbyte[] S, int Size) { D.WriteArray(S, Size); }
    public static void INJ_U8A(DataWriter D, byte[] S, int Size) { D.WriteArray(S, Size); }
    public static void INJ_I16A(DataWriter D, short[] S, int Size) { D.WriteArray(S, Size); }
    public static void INJ_U16A(DataWriter D, ushort[] S, int Size) { D.WriteArray(S, Size); }
    public static void INJ_I32A(DataWriter D, int[] S, int Size) { D.WriteArray(S, Size); }

    public static void INJ_BOOL(DataWriter D, bool S) { D.Write(S); }
    public static void INJ_BYTE(DataWriter D, byte S) { D.Write(S); }
    public static void INJ_I8(DataWriter D, sbyte S) { D.Write(S); }
    public static void INJ_U8(DataWriter D, byte S) { D.WriteU8(S); }
    public static void INJ_I16(DataWriter D, short S) { D.Write(S); }
    public static void INJ_U16(DataWriter D, ushort S) { D.WriteU16(S); }
    public static void INJ_I32(DataWriter D, int S) { D.Write(S); }
    public static void INJ_U32(DataWriter D, uint S) { D.WriteU32(S); }
    public static void INJ_FLOAT(DataWriter D, float S) { D.Write(S); }
    public static void INJ_DOUBLE(DataWriter D, double S) { D.Write(S); }
    public static void INJ_PTR(DataWriter D, object S) { INJ_SKIP(D, 4); }
    public static void INJ_SKIP(DataWriter D, int Size) { D.Skip(Size); }
    public static void INJ_SKIP_I16(DataWriter D) { D.Skip(2); }
    public static void INJ_SKIP_I32(DataWriter D) { D.Skip(4); }
    public static void INJ_SKIP_U8(DataWriter D) { D.Skip(1); }
    public static void INJ_SOLDIER(DataWriter D, Soldier S) { D.Write(Soldier2ID(S)); }
    public static void INJ_VEC3(DataWriter D, Vector3 S) { INJ_FLOAT(D, S.x); INJ_FLOAT(D, S.y); INJ_FLOAT(D, S.z); }
    public static void INJ_AUTO<T>(DataWriter D, T S) where T : unmanaged => D.Write(S);

    // EXTR (extract/read) macros as static methods:
    public static void EXTR_STR(DataReader S, byte[] D, int Size) { S.ReadArray(D, Size); }
    public static void EXTR_BOOLA(DataReader S, bool[] D, int Size) { S.ReadArray(D, Size); }
    public static void EXTR_I8A(DataReader S, sbyte[] D, int Size) { S.ReadArray(D, Size); }
    public static void EXTR_U8A(DataReader S, byte[] D, int Size) { S.ReadArray(D, Size); }
    public static void EXTR_I16A(DataReader S, short[] D, int Size) { S.ReadArray(D, Size); }
    public static void EXTR_U16A(DataReader S, ushort[] D, int Size) { S.ReadArray(D, Size); }
    public static void EXTR_I32A(DataReader S, int[] D, int Size) { S.ReadArray(D, Size); }

    public static void EXTR_BOOL(DataReader S, out bool D) { D = S.Read<bool>(); }
    public static void EXTR_BYTE(DataReader S, out byte D) { D = S.Read<byte>(); }
    public static void EXTR_I8(DataReader S, out sbyte D) { D = S.Read<sbyte>(); }
    public static void EXTR_U8(DataReader S, out byte D) { D = S.ReadU8(); }
    public static void EXTR_I16(DataReader S, out short D) { D = S.Read<short>(); }
    public static void EXTR_U16(DataReader S, out ushort D) { D = S.ReadU16(); }
    public static void EXTR_I32(DataReader S, out int D) { D = S.Read<int>(); }
    public static void EXTR_U32(DataReader S, out uint D) { D = S.Read<uint>(); }
    public static void EXTR_FLOAT(DataReader S, out float D) { D = S.Read<float>(); }
    public static void EXTR_DOUBLE(DataReader S, out double D) { D = S.Read<double>(); }
    public static void EXTR_PTR<T>(DataReader S, out T D) where T : class { D = null; S.Skip(4); }
    public static void EXTR_SKIP(DataReader S, int Size) { S.Skip(Size); }
    public static void EXTR_SKIP_I16(DataReader S) { S.Skip(2); }
    public static void EXTR_SKIP_I32(DataReader S) { S.Skip(4); }
    public static void EXTR_SKIP_U8(DataReader S) { S.Skip(1); }
    public static void EXTR_SOLDIER(DataReader S, out Soldier D) { SoldierID id = S.Read<SoldierID>(); D = ID2Soldier(id); }
    public static void EXTR_VEC3(DataReader S, out Vector3 D)
    {
        // Create a vector and fill its components
        Vector3 vec = new Vector3();
        vec.x = S.Read<float>(); vec.y = S.Read<float>(); vec.z = S.Read<float>();
        D = vec;
    }
    public static void EXTR_AUTO<T>(DataReader S, out T D) where T : unmanaged
    { D = S.Read<T>(); }

    /// <summary>
    /// Placeholder type for a soldier identifier (as stored in save data).
    /// </summary>
    public struct SoldierID
    {
        public uint Value;
        public SoldierID(uint v) { Value = v; }
    }

    /// <summary>
    /// Placeholder Soldier class (game-specific).
    /// </summary>
    public class Soldier { /* ... */ }

    // Placeholder conversion functions for Soldier objects to IDs and back (game-specific logic not implemented):
    private static SoldierID Soldier2ID(Soldier soldier)
    {
        // Convert Soldier object to its identifier for saving
        return soldier != null ? new SoldierID(0) : new SoldierID(0);
    }

    private static Soldier ID2Soldier(SoldierID id)
    {
        // Convert identifier back to Soldier object for loading
        return null;
    }
}

/// <summary>
/// Example 3D vector struct (for INJ_VEC3/EXTR_VEC3 macros).
/// </summary>
public struct Vector3
{
    public float x;
    public float y;
    public float z;
}