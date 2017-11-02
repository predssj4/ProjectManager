using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace ProjectManager.Domain.Tools
{
    public class GmailClient : SMTPClient
    {
        NetworkCredential networkCredentials;

        public GmailClient()
        {
            networkCredentials = new NetworkCredential("adrian.rutkowsky@gmail.com", "xxx");

            smtpClient = new SmtpClient();
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = networkCredentials;
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.Port = 587;
        }
    }
}