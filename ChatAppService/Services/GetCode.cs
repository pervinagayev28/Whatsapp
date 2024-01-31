using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ChatAppService.Services;

namespace ForecastDesign.Statics.StaticClasses.GetSmtpCode
{
    public static class GetCode
    {
        public static async Task<string> GmailVerify(string? gmail)
        {
            //----------- SMTP ------------

            int VerifyCode = Random.Shared.Next(111111, 999999);
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(Configuration.GetValue("Smtp", "Gmail"));
            string sbj = "VERIFY CODE";
            msg.Subject = sbj;
            msg.To.Add(new MailAddress(gmail));
            msg.Body = $@"
        <html>
        <body style='text-align: center; font-family: Arial, sans-serif;'>
            <div style='background-color: #f4f4f4; padding: 20px; border-radius: 10px; width: 300px; margin: 0 auto;'>
                <img src='https://nordicapis.com/wp-content/uploads/6-Best-Free-and-Paid-Weather-APIs.png' alt='Logo' style='max-width: 100%; height: auto; margin-bottom: 15px;'/>
                <h2 style='color: #333;'>Gmail Verification Code</h2>
                <p style='color: #555;'>Hello,</p>
                <p style='color: #555; font-size: 24px; font-weight: bold;'>Your Verification Code: <span style='color: #e44d26; font-size: 36px;'>{VerifyCode}</span></p>
                <p style='color: #555;'>Please use this code to verify your account.</p>
                <p style='color: #555;'>Thank you.</p>
            </div>
        </body>
        </html>
    ";
            msg.IsBodyHtml = true;
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("pervinagayev28@gmail.com", Configuration.GetValue("Smtp", "Password")),
                EnableSsl = true
            };
            smtpClient.Send(msg);
            return VerifyCode.ToString();
        }
    }
}
