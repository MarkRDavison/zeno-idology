namespace Idology.Engine.Ignition;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddEngine(this IServiceCollection services)
    {
        services.AddScoped<Application>();
        services.AddScoped<ISceneService, SceneService>();
        services.AddSingleton<IFontManager, FontManager>();
        services.AddSingleton<ITextureManager, TextureManager>();
        services.AddSingleton<ISpriteSheetManager, SpriteSheetManager>();

        services.AddTransient<RenderSystem>();

        return services;
    }

    public static IServiceCollection RegisterScene<TScene>(this IServiceCollection services) where TScene : Scene
    {
        services.AddTransient<TScene>();
        return services;
    }

}