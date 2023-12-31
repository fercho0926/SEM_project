﻿using SEM_project.Models;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;


namespace SEM_project.Services
{
    public class MailService : IMailService
    {
        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            var password = "zwkq zvdg nhwg sqkq";
            var userC = "sem.project.tdea@gmail.com";
            var smttp = "smtp.gmail.com";
            var port = "587";

            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(userC);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;
            var builder = new BodyBuilder();
            if (mailRequest.Attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in mailRequest.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }

                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }

            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(smttp, int.Parse(port), SecureSocketOptions.StartTls);
            smtp.Authenticate(userC, password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}