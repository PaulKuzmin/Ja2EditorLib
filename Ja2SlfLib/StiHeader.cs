// This is a generated file! Please edit source .ksy file and use kaitai-struct-compiler to rebuild


using Kaitai;
// ReSharper disable InconsistentNaming

namespace Ja2SlfLib
{
    public partial class StiHeader : KaitaiStruct
    {
        public static StiHeader FromFile(string fileName)
        {
            return new StiHeader(new KaitaiStream(fileName));
        }

        public StiHeader(KaitaiStream p__io, KaitaiStruct p__parent = null, StiHeader p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root ?? this;
            f_isIndexed = false;
            f_isRgb = false;
            f_isAuxObjectData = false;
            f_isEtrle = false;
            f_isZlib = false;
            _read();
        }
        private void _read()
        {
            _fileIdentifier = System.Text.Encoding.GetEncoding("ASCII").GetString(m_io.ReadBytes(4));
            _initialSize = m_io.ReadU4le();
            _sizeAfterCompression = m_io.ReadU4le();
            _transparentColor = m_io.ReadU4le();
            _flags = m_io.ReadU4le();
            _height = m_io.ReadU2le();
            _width = m_io.ReadU2le();
            _formatSpecificHeader = m_io.ReadBytes(20);
            _colorDepth = m_io.ReadU1();
            _reserved1 = m_io.ReadBytes(3);
            _auxDataSize = m_io.ReadU4le();
            _reserved2 = m_io.ReadBytes(12);
        }
        private bool f_isIndexed;
        private bool _isIndexed;
        public bool IsIndexed
        {
            get
            {
                if (f_isIndexed)
                    return _isIndexed;
                _isIndexed = (bool) ((Flags & 8) != 0);
                f_isIndexed = true;
                return _isIndexed;
            }
        }
        private bool f_isRgb;
        private bool _isRgb;
        public bool IsRgb
        {
            get
            {
                if (f_isRgb)
                    return _isRgb;
                _isRgb = (bool) ((Flags & 4) != 0);
                f_isRgb = true;
                return _isRgb;
            }
        }
        private bool f_isAuxObjectData;
        private bool _isAuxObjectData;
        public bool IsAuxObjectData
        {
            get
            {
                if (f_isAuxObjectData)
                    return _isAuxObjectData;
                _isAuxObjectData = (bool) ((Flags & 1) != 0);
                f_isAuxObjectData = true;
                return _isAuxObjectData;
            }
        }
        private bool f_isEtrle;
        private bool _isEtrle;
        public bool IsEtrle
        {
            get
            {
                if (f_isEtrle)
                    return _isEtrle;
                _isEtrle = (bool) ((Flags & 32) != 0);
                f_isEtrle = true;
                return _isEtrle;
            }
        }
        private bool f_isZlib;
        private bool _isZlib;
        public bool IsZlib
        {
            get
            {
                if (f_isZlib)
                    return _isZlib;
                _isZlib = (bool) ((Flags & 16) != 0);
                f_isZlib = true;
                return _isZlib;
            }
        }
        private string _fileIdentifier;
        private uint _initialSize;
        private uint _sizeAfterCompression;
        private uint _transparentColor;
        private uint _flags;
        private ushort _height;
        private ushort _width;
        private byte[] _formatSpecificHeader;
        private byte _colorDepth;
        private byte[] _reserved1;
        private uint _auxDataSize;
        private byte[] _reserved2;
        private StiHeader m_root;
        private KaitaiStruct m_parent;

        /// <summary>
        /// Must be &quot;STCI&quot;
        /// </summary>
        public string FileIdentifier { get { return _fileIdentifier; } }

        /// <summary>
        /// Size of image data before compression
        /// </summary>
        public uint InitialSize { get { return _initialSize; } }

        /// <summary>
        /// Size of image data after compression
        /// </summary>
        public uint SizeAfterCompression { get { return _sizeAfterCompression; } }

        /// <summary>
        /// Index or color treated as transparent
        /// </summary>
        public uint TransparentColor { get { return _transparentColor; } }

        /// <summary>
        /// Bit flags controlling image type
        /// </summary>
        public uint Flags { get { return _flags; } }
        public ushort Height { get { return _height; } }
        public ushort Width { get { return _width; } }

        /// <summary>
        /// Either 8-bit or 16-bit subheader depending on flags
        /// </summary>
        public byte[] FormatSpecificHeader { get { return _formatSpecificHeader; } }
        public byte ColorDepth { get { return _colorDepth; } }
        public byte[] Reserved1 { get { return _reserved1; } }

        /// <summary>
        /// Optional auxiliary data section size
        /// </summary>
        public uint AuxDataSize { get { return _auxDataSize; } }
        public byte[] Reserved2 { get { return _reserved2; } }
        public StiHeader M_Root { get { return m_root; } }
        public KaitaiStruct M_Parent { get { return m_parent; } }
    }
}
