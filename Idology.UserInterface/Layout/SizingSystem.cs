using Idology.UserInterface.Elements;

namespace Idology.UserInterface.Layout;

public static class SizingSystem
{
    public static UiElement OpenElement(UiElement? parent, UiElement element)
    {
        element.Parent = parent;

        element.MinWidth = element.WidthSizing.Min;
        element.MaxWidth = element.WidthSizing.Max;

        if (element.WidthSizing.Type == SizingType.Fixed)
        {
            element.Width = element.WidthSizing.Min;
        }

        element.MinHeight = element.HeightSizing.Min;
        element.MaxHeight = element.HeightSizing.Max;

        if (element.HeightSizing.Type == SizingType.Fixed)
        {
            element.Height = element.HeightSizing.Min;
        }

        return element;
    }

    // TODO: Make independant of width/height and be along/against layout axis
    public static void CloseElement(UiElement element, bool xAxis)
    {
        if (element.Parent is { } parent)
        {
            if (xAxis)
            {
                element.Width += element.Padding.Left + element.Padding.Right;
            }
            else
            {
                element.Height += element.Padding.Top + element.Padding.Bottom;
            }

            var childGap = (parent.Children.Count - 1) * parent.ChildGap;
            if (parent.LayoutDirection == LayoutDirection.LeftToRight)
            {

                if (xAxis)
                {
                    element.Width += childGap;
                    parent.Width += element.Width;
                    parent.MinWidth += element.MinWidth;
                }
                else
                {
                    parent.Height = Math.Max(element.Height, parent.Height);
                    parent.MinHeight += Math.Max(element.MinHeight, parent.MinHeight);
                }
            }
            else
            {
                if (xAxis)
                {
                    parent.Width = Math.Max(element.Width, parent.Width);
                    parent.MinWidth += Math.Max(element.MinWidth, parent.MinWidth);
                }
                else
                {
                    element.Height += childGap;
                    parent.Height += element.Height;
                    parent.MinHeight += element.MinHeight;
                }
            }
        }
        else
        {
            if (xAxis)
            {
                if (element.WidthSizing.Type == SizingType.Fixed)
                {
                    element.Width = element.WidthSizing.Min;
                }
            }
            else
            {
                if (element.HeightSizing.Type == SizingType.Fixed)
                {
                    element.Height = element.HeightSizing.Min;
                }
            }
        }
    }

