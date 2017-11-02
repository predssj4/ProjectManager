using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Domain.Tools
{
    public class SMTPClient
    {
       protected SmtpClient smtpClient;

       public bool sendMessage(MailMessage message)
       {
           try
           {
               smtpClient.Send(message);
           }
           catch (SmtpException e)
           {
               // Temporary not doing any action, just return false witch means sending failure
               return false;
           }

           return true;
       }
    }
}
