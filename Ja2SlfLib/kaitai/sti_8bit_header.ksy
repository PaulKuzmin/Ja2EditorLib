meta:
  id: sti_8bit
  file-extension: sti
  endian: le
  title: STCI 8-bit Indexed Image (simplified)

seq:
  - id: number_of_palette_colors
    type: u4
  - id: number_of_images
    type: u2
  - id: red_depth
    type: u1
  - id: green_depth
    type: u1
  - id: blue_depth
    type: u1
  - id: reserved
    size: 11  

