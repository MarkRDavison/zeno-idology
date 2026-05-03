namespace Idology.Engine.Infrastructure;

[Flags]
public enum InputActionState
{
    NONE = 0,
    PRESS = 1 << 0,
    HOLD = 1 << 1,
    RELEASE = 1 << 2,
    DOUBLE = 1 << 3,

    DOWN = PRESS | HOLD
}
