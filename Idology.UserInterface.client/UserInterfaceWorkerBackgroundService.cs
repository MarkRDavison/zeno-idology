namespace Idology.UserInterface.Client;

internal sealed class UserInterfaceWorkerBackgroundService : IdologyWorkerBackgroundService
{
    public UserInterfaceWorkerBackgroundService(
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
            .SetScene<StartScene>(null);
    }

    protected override void RegisterActions(IInputManager inputManager)
    {
        inputManager.RegisterAction(new()
        {
            Name = "LCLICK_DOWN",
            Type = InputActionType.MOUSE,
            State = InputActionState.PRESS,
            Button = MouseButton.Left
        });
        inputManager.RegisterAction(new()
        {
            Name = "LCLICK_UP",
            Type = InputActionType.MOUSE,
            State = InputActionState.RELEASE,
            Button = MouseButton.Left
        });
    }
}
