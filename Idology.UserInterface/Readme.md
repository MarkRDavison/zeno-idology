# Thoughts

2 pass system
 - layout
  - sizes (tree bottom up)
  - grow sizing (tree top down)
  - positions
 - render

- FIT SIZING Limit size to maximum here
- FIT is the default sizing

FIT SIZING WIDTHS
GROW/SHRINK SIZING WIDTHS
WRAP TEXT/FIT WITHINK SHRUNK
FIT SIZING HEIGHTS
GROW/SHRINK SIZING HEIGHTS
POSITIONS AND ALIGNMENT
RENDER

GROW means expand the smallest elements

PrefferedWidth and MinimumWidth

sizing = {
 width = GROW({min = 300, max = 500})
 HEIGHT = FIT({min = 150, max = 200})
}

```
// TODO: Make independant of width/height and be along/against layout axis
void CloseElement(UiElement element)
{
  var parent = element.Parent;
  var padding = element.Padding;
  element.Width += padding.Left + padding.Right;
  element.Height += padding.Top + padding.Bottom;
  var childGap = (parent.Children.Count - 1) * parent.ChildGap;
  if (parent.LayoutDirection == Layout::LeftToRight)
  {
    element.Width += childGap;
    parent.Width += element.Width;
    parent.MinWidth += element.MinWidth;
    parent.Height = max(element.Height, parent.Height);
    parent.MinHeight += max(element.MinHeight, parent.MinHeight);
  }
  else 
  {
    element.Height += childGap;
    parent.Width = max(element.Width, parent.Width);
    parent.MinWidth += max(element.MinWidth, parent.MinWidth);
    parent.Height += element.Height;
    parent.MinHeight += element.MinHeight;
  }
}
```

```
// TODO: Make independant of width/height and be along/against layout axis
void GrowAndShrinkChildElements(UiElement parent, IList<UiElement> growable, IList<UiElement> shrinkable) 
{
  var remainingWidth = parent.Width;
  var remainingHeight = parent.Height;
  
  remainingWidth -= parent.Padding.Left + parent.Padding.Right;
  remainingHeight -= parent.Padding.Top + parent.Padding.Bottom;

  foreach (var c in parent.Children)
  {
    remainingWidth -= child.Width; // Grow elements will have size 0
  }

  remainingWidth -= (parent.Children.Count - 1) * parent.ChildGap;

  // Grow
  while (remainingWidth > 0)
  {
    var smallest = growable[0].Width;
    var secondSmallest = float.MAX;
    var widthToAdd = remainingWidth;

    foreach (var c in growable)
    {
      if (c.Width < smallest)
      {
        secondSmallest = smallest;
        smallest = c.Width;
      }
      if (c.Width > smallest)
      {
        secondSmallest = Math.Min(secondSmallest, c.Width);
        widthToAdd = secondSmallest - smallest
      }
    }

    widthToAdd = Math.Min(widthToAdd, remainingWidth / growable.Count);

    foreach (var c in growable.ToList())
    {
      var previousWidth = c.Width;
      if (c.Width == smallest)
      {
        c.Width += widthToAdd;
        if (c.Width >= c.MaxWidth)
        {
          c.Width = c.MaxWidth;
          shrinkable.Remove(c);
        }
        remainingWidth -= (c.Width - previuousWidth);
      }
    }
  }

  // Shrink
  while (remainingWidth < 0)
  {
    var largest = shrinkable[0].Width;
    var secondLargest = 0.0f;
    var widthToAdd = remainingWidth;

    foreach (var c in shrinkable)
    {
      if (c.Width > largest)
      {
        secondLargest = largest;
        largest = c.Width;
      }
      if (c.Width < largest)
      {
        secondLargest = Math.Max(secondLargest, c.Width);
        widthToAdd = secondLargest - largest
      }
    }

    widthToAdd = Math.Max(widthToAdd, remainingWidth / shrinkable.Count);

    foreach (var c in shrinkable.ToList()) // Or filter out non shrinkable after end of foreach
    {
      var previousWidth = c.Width;
      if (c.Width == largest)
      {
        c.Width += widthToAdd;
        if (c.Width <= c.MinWidth)
        {
          c.Width = c.MinWidth;
          shrinkable.Remove(c);
        }
        remainingWidth -= (c.Width - previousWidth);
      }
    }
  }
}
```