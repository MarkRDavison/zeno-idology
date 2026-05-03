namespace Idology.Engine.Infrastructure;

internal sealed class ActionState
{
    public DateTime? LastActivated { get; set; }
}

internal sealed class InputManager : IInputManager
{
    private readonly TimeSpan _doubleThreshhold;
    private readonly HashSet<string> _handled = [];
    private readonly Dictionary<string, InputAction> _actions = [];
    private readonly Dictionary<string, ActionState> _actionStates = [];

    public InputManager()
    {
        _doubleThreshhold = TimeSpan.FromMilliseconds(250);
    }

    public bool IsActionInvoked(string name)
    {
        if (_handled.Contains(name))
        {
            return false;
        }

        return IsActionInvokedEvenIfHandled(name);
    }

    public bool IsActionInvokedEvenIfHandled(string name)
    {
        if (_actions.TryGetValue(name, out var action))
        {
            var state = _actionStates[name];

            if (action.Type is InputActionType.MOUSE)
            {
                var pressed = Raylib.IsMouseButtonPressed(action.Button);
                var held = Raylib.IsMouseButtonDown(action.Button);
                var released = Raylib.IsMouseButtonReleased(action.Button);

                if (action.State.HasFlag(InputActionState.HOLD) && held)
                {
                    return true;
                }

                if (action.State.HasFlag(InputActionState.PRESS) && pressed)
                {
                    return HandlePotentialDoubleActivation(action, state);
                }

                if (action.State.HasFlag(InputActionState.RELEASE) && released)
                {
                    return HandlePotentialDoubleActivation(action, state);
                }
            }
            else if (action.Type is InputActionType.KEYBOARD)
            {
                var held = Raylib.IsKeyDown(action.Key);
                var pressed = Raylib.IsKeyPressed(action.Key);
                var released = Raylib.IsKeyReleased(action.Key);

                if (action.State.HasFlag(InputActionState.HOLD) && held)
                {
                    return true;
                }

                if (action.State.HasFlag(InputActionState.PRESS) && pressed)
                {
                    return HandlePotentialDoubleActivation(action, state);
                }

                if (action.State.HasFlag(InputActionState.RELEASE) && released)
                {
                    return HandlePotentialDoubleActivation(action, state);
                }
            }
        }

        return false;
    }

    public bool HandleActionIfInvoked(string name)
    {
        if (IsActionInvoked(name))
        {
            MarkActionAsHandled(name);
            return true;
        }

        return false;
    }

    public void MarkActionAsHandled(string name)
    {
        _handled.Add(name);
    }

    public void RegisterAction(InputAction action)
    {
        _actions.Add(action.Name, action);
        _actionStates.Add(action.Name, new ActionState());
    }

    public void Update()
    {
        _handled.Clear();
    }

    public float GetWheelDelta() => Raylib.GetMouseWheelMove();
    public Vector2 GetMousePosition() => Raylib.GetMousePosition();
    public Vector2 GetMousePosition(Camera2D camera) => Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), camera);
    public Vector2 GetScreenSize() => new Vector2(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());

    private bool HandlePotentialDoubleActivation(InputAction action, ActionState state)
    {
        if (action.State.HasFlag(InputActionState.DOUBLE))
        {
            var lastActivated = state.LastActivated;
            if (lastActivated is not null)
            {
                var timespan = DateTime.Now - lastActivated;

                if (timespan < _doubleThreshhold)
                {
                    state.LastActivated = null;
                    return true;
                }
                else
                {
                    state.LastActivated = DateTime.Now;
                    return false;
                }
            }
            else
            {
                state.LastActivated = DateTime.Now;
                return false;
            }
        }

        return true;
    }
}
