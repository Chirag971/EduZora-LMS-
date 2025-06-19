namespace eduzora_lms.Services
{
    public class SmtpOptions
    {
        // 1. Create a SMTP OPT Model
        public string Host { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
    }
}
