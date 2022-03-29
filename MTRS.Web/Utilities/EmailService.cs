using MailKit.Net.Smtp;
using MimeKit;
using MTRS.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTRS.Web.Utilities
{
    public class EmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly IUserService _userService;

        const string _emailTemplatesPath = "/emailtemplates/";

        public EmailService(EmailSettings emailSettings, IUserService userService)
        {
            _emailSettings = emailSettings;
            _userService = userService;
        }

        public bool Send(long userId, long managerId, bool isApproved, string subject, string template)
        {
            try
            {
                var user = _userService.GetById(userId);
                var manager = _userService.GetById(managerId);

                var body = System.IO.File.ReadAllText(template);

                body = body.Replace("##UserName", user.FirstName).Replace("##ManagerName", manager.FirstName + " " + manager.LastName)
                    .Replace("##Approved", isApproved ? "Approved" : "Rejected");

                MimeMessage message = new MimeMessage();

                MailboxAddress from = new MailboxAddress("Admin",
                _emailSettings.From);
                message.From.Add(from);

                MailboxAddress to = new MailboxAddress(user.FirstName + " " + user.LastName,
                user.Email);
                message.To.Add(to);

                message.Subject = subject;

                BodyBuilder bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = body;
                message.Body = bodyBuilder.ToMessageBody();

                SmtpClient client = new SmtpClient();
                client.SslProtocols = System.Security.Authentication.SslProtocols.Tls11;

                client.Connect(_emailSettings.Host, _emailSettings.Port, MailKit.Security.SecureSocketOptions.None);

                if (!string.IsNullOrEmpty(_emailSettings.Username) && !string.IsNullOrEmpty(_emailSettings.Password))
                {
                    client.Authenticate(_emailSettings.Username, _emailSettings.Password);
                }

                client.Send(message);
                client.Disconnect(true);
                client.Dispose();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
