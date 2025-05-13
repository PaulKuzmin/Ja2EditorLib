// This is a generated file! Please edit source .ksy file and use kaitai-struct-compiler to rebuild

using Kaitai;

namespace Ja2SlfLib
{
    public partial class Ja2Slf : KaitaiStruct
    {
        public static Ja2Slf FromFile(string fileName)
        {
            return new Ja2Slf(new KaitaiStream(fileName));
        }


        public enum EntryState
        {
            Ok = 0,
            Old = 1,
            DoesNotExist = 254,
            Deleted = 255,
        }

        public Ja2Slf(KaitaiStream p__io, KaitaiStruct p__parent = null, Ja2Slf p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root ?? this;
            f_files = false;
            _read();
        }

        private void _read()
        {
            _header = new SlfHeader(m_io, this, m_root);
        }

        public partial class SlfHeader : KaitaiStruct
        {
            public static SlfHeader FromFile(string fileName)
            {
                return new SlfHeader(new KaitaiStream(fileName));
            }

            public SlfHeader(KaitaiStream p__io, Ja2Slf p__parent = null, Ja2Slf p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }

            private void _read()
            {
                _libraryName = System.Text.Encoding.GetEncoding("ASCII").GetString(
                    KaitaiStream.BytesTerminate(KaitaiStream.BytesStripRight(m_io.ReadBytes(256), 256), 0, false));
                _libraryPath = System.Text.Encoding.GetEncoding("ASCII").GetString(
                    KaitaiStream.BytesTerminate(KaitaiStream.BytesStripRight(m_io.ReadBytes(256), 256), 0, false));
                _numEntries = m_io.ReadS4le();
                _okEntries = m_io.ReadS4le();
                _sort = m_io.ReadU2le();
                _version = m_io.ReadU2le();
                _containsSubdirectories = m_io.ReadU1();
                _padding1 = m_io.ReadBytes(3);
                _reserved = m_io.ReadS4le();
            }

            private string _libraryName;
            private string _libraryPath;
            private int _numEntries;
            private int _okEntries;
            private ushort _sort;
            private ushort _version;
            private byte _containsSubdirectories;
            private byte[] _padding1;
            private int _reserved;
            private Ja2Slf m_root;
            private Ja2Slf m_parent;

            /// <summary>
            /// Library name, null-terminated, padded with zeros.
            /// </summary>
            public string LibraryName
            {
                get { return _libraryName; }
            }

            /// <summary>
            /// Library path, null-terminated, padded with zeros.
            /// </summary>
            public string LibraryPath
            {
                get { return _libraryPath; }
            }

            /// <summary>
            /// Total number of entries in the archive.
            /// </summary>
            public int NumEntries
            {
                get { return _numEntries; }
            }

            /// <summary>
            /// Number of entries with state OK.
            /// </summary>
            public int OkEntries
            {
                get { return _okEntries; }
            }

            /// <summary>
            /// Sorting indicator. Usually 0xFFFF.
            /// </summary>
            public ushort Sort
            {
                get { return _sort; }
            }

            /// <summary>
            /// Format version. Usually 0x0200.
            /// </summary>
            public ushort Version
            {
                get { return _version; }
            }

            /// <summary>
            /// 1 if paths contain subdirectories.
            /// </summary>
            public byte ContainsSubdirectories
            {
                get { return _containsSubdirectories; }
            }

            /// <summary>
            /// Alignment padding.
            /// </summary>
            public byte[] Padding1
            {
                get { return _padding1; }
            }

            /// <summary>
            /// Reserved (not used).
            /// </summary>
            public int Reserved
            {
                get { return _reserved; }
            }

            public Ja2Slf M_Root
            {
                get { return m_root; }
            }

            public Ja2Slf M_Parent
            {
                get { return m_parent; }
            }
        }

        public partial class SlfEntry : KaitaiStruct
        {
            public static SlfEntry FromFile(string fileName)
            {
                return new SlfEntry(new KaitaiStream(fileName));
            }

            public SlfEntry(KaitaiStream p__io, Ja2Slf p__parent = null, Ja2Slf p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }

            private void _read()
            {
                _filePath = System.Text.Encoding.GetEncoding("ASCII").GetString(
                    KaitaiStream.BytesTerminate(KaitaiStream.BytesStripRight(m_io.ReadBytes(256), 256), 0, false));
                _offset = m_io.ReadU4le();
                _length = m_io.ReadU4le();
                _state = ((Ja2Slf.EntryState)m_io.ReadU1());
                _reserved1 = m_io.ReadBytes(1);
                _padding2 = m_io.ReadBytes(2);
                _filetime = m_io.ReadU8le();
                _reserved2 = m_io.ReadU2le();
                _padding3 = m_io.ReadBytes(2);
            }

            private string _filePath;
            private uint _offset;
            private uint _length;
            private EntryState _state;
            private byte[] _reserved1;
            private byte[] _padding2;
            private ulong _filetime;
            private ushort _reserved2;
            private byte[] _padding3;
            private Ja2Slf m_root;
            private Ja2Slf m_parent;

            /// <summary>
            /// Relative file path inside the SLF archive.
            /// </summary>
            public string FilePath
            {
                get { return _filePath; }
            }

            /// <summary>
            /// Offset of file data in SLF.
            /// </summary>
            public uint Offset
            {
                get { return _offset; }
            }

            /// <summary>
            /// Length of the file data.
            /// </summary>
            public uint Length
            {
                get { return _length; }
            }

            /// <summary>
            /// File entry state.
            /// </summary>
            public EntryState State
            {
                get { return _state; }
            }

            /// <summary>
            /// Unused.
            /// </summary>
            public byte[] Reserved1
            {
                get { return _reserved1; }
            }

            /// <summary>
            /// Alignment padding.
            /// </summary>
            public byte[] Padding2
            {
                get { return _padding2; }
            }

            /// <summary>
            /// Windows FILETIME timestamp.
            /// </summary>
            public ulong Filetime
            {
                get { return _filetime; }
            }

            /// <summary>
            /// Unused.
            /// </summary>
            public ushort Reserved2
            {
                get { return _reserved2; }
            }

            /// <summary>
            /// Alignment padding.
            /// </summary>
            public byte[] Padding3
            {
                get { return _padding3; }
            }

            public Ja2Slf M_Root
            {
                get { return m_root; }
            }

            public Ja2Slf M_Parent
            {
                get { return m_parent; }
            }
        }

        private bool f_files;
        private List<SlfEntry> _files;

        public List<SlfEntry> Files
        {
            get
            {
                if (f_files)
                    return _files;
                long _pos = m_io.Pos;
                m_io.Seek((M_Io.Size - (Header.NumEntries * 280)));
                _files = new List<SlfEntry>();
                for (var i = 0; i < Header.NumEntries; i++)
                {
                    _files.Add(new SlfEntry(m_io, this, m_root));
                }

                m_io.Seek(_pos);
                f_files = true;
                return _files;
            }
        }

        private SlfHeader _header;
        private Ja2Slf m_root;
        private KaitaiStruct m_parent;

        public SlfHeader Header
        {
            get { return _header; }
        }

        public Ja2Slf M_Root
        {
            get { return m_root; }
        }

        public KaitaiStruct M_Parent
        {
            get { return m_parent; }
        }
    }
}