// This is a generated file! Please edit source .ksy file and use kaitai-struct-compiler to rebuild


using Kaitai;

namespace Ja2SlfLib
{
    public partial class Sti16bitHeader : KaitaiStruct
    {
        public static Sti16bitHeader FromFile(string fileName)
        {
            return new Sti16bitHeader(new KaitaiStream(fileName));
        }

        public Sti16bitHeader(KaitaiStream p__io, KaitaiStruct p__parent = null, Sti16bitHeader p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root ?? this;
            _read();
        }
        private void _read()
        {
            _redColorMask = m_io.ReadU4le();
            _greenColorMask = m_io.ReadU4le();
            _blueColorMask = m_io.ReadU4le();
            _alphaChannelMask = m_io.ReadU4le();
            _redColorDepth = m_io.ReadU1();
            _greenColorDepth = m_io.ReadU1();
            _blueColorDepth = m_io.ReadU1();
            _alphaChannelDepth = m_io.ReadU1();
        }
        private uint _redColorMask;
        private uint _greenColorMask;
        private uint _blueColorMask;
        private uint _alphaChannelMask;
        private byte _redColorDepth;
        private byte _greenColorDepth;
        private byte _blueColorDepth;
        private byte _alphaChannelDepth;
        private Sti16bitHeader m_root;
        private KaitaiStruct m_parent;
        public uint RedColorMask { get { return _redColorMask; } }
        public uint GreenColorMask { get { return _greenColorMask; } }
        public uint BlueColorMask { get { return _blueColorMask; } }
        public uint AlphaChannelMask { get { return _alphaChannelMask; } }
        public byte RedColorDepth { get { return _redColorDepth; } }
        public byte GreenColorDepth { get { return _greenColorDepth; } }
        public byte BlueColorDepth { get { return _blueColorDepth; } }
        public byte AlphaChannelDepth { get { return _alphaChannelDepth; } }
        public Sti16bitHeader M_Root { get { return m_root; } }
        public KaitaiStruct M_Parent { get { return m_parent; } }
    }
}
