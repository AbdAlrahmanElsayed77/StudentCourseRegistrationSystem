using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.AspNetCore.Identity.UI.Services;
using StudentCourseRegistration.Services.Interfaces;

namespace StudentCourseRegistration.Services.Implementations
{
    public class EmailService : IEmailSender, IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress(
                    _configuration["EmailSettings:SenderName"],
                    _configuration["EmailSettings:SenderEmail"]));
                emailMessage.To.Add(new MailboxAddress("", email));
                emailMessage.Subject = subject;

                var bodyBuilder = new BodyBuilder { HtmlBody = message };
                emailMessage.Body = bodyBuilder.ToMessageBody();

                using var client = new SmtpClient();
                await client.ConnectAsync(
                    _configuration["EmailSettings:SmtpServer"],
                    int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "587"),
                    MailKit.Security.SecureSocketOptions.StartTls);

                await client.AuthenticateAsync(
                    _configuration["EmailSettings:Username"],
                    _configuration["EmailSettings:Password"]);

                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);

                _logger.LogInformation($"Email sent successfully to {email}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to send email to {email}: {ex.Message}");
                // Don't throw exception - email failure shouldn't break the app
            }
        }

        public async Task SendEmailConfirmationAsync(string email, string confirmationLink)
        {
            var subject = "Confirm your email - Student Registration System";
            var message = $@"
                <!DOCTYPE html>
                <html dir='ltr' lang='en'>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            background-color: #f4f4f4;
                            margin: 0;
                            padding: 0;
                        }}
                        .container {{
                            max-width: 600px;
                            margin: 20px auto;
                            background-color: #ffffff;
                            border-radius: 8px;
                            overflow: hidden;
                            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
                        }}
                        .header {{
                            background-color: #1b6ec2;
                            color: white;
                            padding: 30px;
                            text-align: center;
                        }}
                        .content {{
                            padding: 30px;
                        }}
                        .button {{
                            display: inline-block;
                            background-color: #1b6ec2;
                            color: white;
                            padding: 12px 30px;
                            text-decoration: none;
                            border-radius: 5px;
                            margin: 20px 0;
                        }}
                        .footer {{
                            background-color: #f8f9fa;
                            padding: 20px;
                            text-align: center;
                            color: #666;
                            font-size: 14px;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>Welcome to Student Registration System</h1>
                        </div>
                        <div class='content'>
                            <h2>Confirm Your Email Address</h2>
                            <p>Thank you for registering with us! To complete your registration, please confirm your email address by clicking the button below:</p>
                            <div style='text-align: center;'>
                                <a href='{confirmationLink}' class='button'>Confirm Email Address</a>
                            </div>
                            <p>If the button doesn't work, copy and paste this link into your browser:</p>
                            <p style='word-break: break-all; color: #1b6ec2;'>{confirmationLink}</p>
                            <p style='color: #666; font-size: 14px; margin-top: 30px;'>
                                If you didn't create this account, please ignore this email.
                            </p>
                        </div>
                        <div class='footer'>
                            <p>&copy; 2024 Student Course Registration System. All rights reserved.</p>
                        </div>
                    </div>
                </body>
                </html>
            ";

            await SendEmailAsync(email, subject, message);
        }

        public async Task SendPasswordResetAsync(string email, string resetLink)
        {
            var subject = "Reset your password - Student Registration System";
            var message = $@"
                <!DOCTYPE html>
                <html dir='ltr' lang='en'>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            background-color: #f4f4f4;
                            margin: 0;
                            padding: 0;
                        }}
                        .container {{
                            max-width: 600px;
                            margin: 20px auto;
                            background-color: #ffffff;
                            border-radius: 8px;
                            overflow: hidden;
                            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
                        }}
                        .header {{
                            background-color: #dc3545;
                            color: white;
                            padding: 30px;
                            text-align: center;
                        }}
                        .content {{
                            padding: 30px;
                        }}
                        .button {{
                            display: inline-block;
                            background-color: #dc3545;
                            color: white;
                            padding: 12px 30px;
                            text-decoration: none;
                            border-radius: 5px;
                            margin: 20px 0;
                        }}
                        .footer {{
                            background-color: #f8f9fa;
                            padding: 20px;
                            text-align: center;
                            color: #666;
                            font-size: 14px;
                        }}
                        .warning {{
                            background-color: #fff3cd;
                            border: 1px solid #ffc107;
                            padding: 15px;
                            border-radius: 5px;
                            margin: 20px 0;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>Password Reset Request</h1>
                        </div>
                        <div class='content'>
                            <h2>Reset Your Password</h2>
                            <p>We received a request to reset your password. Click the button below to create a new password:</p>
                            <div style='text-align: center;'>
                                <a href='{resetLink}' class='button'>Reset Password</a>
                            </div>
                            <p>If the button doesn't work, copy and paste this link into your browser:</p>
                            <p style='word-break: break-all; color: #dc3545;'>{resetLink}</p>
                            <div class='warning'>
                                <strong>⚠️ Security Notice:</strong>
                                <ul style='margin: 10px 0 0 0;'>
                                    <li>This link will expire in 24 hours</li>
                                    <li>If you didn't request this, please ignore this email</li>
                                    <li>Your password won't change until you create a new one</li>
                                </ul>
                            </div>
                        </div>
                        <div class='footer'>
                            <p>&copy; 2024 Student Course Registration System. All rights reserved.</p>
                        </div>
                    </div>
                </body>
                </html>
            ";

            await SendEmailAsync(email, subject, message);
        }
    }
}