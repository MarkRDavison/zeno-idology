using System.Diagnostics;

namespace Idology.UserInterface.Layout;

public class LayoutItem
{
    public const uint ContainFlagsMask = 0x000007;
    public const uint AlignFlagsMask = 0x000018;
    public const uint BehaveFlagsMask = 0x0003E0;
    public const uint ItemFlagsMask = 0x001C00;
    public const uint ItemFlagsFixedMask = 0x001800;

    private uint _flags { get; set; }

    public LayoutItem? FirstChild { get; internal set; }
    public LayoutItem? NextSibling { get; internal set; }

    public LayoutEdges RequestedMargin = new();
    public LayoutEdges RequestedPadding = new();
    public LayoutVector RequestedSize = new();

    public float Gap { get; set; } = 0.0f;

    public LayoutRect Rect = new();

    /// <summary>
    /// Controls how any children contained by this layout will be laid out
    /// </summary>
    public ContainFlags Contain
    {
        get => (ContainFlags)(_flags & ContainFlagsMask);
        set => _flags = (_flags & ~ContainFlagsMask) | ((uint)value & ContainFlagsMask);
    }

    public AlignFlags Align
    {
        get => (AlignFlags)(_flags & AlignFlagsMask);
        set => _flags = (_flags & ~AlignFlagsMask) | ((uint)value & AlignFlagsMask);
    }

    public BehaveFlags Behave
    {
        get => (BehaveFlags)(_flags & BehaveFlagsMask);
        set => _flags = (_flags & ~BehaveFlagsMask) | ((uint)value & BehaveFlagsMask);
    }

    public ItemFlags ItemFlags
    {
        get => (ItemFlags)(_flags & ItemFlagsMask);
        set => _flags = (_flags & ~ItemFlagsMask) | ((uint)value & ItemFlagsMask);
    }

    private ItemFlags ItemFlagsHFixed => (ItemFlags)(_flags & ItemFlagsFixedMask);

    public Visibility Visibility { get; set; } = Visibility.Visible;

    public bool ImpactsLayout => Visibility is not Visibility.Collapsed;

    public bool IsInserted
    {
        get => ItemFlags.HasFlag(ItemFlags.Inserted);
        internal set
        {
            if (value)
            {
                ItemFlags |= ItemFlags.Inserted;
            }
            else
            {
                ItemFlags &= ~ItemFlags.Inserted;
            }
        }
    }

    public bool IsItemBreak
    {
        get => Behave.HasFlag(BehaveFlags.Break);
        set
        {
            if (value)
            {
                Behave |= BehaveFlags.Break;
            }
            else
            {
                Behave &= ~BehaveFlags.Break;
            }
        }
    }

    public IEnumerable<LayoutItem> Children
    {
        get
        {
            var child = FirstChild;
            while (child is not null)
            {
                yield return child;
                child = child.NextSibling;
            }
        }
    }

    public IEnumerable<LayoutItem> Siblings
    {
        get
        {
            var item = this;
            while (item.NextSibling is not null)
            {
                yield return item.NextSibling;
                item = item.NextSibling;
            }
        }
    }

    public LayoutItem? LastChild => FirstChild?.LastSibling;
    public LayoutItem LastSibling => Siblings.LastOrDefault(this);

    /// <summary>
    /// Resets this item so it may be re-used.
    /// </summary>
    /// <param name="withChildren">Whether to recursively reset all children as well.</param>
    public void Reset(bool withChildren = false)
    {
        if (withChildren)
        {
            foreach (var child in Children)
            {
                child.Reset();
            }
        }

        _flags = 0;
        FirstChild = null;
        NextSibling = null;
        RequestedMargin = new();
        RequestedPadding = new();
        RequestedSize = new();
        Visibility = Visibility.Visible;
        Rect = new();
    }

