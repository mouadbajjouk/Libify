using Libify.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Libify.Services
{
    public class EmailService : IEmailService
    {
        private const string templatePath = "EmailTemplate/{0}.html";
        private readonly SMTPConfigModel _smtpConfig;

        public EmailService(IOptions<SMTPConfigModel> smtpConfig)
        {
            _smtpConfig = smtpConfig.Value;
        }

        private async Task SendEmail(SendEmailOptions options)
        {
            MailMessage mail = new MailMessage()  // create mail
            {
                Subject = options.Subject,
                Body = options.Body,
                From = new MailAddress(_smtpConfig.SenderAddress, _smtpConfig.SenderDisplayName),
                IsBodyHtml = _smtpConfig.IsBodyHTML
            };

            foreach (var toEmail in options.ToEmails) // select where to send
            {
                mail.To.Add(toEmail);
            }

            NetworkCredential networkCredential = new NetworkCredential(_smtpConfig.UserName, _smtpConfig.Password); // retreive mailtrap.io credentials from JSON file

            SmtpClient smtpClient = new SmtpClient() // setup for sneding the email
            {
                Host = _smtpConfig.Host,
                Port = _smtpConfig.Port,
                EnableSsl = _smtpConfig.EnableSSL,
                UseDefaultCredentials = _smtpConfig.UseDefaultCredentials,
                Credentials = networkCredential
            };

            mail.BodyEncoding = Encoding.Default; // set mail encoding

            await smtpClient.SendMailAsync(mail); // send the email after too much work
        }

        private string GetEmailBody(string templateName)
        {
            var body = File.ReadAllText(string.Format(templatePath, templateName)); // read the body of the email template

            return body;
        }

        private string UpdatePlaceHolder(string text, List<KeyValuePair<string, string>> keyValuePairs)
        {
            if (!string.IsNullOrEmpty(text) && keyValuePairs != null)
            {
                foreach (var placeHolder in keyValuePairs)
                {
                    if (text.Contains(placeHolder.Key))
                    {
                        text = text.Replace(placeHolder.Key, placeHolder.Value);
                    }
                }
            }

            return text;
        }

        public async Task SendForgotPasswordEmail(SendEmailOptions sendEmailOptions)
        {
            sendEmailOptions.Subject = UpdatePlaceHolder("Asalamo alaikom {{UserName}}, reset your password", sendEmailOptions.PlaceHolders);
            sendEmailOptions.Body = UpdatePlaceHolder(GetEmailBody("ForgotPassword"), sendEmailOptions.PlaceHolders);

            await SendEmail(sendEmailOptions);
        }

        public async Task SendEmailForEmailConfirmation(SendEmailOptions sendEmailOptions)
        {
            sendEmailOptions.Subject = UpdatePlaceHolder("Asalamo alaikom {{UserName}}, confirm your email", sendEmailOptions.PlaceHolders);
            sendEmailOptions.Body = UpdatePlaceHolder(GetEmailBody("EmailConfirm"), sendEmailOptions.PlaceHolders);

            await SendEmail(sendEmailOptions);
        }


    }
}
