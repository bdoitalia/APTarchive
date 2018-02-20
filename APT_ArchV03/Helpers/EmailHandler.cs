using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace APT_ArchV03.Helpers
{
    public class EmailHandler
    {
        string SmtpServer { get; set; }
        int SmtpPort { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
        string From { get; set; }
        bool EnableSsl { get; set; }
        bool SmtpUseDefaultCredentials { get; set; }

        public EmailHandler() {

            SmtpServer = "smtp.office365.com";
            SmtpPort = 587;
            SmtpUseDefaultCredentials = true;
            EnableSsl = true;
            UserName = "juan.roca@bdo.it";
            Password = "Protocol76*1";
            From = "juan.roca@bdo.it";

        }


        public bool EmailSender(string to, string subject, string body)
        {
              
            WebMail.SmtpServer = SmtpServer;
            //gmail port to send emails  
            WebMail.SmtpPort = SmtpPort;
            WebMail.SmtpUseDefaultCredentials = SmtpUseDefaultCredentials;
            //sending emails with secure protocol  
            WebMail.EnableSsl = EnableSsl;
            //EmailId used to send emails from application  
            WebMail.UserName = UserName;
            WebMail.Password = Password;

            //Sender email address.  
            WebMail.From = From;

            try
            {
                //Send email  
                WebMail.Send(to: to, subject: subject, body: body, isBodyHtml: true);
                return true;

            }
            catch (Exception ex)
            {
                //Log errors
                string text = ex.Message;
                return false;
            }            

        }

    }

    
}