    /// <summary>
    /// Inserts an item as a sibling after another item. This allows inserting an item into the middle of an existing list of items within a
    /// parent. It's also more efficient than repeatedly using <seealso cref="AddChild(LayoutItem)"/> in a loop to create a list of items in
    /// a parent, because it does not need to traverse the parent's children each time. So if you're creating a long list of children inside
    /// of a parent, you might prefer to use this after using <seealso cref="AddChild(LayoutItem)"/> to insert the first child.
    /// </summary>
    public void AddSibling(LayoutItem sibling)
    {
        Debug.Assert(sibling != this, "Must not be the same item");
        Debug.Assert(!sibling.IsInserted, "Must not already be inserted");

        sibling.NextSibling = NextSibling;
        sibling.IsInserted = true;

        NextSibling = sibling;
    }

    /// <summary>
    /// Inserts an item into this item, forming a parent - child relationship. An item can contain any number of child items. Items inserted into
    /// a parent are put at the end of the ordering, after any existing siblings.
    /// </summary>
    public void AddChild(LayoutItem child)
    {
        Debug.Assert(child != this, "Must not be the same item");
        Debug.Assert(!child.IsInserted, "Must not already be inserted");

        if (FirstChild is null)
        {
            // We have no existing children, make inserted item the first child.
            FirstChild = child;
            child.IsInserted = true;
        }
        else
        {
            // We have existing items, iterate to find the last child and append the inserted item after it.
            LastChild?.AddSibling(child);
        }
    }

    /// <summary>
    /// Inserts multiple items into this item. Calling this is much faster than repeatedly calling <seealso cref="AddChild(LayoutItem)"/>, as it
    /// will use <seealso cref="AddSibling(LayoutItem)"/> on each child and avoid traversing the children each time.
    /// </summary>
    public void AddChildren(IEnumerable<LayoutItem> children)
    {
        LayoutItem? lastChild = null;
        foreach (var child in children)
        {
            if (lastChild is null)
            {
                AddChild(child);
            }
            else
            {
                lastChild.AddSibling(child);
            }
            lastChild = child;
        }
    }

    /// <summary>
    /// Like <seealso cref="AddChild(LayoutItem)"/>, but puts the new item as the first child in a parent instead of as the last.
    /// </summary>
    public void PushChild(LayoutItem child)
    {
        Debug.Assert(child != this, "Must not be same item");
        Debug.Assert(!child.IsInserted, "Must not already be inserted");

        var oldChild = FirstChild;
        FirstChild = child;

        child.IsInserted = true;
        child.NextSibling = oldChild;
    }

    /// <summary>
    /// Run layout calculations starting from this item.
    ///
    /// Running the layout calculations from a specific item is useful if you want
    /// to iteratively re-run parts of your layout hierarchy, or if you are only
    /// interested in updating certain subsets of it. Be careful when using this --
    /// it's easy to generate bad output if the parent items haven't yet had their
    /// output rectangles calculated, or if they've been invalidated (e.g. due to
    /// re-allocation).
    /// </summary>
    public void Run()
    {
        CalcSize(0);
        Arrange(0);
        CalcSize(1);
        Arrange(1);
    }

    private void CalcSize(int dim)
    {
        foreach (var child in Children)
        {
            // NOTE: this is recursive and will run out of stack space if items are nested too deeply.
            child.CalcSize(dim);
        }

        // Set the mutable rect output data to the starting input data
        Rect[dim] = RequestedMargin[dim];

        // If we have an explicit input size, just set our output size (which other
        // calc_size and arrange procedures will use) to it.
        if (RequestedSize[dim] != 0)
        {
            Rect[2 + dim] = RequestedSize[dim];
            return;
        }

        // Calculate our size based on children items. Note that we've already
        // called lay_calc_size on our children at this point.
        float cal_size;
        switch (Contain)
        {
            case ContainFlags.Column | ContainFlags.Wrap:
                // flex model
                if (dim == 1)
                { // direction
                    cal_size = CalcStackedSize(1);
                }
                else
                {
                    cal_size = CalcOverlayedSize(0);
                }
                break;

            case ContainFlags.Row | ContainFlags.Wrap:
                // flex model
                if (dim == 0)
                { // direction
                    cal_size = CalcWrappedStackedSize(0);
                }
                else
                {
                    cal_size = CalcWrappedOverlayedSize(1);
                }
                break;

            case ContainFlags.Column:
            case ContainFlags.Row:
                // flex model
                if ((_flags & 1) == (uint)dim)
                { // direction
                    cal_size = CalcStackedSize(dim);
                }
                else
                {
                    cal_size = CalcOverlayedSize(dim);
                }
                break;

            default:
                // layout model
                cal_size = CalcOverlayedSize(dim);
                break;
        }

        // Set our output data size. Will be used by parent calc_size procedures,
        // and by arrange procedures.
        Rect[2 + dim] = cal_size;
    }

