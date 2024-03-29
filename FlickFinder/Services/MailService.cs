﻿using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace FlickFinder.Services
{
    public class MailService : IMailService
    {
        private readonly IConfiguration _config;

        public MailService(IConfiguration configuration)
        {
            _config = configuration;
        }

        public string EmailBody => @"<!DOCTYPE html>
        <html>
        <head>
            <title>Welcome to Bongo</title>
            <style>
                body {
                    font-family: Arial, sans-serif;
                    background-color: #f8f8f8;
                    text-align: center;
                    padding: 20px;
        }

        h1 {
            color: #0066cc;
            margin-top: 20px;
            margin-bottom: 30px;
        }

        table.container {
            width: 100%;
            max-width: 600px;
            margin: 0 auto;
            background: linear-gradient(45deg, rgba(0, 14, 255, 0.5158438375350141) 0%, rgba(201, 201, 201, 0.742734593837535) 50%, rgba(255, 255, 255, 0.927608543417367) 100%);
            padding: 20px;
            border-radius: 10px;
        }

        table.container td {
            padding: 10px;
            text-align: left;
        }
        .td-image{
        text-align: center;
}

        .welcome-logo {
            font-family: 'Arial Rounded MT', Tahoma, Geneva, Verdana, sans-serif;
            font-size: 38px;
            color: blue;
            letter-spacing: 4px;
            text-align: left;
        }

        .welcome-image {
            width: 150px;
            height: 150px;
            object-fit: cover;
            border-radius: 50%;
            border: 2px white solid;
        }

        p {
            color: #333;
            font-size: 18px;
            line-height: 1.5;
            margin-bottom: 20px;
        }

        .signature {
            font-weight: bold;
            margin-top: 50px;
        }

        .contact-us {
            color: #888;
            font-size: 14px;
            margin-top: 30px;
        }

        .contact-us a {
            color: #0066cc;
            text-decoration: none;
        }
    </style>
</head>
<body>
    <table class=""container""> 
        <tr>
            <td class=""td-image"">
                <img class=""welcome-image"" src=""https://media.tenor.com/D1UYqF1qdb4AAAAd/excited-nollywood.gif"" alt=""Excited Nollywood"" />
            </td>
        </tr>
        <tr>
            <td colspan=""2"">
                <p>Dear User,</p>
                <p>Thank you for joining Bongo, the web app that helps you create personalized timetables.</p>
                <p>With Bongo, you can easily organize your schedule, manage your classes, and stay on top of your academic commitments.</p>
                <p>We hope you find Bongo helpful in optimizing your time and maximizing your productivity.</p>
                <p class=""contact-us"">If you have any feedback or suggestions to help us improve your Bongo experience, <a href=""mailto:feedback@bongoapp.com"">please feel free to reach out to us</a>.</p>
                <p class=""signature"">The Bongo Team</p>
            </td>
        </tr>
    </table>
</body>
</html>";

        public async Task SendMailAsync(string emailTo, string subject, string type, Dictionary<string, string> emailOptions)
        {
            var mailHost = _config.GetValue<string>("MailConfig:EmailHost");
            var mailFrom = _config.GetValue<string>("MailConfig:EmailFrom");
            var mailPw = _config.GetValue<string>("MailConfig:EmailPassword");
            int mailPort = _config.GetValue<int>("MailConfig:EmailPort");

            string templatePath = $"Services/EmailTemplates/{type}.html";
            string emailBody = await File.ReadAllTextAsync(templatePath);
            emailBody = emailBody.Replace("{{username}}", emailOptions["username"]);
            emailBody = emailBody.Replace("{{link}}", emailOptions["link"] ?? "");
            
            var mail = new MimeMessage();
            mail.From.Add(MailboxAddress.Parse(mailFrom));
            mail.To.Add(MailboxAddress.Parse(emailTo));
            mail.Subject = subject;
            mail.Body = new TextPart(TextFormat.Html) { Text = emailBody };

            using (var client = new SmtpClient())
            {
                client.Connect(mailHost, mailPort, SecureSocketOptions.StartTls);
                client.Authenticate(mailFrom, mailPw);
                await client.SendAsync(mail);
                client.Dispose();
            }
        }
    }
}
