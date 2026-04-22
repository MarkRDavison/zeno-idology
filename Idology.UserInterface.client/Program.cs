public class Program
{
    public static async Task Main(string[] args)
    {
        var host = CreateDefaultApp(args).Build();
        await host.RunAsync();
    }

    private static IHostBuilder CreateDefaultApp(string[] args) => Host
        .CreateDefaultBuilder()
        .ConfigureServices(services =>
        {
            services
                .AddHostedService<UserInterfaceWorkerBackgroundService>()
                .AddEngine()
                .AddUserInterface()
                .AddLocalization([])
                .RegisterScene<StartScene>();
        })
        .ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddConsole();
        });
}