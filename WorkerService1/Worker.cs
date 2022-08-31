
using Serilog;

namespace WorkerService1
{
    public class Worker : BackgroundService
    {
        private readonly WorkerOptions _options;
        private readonly IBackupService _backupService;

        public Worker(WorkerOptions options, IBackupService backupService)
        {
            _options = options;
            _backupService = backupService;

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _backupService.Backup();
                await Task.Delay(_options.Interval * 1000, stoppingToken);

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