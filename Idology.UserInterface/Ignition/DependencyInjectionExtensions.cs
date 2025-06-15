namespace Idology.UserInterface.Ignition;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddUserInterface(this IServiceCollection services)
    {
        services.AddKeyedScoped<IUserInterfaceTheme, DefaultUserInterfaceTheme>(nameof(DefaultUserInterfaceTheme));
        return services;
    }
}
