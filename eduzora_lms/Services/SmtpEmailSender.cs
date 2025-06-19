using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace eduzora_lms.Services
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly IOptions<SmtpOptions> options;

        public SmtpEmailSender(IOptions<SmtpOptions> options)
        {
            this.options = options;
        }

        public async Task sendEmailAsync(string FromAddress, string ToAddress, string Subject, string Message, bool isBodyHtml)
        {

            // Set-Up Mail Message
            var mailMsg = new MailMessage(FromAddress, ToAddress, Subject, Message) 
            {
                IsBodyHtml = isBodyHtml
            };

            // Set-up Host and Port
            using (var client = new SmtpClient(options.Value.Host, options.Value.Port) 
            { 
                Credentials = new NetworkCredential(options.Value.UserName, options.Value.Password)    
            })
            {
                // System.Net.Mail Method for Sending Mail with msg
                await client.SendMailAsync(mailMsg);
            }
        }
    }
}