    // TODO: Make independant of width/height and be along/against layout axis
    public static void GrowAndShrinkChildElements(UiElement parent, bool xAxis, IList<UiElement> growable, IList<UiElement> shrinkable)
    {
        if (parent.LayoutDirection == LayoutDirection.LeftToRight)
        {
            {
                var remainingWidth = parent.Width;

                remainingWidth -= parent.Padding.Left + parent.Padding.Right;

                remainingWidth -= parent.Children.Sum(_ => _.Width);
                remainingWidth -= (parent.Children.Count - 1) * parent.ChildGap;

                // Grow
                while (remainingWidth > 0 && growable.Count > 0)
                {
                    var smallest = growable.First().Width;
                    var secondSmallest = float.MaxValue;
                    var widthToAdd = remainingWidth;

                    foreach (var g in growable)
                    {
                        if (g.Width < smallest)
                        {
                            secondSmallest = smallest;
                            smallest = g.Width;
                        }

                        if (g.Width > smallest)
                        {
                            secondSmallest = Math.Min(secondSmallest, g.Width);
                            widthToAdd = secondSmallest - smallest;
                        }
                    }

                    widthToAdd = Math.Min(widthToAdd, remainingWidth / growable.Count);

                    foreach (var g in growable)
                    {
                        var previousWidth = g.Width;
                        if (g.Width == smallest)
                        {
                            g.Width += widthToAdd;

                            if (g.Width >= g.MaxWidth)
                            {
                                g.Width = g.MaxWidth;
                                growable.Remove(g);
                            }

                            remainingWidth -= (g.Width - previousWidth);
                        }
                    }
                }

                // Shrink
                while (remainingWidth < 0 && shrinkable.Count > 0)
                {
                    var largest = shrinkable.First().Width;
                    var secondLargest = 0.0f;
                    var widthToAdd = remainingWidth;

                    foreach (var s in shrinkable)
                    {
                        if (s.Width > largest)
                        {
                            secondLargest = largest;
                            largest = s.Width;
                        }

                        if (s.Width < largest)
                        {
                            secondLargest = Math.Max(secondLargest, s.Width);
                            widthToAdd = secondLargest - largest;
                        }
                    }

                    widthToAdd = Math.Max(widthToAdd, remainingWidth / shrinkable.Count);

                    foreach (var s in shrinkable)
                    {
                        var previousWidth = s.Width;

                        if (s.Width == largest)
                        {
                            s.Width += widthToAdd;

                            if (s.Width <= s.MinWidth)
                            {
                                s.Width = s.MinWidth;
                                shrinkable.Remove(s);
                            }

                            remainingWidth -= (s.Width - previousWidth);
                        }
                    }
                }
            }
            {
                var height = parent.Height - parent.Padding.Top - parent.Padding.Bottom;

                // Grow
                foreach (var g in growable)
                {
                    g.Height = Math.Max(g.MinHeight, Math.Min(g.MaxHeight, height - g.Padding.Top - g.Padding.Bottom));
                }

                // Shrink
            }
        }
        else
        {
            var remainingHeight = parent.Height;

            remainingHeight -= parent.Padding.Top + parent.Padding.Bottom;

            remainingHeight -= parent.Children.Sum(_ => _.Height);
            remainingHeight -= (parent.Children.Count - 1) * parent.ChildGap;

            // Grow
            while (remainingHeight > 0 && growable.Count > 0)
            {
                var smallest = growable.First().Height;
                var secondSmallest = float.MaxValue;
                var heightToAdd = remainingHeight;

                foreach (var g in growable)
                {
                    if (g.Height < smallest)
                    {
                        secondSmallest = smallest;
                        smallest = g.Height;
                    }

                    if (g.Height > smallest)
                    {
                        secondSmallest = Math.Min(secondSmallest, g.Height);
                        heightToAdd = secondSmallest - smallest;
                    }
                }

                heightToAdd = Math.Min(heightToAdd, remainingHeight / growable.Count);

                foreach (var g in growable)
                {
                    var previousHeight = g.Height;
                    if (g.Height == smallest)
                    {
                        g.Height += heightToAdd;

                        if (g.Height >= g.MaxHeight)
                        {
                            g.Height = g.MaxHeight;
                            shrinkable.Remove(g);
                        }

                        remainingHeight -= (g.Height - previousHeight);
                    }
                }
            }

            // Shrink
            while (remainingHeight < 0 && shrinkable.Count > 0)
            {
                var largest = shrinkable.First().Height;
                var secondLargest = 0.0f;
                var heightToAdd = remainingHeight;

                foreach (var s in shrinkable)
                {
                    if (s.Height > largest)
                    {
                        secondLargest = largest;
                        largest = s.Height;
                    }

                    if (s.Height < largest)
                    {
                        secondLargest = Math.Max(secondLargest, s.Height);
                        heightToAdd = secondLargest - largest;
                    }
                }

                heightToAdd = Math.Max(heightToAdd, remainingHeight / shrinkable.Count);

                foreach (var s in shrinkable)
                {
                    var previousHeight = s.Height;

                    if (s.Height == largest)
                    {
                        s.Height += heightToAdd;

                        if (s.Height <= s.MinHeight)
                        {
                            s.Height = s.MinHeight;
                            shrinkable.Remove(s);
                        }

                        remainingHeight -= (s.Height - previousHeight);
                    }
                }
            }
        }
    }
}
