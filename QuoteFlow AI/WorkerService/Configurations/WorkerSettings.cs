namespace WorkerService.Configurations
{
    public class WorkerSettings
    {
        public int CheckIntervalSeconds { get; set; } = 60;
        public int RetryCount { get; set; } = 3;
        public int RetryDelaySeconds { get; set; } = 5;
        public bool EnableWhatsApp { get; set; } = true;
        public bool EnableEmail { get; set; } = true;
        public string DefaultChannel { get; set; } = "WhatsApp";
        public string ApiBaseUrl { get; set; } = string.Empty;
    }
}