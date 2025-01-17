using BlazorWebAppInteractive.Backend.Data.Models;
using BlazorWebAppInteractive.Backend.IServices;
using Microsoft.AspNetCore.Identity;
using System.Net.Mail;

namespace BlazorWebAppInteractive.Backend.Services
{

    public class EmailSender : IEmailSender
    {
        public async Task<IdentityResult> SendConfirmation(ApplicationUser user, string confirmLink)
        {
            // HTML email template with the confirmation link embedded
            string message = $@"
    <!DOCTYPE html>
    <html lang='en'>
    <head>
        <meta charset='UTF-8'>
        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
        <title>Confirm Your Account</title>
        <style>
            body {{
                font-family: Arial, sans-serif;
                background-color: #f4f4f4;
                margin: 0;
                padding: 0;
            }}
            .email-container {{
                max-width: 600px;
                margin: 20px auto;
                background-color: #ffffff;
                border: 1px solid #ddd;
                border-radius: 8px;
                overflow: hidden;
                box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
            }}
            .email-header {{
                background-color: #1e3c72;
                color: #ffffff;
                padding: 20px;
                text-align: center;
            }}
            .email-header h1 {{
                margin: 0;
                font-size: 24px;
            }}
            .email-body {{
                padding: 20px;
                color: #333333;
                line-height: 1.6;
            }}
            .email-body h2 {{
                margin-top: 0;
                color: #1e3c72;
            }}
            .email-footer {{
                text-align: center;
                padding: 10px;
                background-color: #f4f4f4;
                font-size: 12px;
                color: #888888;
            }}
            .confirm-button {{
                display: inline-block;
                margin: 20px 0;
                padding: 10px 20px;
                font-size: 16px;
                color: #ffffff;
                background-color: #1f365e;
                text-decoration: none;
                border-radius: 4px;
            }}
            .confirm-button:hover {{
                background-color: #1e3c72;
            }}
        </style>
    </head>
    <body>
        <div class='email-container'>
            <div class='email-header'>
                <h1>Confirm Your Account</h1>
            </div>
            <div class='email-body'>
                <h2>Hello {user.Firstname},</h2>
                <p>Thank you for creating an account with us! To complete your registration, please confirm your email address by clicking the button below.</p>
                <p>If you did not create this account, you can safely ignore this email.</p>
                <a href='{confirmLink}' class='confirm-button'>Confirm Your Account</a>
            </div>
            <div class='email-footer'>
                &copy; 2024 Your Company. All rights reserved.
            </div>
        </div>
    </body>
    </html>";

            return await Sender(user, "Email confirmation", message);
        }

        public async Task<IdentityResult> SendEventRemider(ApplicationUser user, string message)
        {
            return await Sender(user, "Your daily event reminder", message);
        }

        public async Task<IdentityResult> SendFamilyUpdated(ApplicationUser user, string message)
        {
            return await Sender(user, "Your family got updated", message);
        }

        public async Task<IdentityResult> SendResetPassword(ApplicationUser user, string message)
        {
            string htmlMessage = $@"
    <!DOCTYPE html>
    <html lang='en'>
    <head>
        <meta charset='UTF-8'>
        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
        <title>Reset Password</title>
        <style>
            body {{
                font-family: Arial, sans-serif;
                margin: 0;
                padding: 0;
                background-color: #f4f4f4;
                color: #333;
            }}
            .email-container {{
                max-width: 600px;
                margin: 20px auto;
                background-color: #ffffff;
                border-radius: 8px;
                box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
                overflow: hidden;
            }}
            .header {{
                background-color: #1e3c72;
                color: #ffffff;
                padding: 20px;
                text-align: center;
                font-size: 24px;
            }}
            .content {{
                padding: 20px;
            }}
            .content p {{
                font-size: 16px;
                line-height: 1.6;
            }}
            .button {{
                display: inline-block;
                margin: 20px 0;
                padding: 10px 20px;
                background-color: #ec64b3;
                color: #ffffff;
                text-decoration: none;
                border-radius: 5px;
                font-weight: bold;
            }}
            .footer {{
                text-align: center;
                padding: 20px;
                font-size: 12px;
                color: #888;
            }}
        </style>
    </head>
    <body>
        <div class='email-container'>
            <div class='header'>
                Reset Your Password
            </div>
            <div class='content'>
                <p>Hi {user.Firstname},</p>
                <p>We received a request to reset your password. Click the button below to set up a new password:</p>
                <a href='{message}' class='button'>Reset Password</a>
                <p>If you did not request a password reset, please ignore this email or contact our support team if you have any concerns.</p>
                <p>Thank you, <br> The [Company Name] Team</p>
            </div>
            <div class='footer'>
                © 2024 [Company Name]. All rights reserved.
            </div>
        </div>
    </body>
    </html>";

            return await Sender(user, "Reset Password", htmlMessage);
        }

        private async Task<IdentityResult> Sender(ApplicationUser user, string subject, string message, string link = "")
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("SmtpServer");
            mail.From = new MailAddress("BlazorWebAppInteractive@BlazorWebAppInteractive.com");
            mail.To.Add(user.Email ?? "idk");
            mail.Subject = subject;
            mail.Body = message + link;
            mail.IsBodyHtml = true;

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("username", "password"); //passwort nicht vergessen
            SmtpServer.EnableSsl = true;
            try
            {
                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "#####",
                    Description = "Email could not be send"
                });
            }
            finally
            {
                SmtpServer.Dispose();
            }

            return IdentityResult.Success;
        }
    }
}