    private float CalcStackedSize(int dim)
    {
        int wdim = dim + 2;

        float needSize = 0;
        int count = 0;

        foreach (var child in Children.Where(c => c.ImpactsLayout))
        {
            needSize +=
                child.Rect[dim] +
                child.Rect[wdim] +
                child.RequestedMargin[wdim];

            count++;
        }

        if (count > 1)
        {
            needSize += Gap * (count - 1);
        }

        return RequestedPadding.GetDimension(dim) + needSize;
    }

    private float CalcOverlayedSize(int dim)
    {
        int wdim = dim + 2;
        float need_size = 0;
        foreach (var child in Children.Where(c => c.ImpactsLayout))
        {
            // width = start margin + calculated width + end margin
            float child_size = child.Rect[dim] + child.Rect[wdim] + child.RequestedMargin[wdim];
            need_size = Math.Max(need_size, child_size);
        }

        return RequestedPadding.GetDimension(dim) + need_size;
    }

    private float CalcWrappedStackedSize(int dim)
    {
        int wdim = dim + 2;

        float lineSize = 0;
        float maxLine = 0;
        int lineCount = 0;

        foreach (var child in Children.Where(c => c.ImpactsLayout))
        {
            if (child.Behave.HasFlag(BehaveFlags.Break))
            {
                if (lineCount > 1)
                {
                    lineSize += Gap * (lineCount - 1);
                }

                maxLine = Math.Max(maxLine, lineSize);

                lineSize = 0;
                lineCount = 0;
            }

            lineSize +=
                child.Rect[dim] +
                child.Rect[wdim] +
                child.RequestedMargin[wdim];

            lineCount++;
        }

        if (lineCount > 1)
        {
            lineSize += Gap * (lineCount - 1);
        }

        return RequestedPadding.GetDimension(dim) + Math.Max(maxLine, lineSize);
    }

    private float CalcWrappedOverlayedSize(int dim)
    {
        int wdim = dim + 2;
        float need_size = 0;
        float need_size2 = 0;
        foreach (var child in Children.Where(c => c.ImpactsLayout))
        {
            if (child.Behave.HasFlag(BehaveFlags.Break))
            {
                need_size2 += need_size;
                need_size = 0;
            }
            float child_size = child.Rect[dim] + child.Rect[wdim] + child.RequestedMargin[wdim];
            need_size = Math.Max(need_size, child_size);
        }

        return RequestedPadding.GetDimension(dim) + need_size2 + need_size;
    }

    private void Arrange(int dim)
    {
        switch (Contain)
        {
            case ContainFlags.Column | ContainFlags.Wrap:
                if (dim != 0)
                {
                    ArrangeStacked(1, true);
                    float offset = ArrangeWrappedOverlaySqueezed(0);
                    Rect[2] = offset - Rect[0];
                }
                break;

            case ContainFlags.Row | ContainFlags.Wrap:
                if (dim == 0)
                {
                    ArrangeStacked(0, true);
                }
                else
                {
                    // discard return value
                    ArrangeWrappedOverlaySqueezed(1);
                }
                break;

            case ContainFlags.Column:
            case ContainFlags.Row:
                if ((_flags & 1) == (uint)dim)
                {
                    ArrangeStacked(dim, false);
                }
                else
                {
                    ArrangeOverlaySqueezedRange(dim, FirstChild, null,
                        Rect[dim] + RequestedPadding[dim],
                        Rect[2 + dim] - RequestedPadding.GetDimension(dim));
                }
                break;

            default:
                ArrangeOverlay(dim);
                break;
        }

        foreach (var child in Children)
        {
            // NOTE: this is recursive and will run out of stack space if items are nested too deeply.
            child.Arrange(dim);
        }
    }

