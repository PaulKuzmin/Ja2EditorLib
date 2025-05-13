using Kaitai;
using System.Drawing.Imaging;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Ja2SlfLib;

public class Ja2Sti
{
    private const int HeaderSize = 64;
    
    private readonly byte[] _bytes;
    private StiHeader _header;
    private bool _is8Otherwise16Bit;
    private string _path;
    private string _prefix;

    public Ja2Sti(string filename)
    {
        _bytes = File.ReadAllBytes(filename);
    }

    public Ja2Sti(byte[] bytes)
    {
        _bytes = bytes;
    }

    public List<string> ExtractImages(string path, string prefix = null)
    {
        _path = path;
        _prefix = prefix;

        using var ms = new MemoryStream(_bytes);
        _header = new StiHeader(new KaitaiStream(_bytes));
        //StiHeader.FromFile(_filename);
        
        _is8Otherwise16Bit = Is8BitSti();
        if (!_is8Otherwise16Bit)
        {
            var is16Bit = Is16BitSti();
            if (!is16Bit)
                throw new FormatException("Supported only 8 and 16 bit images");
        }

        return !_is8Otherwise16Bit ? Save16Bit() : Save8Bit();
    }

    private List<string> Save16Bit()
    {
        var result = new List<string>();

        using var kms = new KaitaiStream(_header.FormatSpecificHeader);
        var header16Bit = new Sti16bitHeader(kms);

        var numberOfPixels = _header.Width * _header.Height;
        var redColorMask = header16Bit.RedColorMask;
        var greenColorMask = header16Bit.GreenColorMask;
        var blueColorMask = header16Bit.BlueColorMask;

        var pixelsData = ReadPixelData(_header.M_Io, numberOfPixels);

        var rgbBuffer = new byte[pixelsData.Length * 3];
        for (int i = 0; i < pixelsData.Length; i++)
        {
            ushort pixel = pixelsData[i];

            byte r = (byte)((pixel & redColorMask) >> 8);
            byte g = (byte)((pixel & greenColorMask) >> 3);
            byte b = (byte)((pixel & blueColorMask) << 3);

            int baseIndex = i * 3;
            rgbBuffer[baseIndex + 0] = r;
            rgbBuffer[baseIndex + 1] = g;
            rgbBuffer[baseIndex + 2] = b;
        }

        var bmp = CreateImage16Bit(rgbBuffer, _header.Width, _header.Height);

        var ii = 0;
        // ReSharper disable once RedundantAssignment
        var newFilename = Path.Combine(_path, $"{_prefix}_{ii++}.png");
            
        bmp.Save(newFilename, ImageFormat.Png);
        result.Add(newFilename);

        return result;
    }

    private Bitmap CreateImage16Bit(byte[] rgbBytes, int width, int height)
    {
        Bitmap bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);

        BitmapData bmpData = bmp.LockBits(
            new Rectangle(0, 0, width, height),
            ImageLockMode.WriteOnly,
            PixelFormat.Format24bppRgb);

        int expectedLength = bmpData.Stride * height;
        if (rgbBytes.Length != expectedLength && rgbBytes.Length == width * height * 3)
        {
            // Handle padding manually
            byte[] padded = new byte[expectedLength];
            for (int y = 0; y < height; y++)
            {
                Buffer.BlockCopy(rgbBytes, y * width * 3, padded, y * bmpData.Stride, width * 3);
            }
            Marshal.Copy(padded, 0, bmpData.Scan0, expectedLength);
        }
        else
        {
            Marshal.Copy(rgbBytes, 0, bmpData.Scan0, Math.Min(rgbBytes.Length, expectedLength));
        }

        bmp.UnlockBits(bmpData);

