namespace Idology.Conservation.Client;

internal sealed class ConservationWorkerBackgroundService : IdologyWorkerBackgroundService
{
    public ConservationWorkerBackgroundService(
        IHostApplicationLifetime hostApplicationLifetime,
        IServiceScopeFactory serviceScopeFactory
    ) : base(
        hostApplicationLifetime,
        serviceScopeFactory)
    {
    }

    public override string WindowTitleTranslationKey => "WINDOW_TITLE";

    protected override void BeforeStartInitialize(IServiceProvider serviceProvider)
    {
        serviceProvider
            .GetRequiredService<ISceneService>()
            .SetScene<ConservationTitleScene>(null);
    }

    protected override void RegisterActions(IInputManager inputManager)
    {
        inputManager.RegisterAction(new()
        {
            Name = Constants.Action_Click_Start,
            Type = InputActionType.MOUSE,
            State = InputActionState.PRESS,
            Button = MouseButton.Left
        });

        inputManager.RegisterAction(new()
        {
            Name = Constants.Action_Pan,
            Type = InputActionType.MOUSE,
            State = InputActionState.DOWN,
            Button = MouseButton.Left
        });

        inputManager.RegisterAction(new()
        {
            Name = Constants.Action_Click,
            Type = InputActionType.MOUSE,
            State = InputActionState.RELEASE,
            Button = MouseButton.Left
        });

        inputManager.RegisterAction(new()
        {
            Name = Constants.Action_Click_Context,
            Type = InputActionType.MOUSE,
            State = InputActionState.RELEASE,
            Button = MouseButton.Right
        });

        inputManager.RegisterAction(new()
        {
            Name = Constants.Action_Escape,
            Type = InputActionType.KEYBOARD,
            State = InputActionState.RELEASE,
            Key = KeyboardKey.Escape
        });

        inputManager.RegisterAction(new()
        {
            Name = Constants.Action_Enter,
            Type = InputActionType.KEYBOARD,
            State = InputActionState.RELEASE,
            Key = KeyboardKey.Enter
        });

        inputManager.RegisterAction(new()
        {
            Name = Constants.Action_CycleRegion,
            Type = InputActionType.KEYBOARD,
            State = InputActionState.RELEASE,
            Key = KeyboardKey.Tab
        });

        inputManager.RegisterAction(new()
        {
            Name = Constants.Action_Shortcut_Kakapo,
            Type = InputActionType.KEYBOARD,
            State = InputActionState.RELEASE,
            Key = KeyboardKey.F1
        });

        inputManager.RegisterAction(new()
        {
            Name = Constants.Action_Shortcut_Staff,
            Type = InputActionType.KEYBOARD,
            State = InputActionState.RELEASE,
            Key = KeyboardKey.F2
        });
    }

    protected override void LoadFonts(IFontManager fontManager)
    {
        fontManager.LoadFont("DEBUG", "Assets/Fonts/Kenney-Mini.ttf");
        fontManager.LoadFont("CALIBRIB", "Assets/Fonts/calibrib.ttf");
    }
}
