using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace PositionTracking
{
    public class EmailSender
    {
        private readonly ILogger _logger;

        private readonly bool _ssl;
        private readonly int _port, _timeout;
        private readonly string _title, _address, _host, _username, _password;

        public EmailSender(IConfiguration config, ILogger<EmailSender> logger)
        {
            var smtp = config.GetSection("Smtp");

            _title = smtp.GetValue<string>("Title");
            _address = smtp.GetValue<string>("Address");
            _host = smtp.GetValue<string>("Host");
            _port = smtp.GetValue<int>("Port");
            _ssl = smtp.GetValue<bool>("Ssl");
            _username = smtp.GetValue<string>("Username");
            _password = smtp.GetValue<string>("Password");
            _timeout = smtp.GetValue<int>("TimeoutMs");

            _logger = logger;
        }

        public async Task<bool> SendAsync(string destination,string subject,string message)
        {
            MimeMessage msg;
            SmtpClient smtp = null;
            CancellationTokenSource tokenSource = null;

            try
            {
                msg = new MimeMessage();
                msg.From.Add(new MailboxAddress(_title,_address));
                msg.To.Add(MailboxAddress.Parse(destination));
                msg.Subject = subject;
                msg.Body = new BodyBuilder()
                {
                    TextBody = message   
                }.ToMessageBody();

                tokenSource = new CancellationTokenSource();
                tokenSource.CancelAfter(_timeout);

                smtp = new SmtpClient();
                await smtp.ConnectAsync(_host, _port, _ssl, tokenSource.Token);
                await smtp.AuthenticateAsync(_username, _password,tokenSource.Token);
                await smtp.SendAsync(msg, tokenSource.Token);
                await smtp.DisconnectAsync(true, tokenSource.Token);

                _logger.LogDebug($"Email message successfully sent to '{destination}'.");
            }
            catch (OperationCanceledException e)
            {
                _logger.LogWarning(e, $"Sending email to '{destination}' canceled");
                return false;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Sending email to '{destination}' failed");
                return false;
            }
            finally
            {
                smtp?.Dispose();
                tokenSource?.Dispose();
            }

            return true;
        }
    }
}
