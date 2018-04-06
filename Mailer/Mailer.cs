using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Mailer
{
    public class Mailer
    {
        private SmtpClient client;
        private MailMessage message;
        private MailAddress getter;
        private MailAddress sender;
        private string subject;
        private string body;
        public Mailer()
        {
            client = new SmtpClient("smtp.gmail.com", 587);
            client.Credentials = new NetworkCredential("students.mine.suport@gmail.com", "qweqweqwestudent");
            message = new MailMessage();
            message.IsBodyHtml = true;
            sender = new MailAddress("students.mine.suport@gmail.com", "Daniel");
            
        }

        public void SetGeter(string email)
        {
            getter = new MailAddress(email);
        }

        public void SetGeters(List<string> emails)
        {
            string emailsStr = "";
            foreach (var email in emails)
            {
                emailsStr += email + ";";  
            }
            getter = new MailAddress(emailsStr);
        }

        public void SetTitleAndBody(string subject , string body)
        {
            this.subject = subject;
            this.body = body;
        }

        public bool Send()
        {
            client.EnableSsl = true;
            message = new MailMessage(sender, getter);
            message.Subject = this.subject;
            message.Body = this.body;
            Task.Factory.StartNew(() => client.Send(message));
            return true;
        }

    }
}
