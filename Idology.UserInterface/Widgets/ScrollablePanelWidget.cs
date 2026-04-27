namespace Idology.UserInterface.Widgets;

public class ScrollablePanelWidget : PanelWidget
{
    private float _scrollOffset = 0.0f;

    public override void Update(float delta)
    {
        var isOverflowing = false;
        float overflowSize = 0.0f;
        float displaySize = Layout.Rect.Y + Layout.Rect.Height;
        float maxMovement = 32.0f; // TODO: Setting/config???

        if (Layout.LastChild is { } lc)
        {
            var endOfChild =
                lc.Rect.Y +
                Layout.LastChild.Rect.Height +
                Layout.RequestedPadding.Top +
                Layout.RequestedPadding.Bottom;

            overflowSize = endOfChild - displaySize;

            if (overflowSize > 0.0f)
            {
                isOverflowing = true;
            }
        }

        if (isOverflowing)
        {
            _scrollOffset -= Math.Sign(Raylib.GetMouseWheelMove()) * maxMovement;

            _scrollOffset = Math.Clamp(_scrollOffset, 0.0f, overflowSize);
        }
        else
        {
            _scrollOffset = 0.0f;
        }
    }

    public override void Draw()
    {
        DrawPanelSelf();

        Raylib.BeginScissorMode(
            (int)(Layout.Rect.X),
            (int)(Layout.Rect.Y + Layout.RequestedPadding.Top),
            (int)(Layout.Rect.Width),
            (int)(Layout.Rect.Height - Layout.RequestedPadding.Top - Layout.RequestedPadding.Bottom));

        if (_scrollOffset != 0.0f)
        {
            // Recursively offset children by _scroll offset
            ForEachChildRecursively(c =>
            {
                c.Layout.Rect.Y -= _scrollOffset;
            });
        }

        DrawChildren();

        if (_scrollOffset != 0.0f)
        {
            // Recursively restore offset children by _scroll offset
            ForEachChildRecursively(c =>
            {
                c.Layout.Rect.Y += _scrollOffset;
            });
        }

        Raylib.EndScissorMode();
    }
}
