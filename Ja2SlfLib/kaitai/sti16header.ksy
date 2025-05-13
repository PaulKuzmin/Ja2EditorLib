meta:
  id: sti_16bit_header
  endian: le
  title: STCI 16-bit Format-Specific Header

seq:
  - id: red_color_mask
    type: u4
  - id: green_color_mask
    type: u4
  - id: blue_color_mask
    type: u4
  - id: alpha_channel_mask
    type: u4
  - id: red_color_depth
    type: u1
  - id: green_color_depth
    type: u1
  - id: blue_color_depth
    type: u1
  - id: alpha_channel_depth
    type: u1
