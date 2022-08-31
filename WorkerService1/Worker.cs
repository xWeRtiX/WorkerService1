
namespace WorkerService1
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly WorkerOptions _options;
        private readonly IBackupService _backupService;

        public Worker(ILogger<Worker> logger, WorkerOptions options, IBackupService backupService)
        {
            _logger = logger;
            _options = options;
            _backupService = backupService;

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _backupService.Backup();
                int err = 0;
                foreach (string file in _options.Files)
                {
                    if (!File.Exists(file)) err++;

                }
                if (err == _options.Files.Length)
                {
                    _logger.LogInformation("Neexistující soubory");
                    break;
                }
                else await Task.Delay(_options.Interval * 1000, stoppingToken);
                err = 0;

            }
        }

    }
    public class WorkerOptions
    {
        public string[] Files { get; set; }
        public string BackupDirectory { get; set; }
        public int Interval { get; set; }
    }
}