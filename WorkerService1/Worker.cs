namespace WorkerService1
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly WorkerOptions _options;

        public Worker(ILogger<Worker> logger, WorkerOptions options)
        {
            _logger = logger;
            _options = options;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int index = 0;
            int errCounter = 0;
            while (!stoppingToken.IsCancellationRequested)
            {
                foreach (string path in _options.Files)
                {
                    try
                    {
                        _logger.LogInformation("Worker running at: {time} and {dir} saving to the {backupDir}", DateTimeOffset.Now, path, _options.BackupDirectory);
                        index++;
                        string ext = Path.GetExtension(path);
                        File.Copy(path, Path.Combine(_options.BackupDirectory + Path.GetFileNameWithoutExtension(path) + "-" + index + "-" + DateTimeOffset.Now.Year + "-" + DateTimeOffset.Now.Month + "-" + DateTimeOffset.Now.Day + "-" + DateTimeOffset.Now.Hour + "-" + DateTimeOffset.Now.Minute + "-" + DateTimeOffset.Now.Second + ext), true);

                    }
                    catch (FileNotFoundException e)
                    {
                        Console.WriteLine(e.Message);
                        errCounter++;
                    }

                }
                index = 0;
                if (errCounter == _options.Files.Length)
                {
                    Console.WriteLine("Zadané soubory neexistují");
                    break;
                }
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