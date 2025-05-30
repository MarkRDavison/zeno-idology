namespace Idology.UserInterface.Elements;

public sealed class Sizing
{
    public Sizing(SizingType type, float min, float max)
    {
        Type = type;
        Min = min;
        Max = max;
    }

    public static Sizing Grow(float min = float.MinValue, float max = float.MaxValue)
    {
        return new Sizing(SizingType.Grow, min, max);
    }

    public static Sizing Fit(float min = float.MinValue, float max = float.MaxValue)
    {
        return new Sizing(SizingType.Fit, min, max);
    }

    public static Sizing Fixed(float size)
    {
        return new Sizing(SizingType.Fixed, size, size);
    }

    public SizingType Type { get; }
    public float Min { get; }
    public float Max { get; }

}
