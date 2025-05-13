meta:
  id: ja2_slf
  title: Jagged Alliance 2 SLF Archive
  file-extension: slf
  endian: le

seq:
  - id: header
    type: slf_header

instances:
  files:
    pos: _io.size - header.num_entries * 280
    type: slf_entry
    repeat: expr
    repeat-expr: header.num_entries

# Header structure (532 bytes total)
types:
  slf_header:
    seq:
      - id: library_name
        type: str
        size: 256
        encoding: ASCII
        terminator: 0
        pad-right: 256
        doc: "Library name, null-terminated, padded with zeros."

      - id: library_path
        type: str
        size: 256
        encoding: ASCII
        terminator: 0
        pad-right: 256
        doc: "Library path, null-terminated, padded with zeros."

      - id: num_entries
        type: s4
        doc: "Total number of entries in the archive."

      - id: ok_entries
        type: s4
        doc: "Number of entries with state OK."

      - id: sort
        type: u2
        doc: "Sorting indicator. Usually 0xFFFF."

      - id: version
        type: u2
        doc: "Format version. Usually 0x0200."

      - id: contains_subdirectories
        type: u1
        doc: "1 if paths contain subdirectories."

      - id: padding1
        size: 3
        doc: "Alignment padding."

      - id: reserved
        type: s4
        doc: "Reserved (not used)."

# Entry structure (280 bytes)
  slf_entry:
    seq:
      - id: file_path
        type: str
        size: 256
        encoding: ASCII
        terminator: 0
        pad-right: 256
        doc: "Relative file path inside the SLF archive."

      - id: offset
        type: u4
        doc: "Offset of file data in SLF."

      - id: length
        type: u4
        doc: "Length of the file data."

      - id: state
        type: u1
        enum: entry_state
        doc: "File entry state."

      - id: reserved1
        size: 1
        doc: "Unused."

      - id: padding2
        size: 2
        doc: "Alignment padding."

      - id: filetime
        type: u8
        doc: "Windows FILETIME timestamp."

      - id: reserved2
        type: u2
        doc: "Unused."

      - id: padding3
        size: 2
        doc: "Alignment padding."

enums:
  entry_state:
    0: ok
    1: old
    254: does_not_exist
    255: deleted