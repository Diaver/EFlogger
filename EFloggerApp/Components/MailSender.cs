using System.Net;
using System.Net.Mail;

namespace EFloggerApp.Components
{
    public static class MailSender
    {
        private const string ServerSMTP = "mail.ef-logger.com";
        private const int PortSMTP = 26;
        private const string LoginSMTP = "for_resend@ef-logger.com";
        private const string PasswdSMTP = "}=S_;CuFoTl4";


        public static void SendEmail(string to, string copy, string from, string boby, string subject, int entityId, string entityType)
        {
            var client = new SmtpClient(ServerSMTP, PortSMTP)
            {
                Credentials = new NetworkCredential(LoginSMTP, PasswdSMTP)
            };

            client.EnableSsl = false;
            var message = new MailMessage(from, to, subject, boby);

            if(!string.IsNullOrEmpty(copy))
                message.CC.Add(copy);

            client.Send(message);
          
        }
    }
}
