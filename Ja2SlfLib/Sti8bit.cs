// This is a generated file! Please edit source .ksy file and use kaitai-struct-compiler to rebuild


using Kaitai;
// ReSharper disable InconsistentNaming

namespace Ja2SlfLib
{
    public partial class Sti8bit : KaitaiStruct
    {
        public static Sti8bit FromFile(string fileName)
        {
            return new Sti8bit(new KaitaiStream(fileName));
        }

        public Sti8bit(KaitaiStream p__io, KaitaiStruct p__parent = null, Sti8bit p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root ?? this;
            _read();
        }
        private void _read()
        {
            _numberOfPaletteColors = m_io.ReadU4le();
            _numberOfImages = m_io.ReadU2le();
            _redDepth = m_io.ReadU1();
            _greenDepth = m_io.ReadU1();
            _blueDepth = m_io.ReadU1();
            _reserved = m_io.ReadBytes(11);
        }
        private uint _numberOfPaletteColors;
        private ushort _numberOfImages;
        private byte _redDepth;
        private byte _greenDepth;
        private byte _blueDepth;
        private byte[] _reserved;
        private Sti8bit m_root;
        private KaitaiStruct m_parent;
        public uint NumberOfPaletteColors { get { return _numberOfPaletteColors; } }
        public ushort NumberOfImages { get { return _numberOfImages; } }
        public byte RedDepth { get { return _redDepth; } }
        public byte GreenDepth { get { return _greenDepth; } }
        public byte BlueDepth { get { return _blueDepth; } }
        public byte[] Reserved { get { return _reserved; } }
        public Sti8bit M_Root { get { return m_root; } }
        public KaitaiStruct M_Parent { get { return m_parent; } }
    }
}
