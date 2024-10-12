using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Services.Dtos.EmailDtos;
using Services.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.EmailServices
{
    public class EmailServices : IEmailServices
    {
        private readonly Emails _email;

        public EmailServices(IOptions<Emails>Email )
        {
            _email = Email.Value;

        }


        public async  Task SendEmail(EmailDto dto)
        {

            var email = new MimeMessage();
            email.Sender=MailboxAddress.Parse(_email.Email);
            email.To.Add(MailboxAddress.Parse(dto.ToEmail));
            email.Subject=dto.Subject;
            var builder = new BodyBuilder();
            builder.HtmlBody= dto.Body;
            email.Body=builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(_email.Host, _email.Port,SecureSocketOptions.StartTls);
            smtp.Authenticate(_email.Email,_email.Password);
            await smtp.SendAsync(email);

            smtp.Disconnect(true);

        }
    }
}