    private void ArrangeStacked(int dim, bool wrap)
    {
        int wdim = dim + 2;

        float space = Rect[wdim] - RequestedPadding.GetDimension(dim);
        float max_x2 = Rect[dim] + space + RequestedPadding[dim];

        var startChild = FirstChild;

        while (startChild is not null)
        {
            float used = 0;
            int count = 0;             // fillers (HFill)
            int squeezed_count = 0;    // squeezable items
            int total = 0;
            bool hardbreak = false;

            var child = startChild;
            LayoutItem? endChild = null;

            bool firstItem = true;

            // -------------------------------------------------
            // FIRST PASS — measure used space
            // -------------------------------------------------
            while (child is not null)
            {
                if (child.ImpactsLayout)
                {
                    var flags = (BehaveFlags)((uint)child.Behave >> dim);
                    var fflags = (ItemFlags)((uint)child.ItemFlagsHFixed >> dim);

                    float extend = used;

                    // GAP
                    if (!firstItem)
                    {
                        extend += Gap;
                    }

                    if (flags.HasFlag(BehaveFlags.HFill))
                    {
                        count++;
                        extend += child.Rect[dim]
                               + child.RequestedMargin[wdim];
                    }
                    else
                    {
                        if (!fflags.HasFlag(ItemFlags.HFixed))
                            squeezed_count++;

                        extend += child.Rect[dim]
                               + child.Rect[wdim]
                               + child.RequestedMargin[wdim];
                    }

                    // wrapping logic
                    if (wrap && total > 0 &&
                        ((extend > space) ||
                         child.Behave.HasFlag(BehaveFlags.Break)))
                    {
                        endChild = child;
                        hardbreak = child.Behave.HasFlag(BehaveFlags.Break);

                        // preserve break marker
                        child._flags |= (uint)BehaveFlags.Break;
                        break;
                    }

                    used = extend;
                    total++;
                    firstItem = false;
                }

                child = child.NextSibling;
            }

            float extra_space = space - used;
            float filler = 0;
            float spacer = 0;
            float extra_margin = 0;
            float eater = 0;

            if (extra_space > 0)
            {
                if (count > 0)
                {
                    filler = extra_space / count;
                }
                else if (total > 0)
                {
                    switch (Align)
                    {
                        case AlignFlags.Justify:
                            if (!wrap || ((endChild is not null) && !hardbreak))
                            {
                                spacer = extra_space / (total - 1);
                            }
                            break;

                        case AlignFlags.Start:
                            break;

                        case AlignFlags.End:
                            extra_margin = extra_space;
                            break;

                        default:
                            extra_margin = extra_space / 2;
                            break;
                    }
                }
            }
            else if (!wrap && squeezed_count > 0)
            {
                eater = extra_space / squeezed_count;
            }

            // -------------------------------------------------
            // SECOND PASS — assign positions/sizes
            // -------------------------------------------------
            float x = Rect[dim] + RequestedPadding[dim];
            child = startChild;

            firstItem = true;

            while (child != endChild && child is not null)
            {
                if (child.ImpactsLayout)
                {
                    var flags = (BehaveFlags)((uint)child.Behave >> dim);
                    var fflags = (ItemFlags)((uint)child.ItemFlagsHFixed >> dim);

                    // GAP
                    if (!firstItem)
                    {
                        x += Gap;
                    }

                    x += child.Rect[dim] + extra_margin;

                    float x1;

                    if (flags.HasFlag(BehaveFlags.HFill))
                    {
                        x1 = x + filler;
                    }
                    else if (fflags.HasFlag(ItemFlags.HFixed))
                    {
                        x1 = x + child.Rect[wdim];
                    }
                    else
                    {
                        x1 = x + Math.Max(0.0f, child.Rect[wdim] + eater);
                    }

                    float ix0 = x;
                    float ix1 = wrap
                        ? Math.Min(max_x2 - child.RequestedMargin[wdim], x1)
                        : x1;

                    child.Rect[dim] = ix0;
                    child.Rect[wdim] = ix1 - ix0;

                    x = x1 + child.RequestedMargin[wdim];
                    extra_margin = spacer;

                    firstItem = false;
                }

                child = child.NextSibling;
            }

            startChild = endChild;
        }
    }

