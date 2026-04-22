public class Program
{
    public static async Task Main(string[] args)
    {
        var host = CreateDefaultApp(args).Build();
        await host.RunAsync();
    }

    private static IHostBuilder CreateDefaultApp(string[] args) => Host
        .CreateDefaultBuilder()
        .ConfigureHostConfiguration(config =>
        {

        })
        .ConfigureServices(services =>
        {
            services
                .AddHostedService<UserInterfaceWorkerBackgroundService>()
                .AddEngine()
                .AddUserInterface()
                .AddLocalization(new Dictionary<string, string>
                {
                    { "WINDOW_TITLE", "Idology User Interface Client" }
                })
                .RegisterScene<StartScene>();
        })
        .ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddConsole();
        });
}