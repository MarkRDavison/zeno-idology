namespace Idology.UserInterface.Layout;

[Flags]
public enum ItemFlags : uint
{
    // item has been inserted
    Inserted = 0x400,

    // horizontal size has been explicitly set
    HFixed = 0x800,
    // vertical size has been explicitly set
    VFixed = 0x1000,
}

[Flags]
public enum ContainFlags : uint // aka: box flags
{
    // flex-direction (bit 0+1)

    // left to right
    Row = 0x002,
    // top to bottom
    Column = 0x003,

    // model (bit 1)

    // free layout
    Layout = 0x000,
    // flex model
    Flex = 0x002,

    // flex-wrap (bit 2)

    // single-line
    NoWrap = 0x000,
    // multi-line, wrap left to right
    Wrap = 0x004,


    // align-items
    // can be implemented by putting a flex container in a layout container,
    // then using LAY_TOP, LAY_BOTTOM, LAY_VFILL, LAY_VCENTER, etc.
    // FILL is equivalent to stretch/grow

    // align-content (start, end, center, stretch)
    // can be implemented by putting a flex container in a layout container,
    // then using LAY_TOP, LAY_BOTTOM, LAY_VFILL, LAY_VCENTER, etc.
    // FILL is equivalent to stretch; space-between is not supported.
}

[Flags]
public enum BehaveFlags : uint // aka: layout flags
{
    // attachments (bit 5-8)
    // fully valid when parent uses LAY_LAYOUT model
    // partially valid when in LAY_FLEX model

    // anchor to left item or left side of parent
    Left = 0x020,
    // anchor to top item or top side of parent
    Top = 0x040,
    // anchor to right item or right side of parent
    Right = 0x080,
    // anchor to bottom item or bottom side of parent
    Bottom = 0x100,
    // anchor to both left and right item or parent borders
    HFill = 0x0a0,
    // anchor to both top and bottom item or parent borders
    VFill = 0x140,
    // center horizontally, with left margin as offset
    HCenter = 0x000,
    // center vertically, with top margin as offset
    VCenter = 0x000,
    // center in both directions, with left/top margin as offset
    Center = 0x000,
    // anchor to all four directions
    Fill = HFill | VFill,

    // When in a wrapping container, put this element on a new line. Wrapping
    // layout code auto-inserts LAY_BREAK flags as needed. See GitHub issues for
    // TODO related to this.
    //
    // Drawing routines can read this via item pointers as needed after
    // performing layout calculations.
    Break = 0x200,
}

[Flags]
public enum AlignFlags : uint // aka: box flags
{
    // justify-content (start, end, center, space-between)
    // at start of row/column
    Start = 0x008,
    // at center of row/column
    Middle = 0x000,
    // at end of row/column
    End = 0x010,
    // insert spacing to stretch across whole row/column
    Justify = 0x018,
}