    private void ArrangeOverlay(int dim)
    {
        int wdim = dim + 2;
        float offset = Rect[dim] + RequestedPadding[dim];
        float space = Rect[wdim] - RequestedPadding.GetDimension(dim);

        foreach (var child in Children.Where(c => c.ImpactsLayout))
        {
            var b_flags = (BehaveFlags)((uint)child.Behave >> dim);

            switch (b_flags & BehaveFlags.HFill)
            {
                case BehaveFlags.HCenter:
                    child.Rect[dim] += (space - child.Rect[wdim] - child.RequestedMargin[wdim]) / 2;
                    break;

                case BehaveFlags.Right:
                    child.Rect[dim] += space - child.Rect[wdim] - child.RequestedMargin[dim] - child.RequestedMargin[wdim];
                    break;

                case BehaveFlags.HFill:
                    child.Rect[wdim] = Math.Max(0.0f, space - child.Rect[dim] - child.RequestedMargin[wdim]);
                    break;

                default:
                    break;
            }

            child.Rect[dim] += offset;
        }
    }

    private float ArrangeWrappedOverlaySqueezed(int dim)
    {
        int wdim = dim + 2;
        float offset = Rect[dim] + RequestedPadding[dim];
        float need_size = 0;
        var startChild = FirstChild;
        foreach (var child in Children.Where(c => c.ImpactsLayout))
        {
            if (child.Behave.HasFlag(BehaveFlags.Break))
            {
                ArrangeOverlaySqueezedRange(dim, startChild, child, offset, need_size);
                offset += need_size;
                startChild = child;
                need_size = 0;
            }
            float child_size = child.Rect[dim] + child.Rect[wdim] + child.RequestedMargin[wdim];
            need_size = Math.Max(need_size, child_size);
        }
        ArrangeOverlaySqueezedRange(dim, startChild, null, offset, need_size);
        offset += need_size;
        return offset;
    }

    private static void ArrangeOverlaySqueezedRange(int dim, LayoutItem? startItem, LayoutItem? endItem, float offset, float space)
    {
        int wdim = dim + 2;
        var item = startItem;
        while (item != endItem && item is not null)
        {
            if (item.ImpactsLayout)
            {
                var b_flags = (BehaveFlags)((uint)item.Behave >> dim);

                float min_size = Math.Max(0.0f, space - item.Rect[dim] - item.RequestedMargin[wdim]);
                switch (b_flags & BehaveFlags.HFill)
                {
                    case BehaveFlags.HCenter:
                        item.Rect[wdim] = Math.Min(item.Rect[wdim], min_size);
                        item.Rect[dim] += (space - item.Rect[wdim] - item.RequestedMargin[wdim]) / 2;
                        break;

                    case BehaveFlags.Right:
                        item.Rect[wdim] = Math.Min(item.Rect[wdim], min_size);
                        item.Rect[dim] = space - item.Rect[wdim] - item.RequestedMargin[wdim];
                        break;

                    case BehaveFlags.HFill:
                        item.Rect[wdim] = min_size;
                        break;

                    default:
                        item.Rect[wdim] = Math.Min(item.Rect[wdim], min_size);
                        break;
                }

                item.Rect[dim] += offset;
            }
            item = item.NextSibling;
        }
    }
}