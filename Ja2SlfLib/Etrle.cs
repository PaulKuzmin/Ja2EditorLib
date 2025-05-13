// ReSharper disable InconsistentNaming
namespace Ja2SlfLib;

using System;
using System.IO;

public class EtrleException : Exception
{
    public EtrleException(string message) : base(message) { }
}

public static class Etrle
{
    private const byte ALPHA_VALUE = 0;
    private const byte IS_COMPRESSED_BYTE_MASK = 0x80;
    private const byte NUMBER_OF_BYTES_MASK = 0x7F;

    public static byte[] Decompress(byte[] data)
    {
        using var output = new MemoryStream();
        int bytesTilNextControlByte = 0;

        foreach (var currentByte in data)
        {
            if (bytesTilNextControlByte == 0)
            {
                bool isCompressedAlphaByte = (currentByte & IS_COMPRESSED_BYTE_MASK) != 0;
                int lengthOfSubsequence = currentByte & NUMBER_OF_BYTES_MASK;

                if (isCompressedAlphaByte)
                {
                    for (int i = 0; i < lengthOfSubsequence; i++)
                    {
                        output.WriteByte(ALPHA_VALUE);
                    }
                }
                else
                {
                    bytesTilNextControlByte = lengthOfSubsequence;
                }
            }
            else
            {
                output.WriteByte(currentByte);
                bytesTilNextControlByte--;
            }
        }

        if (bytesTilNextControlByte != 0)
        {
            throw new EtrleException("Not enough data to decompress");
        }

        return output.ToArray();
    }

    public static byte[] Compress(byte[] data)
    {
        using var compressedBuffer = new MemoryStream();
        int current = 0;

        while (current < data.Length)
        {
            int runLength = 0;

            if (data[current] == 0)
            {
                while (current + runLength < data.Length &&
                       data[current + runLength] == 0 &&
                       runLength < NUMBER_OF_BYTES_MASK)
                {
                    runLength++;
                }

                compressedBuffer.WriteByte((byte)(runLength | IS_COMPRESSED_BYTE_MASK));
            }
            else
            {
                while (current + runLength < data.Length &&
                       data[current + runLength] != 0 &&
                       runLength < NUMBER_OF_BYTES_MASK)
                {
                    runLength++;
                }

                compressedBuffer.WriteByte((byte)runLength);
                compressedBuffer.Write(data, current, runLength);
            }

            current += runLength;
        }

        return compressedBuffer.ToArray();
    }
}
