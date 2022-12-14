using Serilog;
namespace WorkerService1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                IConfiguration configuration = hostContext.Configuration;

                WorkerOptions options = configuration.GetSection("Directories").Get<WorkerOptions>();

                services.AddSingleton(options);
                services.AddSingleton<IBackupService, BackupService>();

                services.AddHostedService<Worker>();
            }).UseSerilog((cfg, lc) =>
            {
                WorkerOptions config = cfg.Configuration.GetSection("Directories").Get<WorkerOptions>();
            lc.WriteTo.Console().WriteTo.File(Path.Combine(config.BackupDirectory + "/logs/"), rollingInterval: RollingInterval.Day);
            });
    }
}