        return bmp;
    }


    private ushort[] ReadPixelData(KaitaiStream reader, int numberOfPixels)
    {
        ushort[] pixels = new ushort[numberOfPixels];
        for (int i = 0; i < numberOfPixels; i++)
        {
            pixels[i] = reader.ReadUInt16(); // Little-endian by default
        }
        return pixels;
    }

    private List<string> Save8Bit()
    {
        var result = new List<string>();

        using var kms = new KaitaiStream(_header.FormatSpecificHeader);
        var header8Bit = new Sti8bit(kms);

        //Console.WriteLine($"{header8Bit.RedDepth} {header8Bit.GreenDepth} {header8Bit.BlueDepth}");

        _header.M_Io.Seek(HeaderSize);
        var paletteSize = header8Bit.NumberOfPaletteColors * 3;
        var paletteBytes = _header.M_Io.ReadBytes(paletteSize);
        var palette = Get8BitRgbPalette(paletteBytes, header8Bit.NumberOfPaletteColors);

        var subImageHeaderSize = header8Bit.NumberOfImages * 16;
        var subImageHeaderBytes = _header.M_Io.ReadBytes(subImageHeaderSize);
        var subImageHeaders = Get8BitSubImageHeaders(subImageHeaderBytes, header8Bit.NumberOfImages);

        var i = 0;
        foreach (var subImageHeader in subImageHeaders)
        {
            var compressedData = _header.M_Io.ReadBytes(subImageHeader.Length);
            var deCompressedData = Etrle.Decompress(compressedData);
            var bmp = Create8BppBmp(deCompressedData, subImageHeader.Width, subImageHeader.Height, palette);

            var newFilename = Path.Combine(_path, $"{_prefix}_{i++}.png");
            bmp.Save(newFilename, ImageFormat.Png);
            result.Add(newFilename);
        }

        return result;
    }

    private Bitmap Create8BppBmp(byte[] imageData, ushort width, ushort height, List<Ja2StiPaletteColor> palette)
    {
        if (imageData.Length != width * height)
            throw new ArgumentException("Image data size doesn't match width*height");

        if (palette.Count != 256)
            throw new ArgumentException("Palette must have exactly 256 colors");

        // Create 8bpp indexed bitmap
        Bitmap bmp = new Bitmap(width, height, PixelFormat.Format8bppIndexed);

        // Apply palette
        ColorPalette pal = bmp.Palette;
        for (int i = 0; i < 256; i++)
        {
            var color = Color.FromArgb(palette[i].R, palette[i].G, palette[i].B);
            pal.Entries[i] = color;
        }
        bmp.Palette = pal;

        // Copy image data into bitmap
        BitmapData bmpData = bmp.LockBits(
            new Rectangle(0, 0, width, height),
            ImageLockMode.WriteOnly,
            PixelFormat.Format8bppIndexed);

        int stride = bmpData.Stride;
        IntPtr ptr = bmpData.Scan0;

        byte[] padded = new byte[stride * height];
        for (int y = 0; y < height; y++)
        {
            Buffer.BlockCopy(imageData, y * width, padded, y * stride, width);
        }

        Marshal.Copy(padded, 0, ptr, padded.Length);
        bmp.UnlockBits(bmpData);

        return bmp;
    }

    private List<Ja2StiPaletteColor> Get8BitRgbPalette(byte[] bytes, uint numberOfPaletteColors)
    {
        var result = new List<Ja2StiPaletteColor>();

        using var ms = new MemoryStream(bytes);
        using var reader = new BinaryReader(ms);

        for (var i = 0; i < numberOfPaletteColors; i++)
        {
            var color = new Ja2StiPaletteColor
            {
                R = reader.ReadByte(),
                G = reader.ReadByte(),
                B = reader.ReadByte()
            };

            result.Add(color);
        }

        return result;
    }

    private List<Ja2StiSubImageHeader> Get8BitSubImageHeaders(byte[] bytes, uint numberOfImages)
    {
        var result = new List<Ja2StiSubImageHeader>();

        using var ms = new MemoryStream(bytes);
        using var reader = new BinaryReader(ms);

        for (var i = 0; i < numberOfImages; i++)
        {
            var header = new Ja2StiSubImageHeader
            {
                Offset = reader.ReadUInt32(),
                Length = reader.ReadUInt32(),
                OffsetX = reader.ReadUInt16(),
                OffsetY = reader.ReadUInt16(),
                Height = reader.ReadUInt16(),
                Width = reader.ReadUInt16()
            };

            result.Add(header);
        }

        return result;
    }

    private bool Is16BitSti()
    {
        return _header.FileIdentifier == "STCI"
               && _header.IsRgb
               && !_header.IsIndexed;
    }

    private bool Is8BitSti()
    {
        return _header.FileIdentifier == "STCI"
               && _header.IsIndexed
               && !_header.IsRgb;
    }
}

public class Ja2StiPaletteColor
{
    public byte R { get; set; }
    public byte G { get; set; }
    public byte B { get; set; }
}

public class Ja2StiSubImageHeader
{
    public uint Offset { get; set; }
    public uint Length { get; set; }
    public ushort OffsetX { get; set; }
    public ushort OffsetY { get; set; }
    public ushort Height { get; set; }
    public ushort Width { get; set; }
}