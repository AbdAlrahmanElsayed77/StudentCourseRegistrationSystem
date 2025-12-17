using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.AspNetCore.Identity.UI.Services;
using StudentCourseRegistration.Services.Interfaces;

namespace StudentCourseRegistration.Services.Implementations
{
    // ✅ الآن EmailService بيطبق الاتنين: IEmailSender و IEmailService
    public class EmailService : IEmailSender, IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        // ✅ هذه الـ Method اللي Identity محتاجها
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
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                    <h2 style='color: #1b6ec2;'>Welcome to Student Course Registration System</h2>
                    <p>Please confirm your email by clicking the link below:</p>
                    <p style='margin: 20px 0;'>
                        <a href='{confirmationLink}' 
                           style='background-color: #1b6ec2; 
                                  color: white; 
                                  padding: 10px 20px; 
                                  text-decoration: none; 
                                  border-radius: 5px;
                                  display: inline-block;'>
                            Confirm Email
                        </a>
                    </p>
                    <p style='color: #666; font-size: 14px;'>
                        If you didn't create this account, please ignore this email.
                    </p>
                </div>
            ";

            await SendEmailAsync(email, subject, message);
        }

        public async Task SendPasswordResetAsync(string email, string resetLink)
        {
            var subject = "Reset your password - Student Registration System";
            var message = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                    <h2 style='color: #1b6ec2;'>Password Reset Request</h2>
                    <p>Click the link below to reset your password:</p>
                    <p style='margin: 20px 0;'>
                        <a href='{resetLink}' 
                           style='background-color: #1b6ec2; 
                                  color: white; 
                                  padding: 10px 20px; 
                                  text-decoration: none; 
                                  border-radius: 5px;
                                  display: inline-block;'>
                            Reset Password
                        </a>
                    </p>
                    <p style='color: #666; font-size: 14px;'>
                        If you didn't request this, please ignore this email.
                    </p>
                    <p style='color: #666; font-size: 14px;'>
                        This link will expire in 24 hours.
                    </p>
                </div>
            ";

            await SendEmailAsync(email, subject, message);
        }
    }
}