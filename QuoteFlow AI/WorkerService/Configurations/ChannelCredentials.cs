namespace WorkerService.Configurations
{
    public class ChannelCredentials
    {
        public WhatsAppCreds WhatsApp { get; set; } = new();
        public EmailCreds Email { get; set; } = new();
    }

    public class WhatsAppCreds
    {
        public string PhoneNumberId { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;
        public string ApiBaseUrl { get; set; } = string.Empty;
    }

    public class EmailCreds
    {
        public string SmtpServer { get; set; } = string.Empty;
        public int SmtpPort { get; set; }
        public string SenderEmail { get; set; } = string.Empty;
        public string SenderName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool EnableSsl { get; set; }
    }
}