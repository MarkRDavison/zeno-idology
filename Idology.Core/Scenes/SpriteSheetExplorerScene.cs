namespace Idology.Core.Scenes;

public abstract class SpriteSheetExplorerScene : Scene
{
    private readonly List<string> _itemNames = [];
    private readonly ISpriteSheetManager _spriteSheetManager;
    private readonly IFontManager _fontManager;

    private readonly List<Button> _buttons = [];

    protected SpriteSheetExplorerScene(
        ISpriteSheetManager spriteSheetManager,
        IFontManager fontManager)
    {
        _spriteSheetManager = spriteSheetManager;
        _fontManager = fontManager;
    }

    protected abstract string Name { get; }

    public int Index { get; set; }

    public override void Init()
    {
        _itemNames.AddRange(_spriteSheetManager.GetItemNames(Name));

        _buttons.Add(new Button
        {
            Label = "Previous",
            Bounds = new Rectangle(64, Raylib.GetScreenHeight() - 64 - 64, 192, 64),
            OnClick = () => Index = (_itemNames.Count + Index - 1) % _itemNames.Count
        });
        _buttons.Add(new Button
        {
            Label = "Next",
            Bounds = new Rectangle(Raylib.GetScreenWidth() - 192 - 64, Raylib.GetScreenHeight() - 64 - 64, 192, 64),
            OnClick = () => Index = (Index + 1) % _itemNames.Count
        });
    }

    public override void Update(float delta)
    {
        foreach (var c in _buttons)
        {
            c.Update(delta);
        }

        if (Raylib.IsKeyPressed(KeyboardKey.Left) || Raylib.IsKeyPressedRepeat(KeyboardKey.Left))
        {
            Index = (_itemNames.Count + Index - 1) % _itemNames.Count;
        }
        if (Raylib.IsKeyPressed(KeyboardKey.Right) || Raylib.IsKeyPressedRepeat(KeyboardKey.Right))
        {
            Index = (Index + 1) % _itemNames.Count;
        }
    }

    public override void Draw()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.SkyBlue);

        const int FontSize = 32;

        var (currentItem, texture) = _spriteSheetManager.GetItem(Name, _itemNames[Index]);

        var text = $"{currentItem.Name}     {Index + 1}/{_itemNames.Count}";

        var font = _fontManager.GetFont("CALIBRIB");
        var textBounds = Raylib.MeasureTextEx(font, text, FontSize, 1);
        var position = new Vector2(
            Raylib.GetScreenWidth() / 2,
            Raylib.GetScreenHeight() * 3 / 4)
            -
            new Vector2(textBounds.X, textBounds.Y) / 2;

        Raylib.DrawTextEx(font, text, position, FontSize, 1, Color.Black);

        var size = currentItem.Bounds.Size * 2;

        var spritePosition = new Vector2(
            Raylib.GetScreenWidth() / 2,
            Raylib.GetScreenHeight() / 2) - size / 2;

        var targetBounds = new Rectangle(spritePosition, size);

        Raylib.DrawRectangleLines((int)targetBounds.X, (int)targetBounds.Y, (int)targetBounds.Width, (int)targetBounds.Height, Color.Green);

        Raylib.DrawTexturePro(texture, currentItem.Bounds, new Rectangle(spritePosition, size), new Vector2(), 0.0f, Color.White);

        foreach (var c in _buttons)
        {
            c.Draw();
        }

        Raylib.EndDrawing();
    }
}
