using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using config = PK.SimpleCraigslistListener.BLL.Properties.application;
using PK.SimpleCraigslistListener.BLL;

namespace PK.SimpleCraigslistListener.Utilities
{
    public class Utilities
    {
        public static void SendMail(string subject, string body)
        {
            var fromAddress = new MailAddress(config.Default.smtpUser, config.Default.mailSubject);
            var toAddress = new MailAddress("phillip.knezevich@live.com");
            string fromPassword = config.Default.smtpPassword;
            

            var smtp = new SmtpClient
            {
                Host = config.Default.smtp,
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress) { Subject = subject, Body = body, IsBodyHtml = true })
            {
                smtp.Send(message);
            }
        }

        public static void SendRssManifest(List<RssFeedItem> feed)
        {
            StringBuilder sb = new StringBuilder();
            foreach (RssFeedItem f in feed)
            {
                string body = "Results for Craigslist RSS<br><br> <a href=\"" + f.Link + "\">" + f.Title + "</a><br>" + f.Description + "";
                sb.AppendLine(body);
            }
            SendMail("Craigslist RSS Results", sb.ToString());
        }
    }
}