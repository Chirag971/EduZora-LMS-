namespace eduzora_lms.Services
{
    public interface IEmailSender
    {
        // 2. Declare A Method
        Task sendEmailAsync(string FromAddress, string ToAddress, string Subject, string Message,
            bool isBodyHtml);
    }
}
