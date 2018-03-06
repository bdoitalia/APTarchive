using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using APT_ArchV03.Helpers;
using APT_ArchV03.Models;

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
        NLogHandler emailNLogHandler = new NLogHandler();


        public EmailHandler() {

            SmtpServer = "smtp.office365.com";
            SmtpPort = 587;
            SmtpUseDefaultCredentials = true;
            EnableSsl = true;
            UserName = "juan.roca@bdo.it";
            Password = "Protocol76*";
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
                emailNLogHandler.APTLoggerUser("Sent success to " + to, "Info");
                return true;

            }
            catch (Exception ex)
            {
                //Log errors
                string text = ex.Message;
                emailNLogHandler.APTLoggerUser("Sent Failure to: " + to, "Error");
                emailNLogHandler.APTLoggerSystem("Sent Failure to: " + to + ". Error msg: " + text, "Error");
                return false;
            }            

        }

        public bool EmailSender(string to, string subject, string body, string cc)
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
                WebMail.Send(to: to, subject: subject, body: body, isBodyHtml: true, cc: cc);
                emailNLogHandler.APTLoggerUser("Sent success to " + to, "Info");
                return true;

            }
            catch (Exception ex)
            {
                //Log errors
                string text = ex.Message;
                emailNLogHandler.APTLoggerUser("Sent Failure to: " + to, "Error");
                emailNLogHandler.APTLoggerSystem("Sent Failure to: " + to + ". Error msg: " + text, "Error");
                return false;
            }

        }

        //Email body constructor
        public string BodyConstructor(int phase, Caw caw)
        {
            string[] HtmlEmailBody = new string[3];

            int sep = caw.caw_usrcreator.Count(c => c == ' ');

            string uName = caw.caw_usrcreator.Substring(caw.caw_usrcreator.Length - caw.caw_usrcreator.IndexOf(' ', caw.caw_usrcreator.IndexOf(' ') + sep - 1));
                       

            HtmlEmailBody[0] = "<div><font face='Trebuchet MS'><font color='#333333'><span style='FONT-SIZE: 11pt'>Caro " + uName + ",<br><br>" +
                "ti confermaimo che il CAW &egrave; stato inserito correttamente.<br>Di seguito ti elenchiamo i dati del CAW:<br><br>" +
                 "<table> " +
                  "<tbody> " +
                    "<tr> <td> " +
                      "<table style='FONT-SIZE: 11pt'> " +
                        "<tbody>" +
                          "<tr><td style='font-weight:bold;'>Codice Cliente:</td><td>" + caw.caw_client_code + "</td></tr>" +
                          "<tr><td style='font-weight:bold;'>Partner:</td><td>" + caw.caw_partner + "</td></tr>" +
                          "<tr><td style='font-weight:bold;display: inline-block;line-height: 14px;'>Commesse:</td><td><dl style='margin-top: 0;'>" + "<dt>" + String.Join("</dt><dt>", caw.CawJobs.Select(x => x.cawjob_jn).ToArray()) + "</dt>" + "</dl></td></tr>" +
                          "<tr><td style='font-weight:bold;'>Data Bilancio:</td><td>" + caw.caw_stdate.Value.ToShortDateString() + "</td></tr>" +
                          "<tr><td style='font-weight:bold;'>Nome CAW:</td><td>" + caw.caw_name + "</td></tr>" +
                        "</tbody>" +
                      "</table>" +
                    "</td></tr>" +
                   "</tbody>" +
                  "</table><br>" +
                  "Cliccando su questo <a href='" + HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/Caws/Details/" + caw.caw_id + "'>link</a> puoi visualizzare la scheda del CAW.<br><br>" +
                  "<strong><span style='FONT-SIZE: 11pt'>APT TEAM</span>&nbsp;&nbsp; </strong></span><span style='FONT-SIZE: 11pt'>&nbsp;</span> <br>" +
                  "</font><font color='#333333'><span style='FONT-SIZE: 11pt'><span style='FONT-SIZE: 11pt'><span style='FONT-SIZE: 11pt'><span style='FONT-SIZE: 11pt'></span></span><span style='FONT-SIZE: 11pt'>Professional Standards</span><span style='FONT-SIZE: 11pt'></span></span><span style='FONT-SIZE: 11pt'>&nbsp;</span><span style='FONT-SIZE: 11pt'>&nbsp;</span> <br>" +
                  "<span style='FONT-SIZE: 11pt'>Direct +39 02 582010</span><span style='FONT-SIZE: 11pt'>&nbsp;</span> <br>" +
                  "<span style='FONT-SIZE: 11pt'></span><font color='#ff0000'><u><span style='FONT-SIZE: 11pt'>apt@bdo.it</span></u></font></font></div><div><font face='Trebuchet MS'></font>&nbsp;</div><div><font face='Trebuchet MS'><font color='#333333'><span style='FONT-SIZE: 11pt'>BDO Italia S.p.A.</span><span style='FONT-SIZE: 11pt'>&nbsp;</span>" +
                  "<br><span style='FONT-SIZE: 11pt'>Viale Abruzzi,94</span><br><span style='FONT-SIZE: 11pt'>20131 Milano</span><span style='FONT-SIZE: 11pt'>&nbsp;</span> " +
                  "<br><span style='FONT-SIZE: 11pt'>ITALY</span><br>" +
                  "<span style='FONT-SIZE: 11pt'>Office +39 02 582010</span></font><span style='FONT-SIZE: 11pt'>&nbsp;</span> <br>" +
                  "</font><a href='" + HttpUtility.UrlEncode("http://www.bdo.it") + "'><font color='#ff0000' face='Trebuchet MS'><span style='FONT-SIZE: 11pt'>www.bdo.it</span></font></a><br>" +
                  "<font color='#009900' face='Webdings' size='4'><br>" +
                  "<span style='FONT-SIZE: 11pt'><font color='#333333' face='Trebuchet MS'><span style='FONT-SIZE: 10pt'>Seguici su </span><a href='https://www.linkedin.com/company/bdo'><span style='FONT-SIZE: 10pt'>Linkedin</span></a>&nbsp;<font size='2'>e <a href='https://twitter.com/bdo_italia'>Twitter</a>&nbsp;<br><br><br>" +
                  "</font></font></span>P</font><span style='FONT-SIZE: 8pt'><font face='Trebuchet MS'> <font color='#333333'>Before you print think about the environment</font> </font> <br><br>" +
                  "<font color='#333333'><font face='Trebuchet MS'><span style='FONT-SIZE: 8pt'>BDO Italia S.p.A., an Italian limited liability company, is a member of BDO International Limited, a UK company limited by guarantee, and forms part of the international BDO network of independent member firms. BDO is the brand name for the BDO network and for each of the BDO Member Firms. </span><br style='FONT-SIZE: 8pt'><br style='FONT-SIZE: 8pt'>" +
                  "<span style='FONT-SIZE: 8pt'>AVVISO DI RISERVATEZZA PER LA POSTA ELETTRONICA / IMPORTANT NOTICE </span><br style='FONT-SIZE: 8pt'><br style='FONT-SIZE: 8pt'>" +
                  "<span style='FONT-SIZE: 8pt'>Le informazioni contenute nella presente comunicazione sono di carattere strettamente confidenziale e sono riservate alla sola persona o società identificata come destinataria. Nel caso non siate la persona destinataria Vi informiamo che ogni divulgazione, copia o azione intrapresa sulla base delle informazioni contenute nella presente mail è proibita e sarà perseguita nei termini di legge. Qualora riceveste questa mail per errore, del quale ci scusiamo, Vi preghiamo di darcene immediata comunicazione rispondendo a questo stesso indirizzo e-mail e di cancellarlo definitivamente dal vostro computer. </span><br style='FONT-SIZE: 8pt'><br style='FONT-SIZE: 8pt'>" +
                  "<span style='FONT-SIZE: 8pt'>The contents of this message, as well as any enclosures, are addressed personally to, and thus solely intended for the addressee. They may contain information regarding a third party. A recipient who is neither the addressee, nor empowered to receive this message on behalf of the addressee, is kindly requested to immediately inform the sender of receipt replying to this e-mail and to delete it from your system. Any use of the contents of this message and/or of the enclosures by any other person than the addressee is illegal towards the sender and the aforementioned third party.</span></font></font> </span></div>";


            HtmlEmailBody[1] = "<div><font face='Trebuchet MS'><font color='#333333'><span style='FONT-SIZE: 11pt'>Caro " + uName + ",<br><br>" +
                "ti confermaimo che il CAW &egrave; stato inserito correttamente.<br>Di seguito ti elenchiamo i dati del CAW:<br><br>" +
                 "<table> " +
                  "<tbody> " +
                    "<tr> <td> " +
                      "<table style='FONT-SIZE: 11pt'> " +
                        "<tbody>" +
                          "<tr><td style='font-weight:bold;'>Codice Cliente:</td><td>" + caw.caw_client_code + "</td></tr>" +
                          "<tr><td style='font-weight:bold;'>Partner:</td><td>" + caw.caw_partner + "</td></tr>" +
                          "<tr><td style='font-weight:bold;display: inline-block;line-height: 14px;'>Commesse:</td><td><dl style='margin-top: 0;'>" + "<dt>" + String.Join("</dt><dt>", caw.CawJobs.Select(x => x.cawjob_jn).ToArray()) + "</dt>" + "</dl></td></tr>" +
                          "<tr><td style='font-weight:bold;'>Data Bilancio:</td><td>" + caw.caw_stdate.Value.ToShortDateString() + "</td></tr>" +
                          "<tr><td style='font-weight:bold;'>Nome CAW:</td><td>" + caw.caw_name + "</td></tr>" +
                          "<tr><td style='font-weight:bold;'>Data Relazione:</td><td>" + caw.caw_reldate.Value.ToShortDateString() + "</td></tr>" +
                          "<tr><td style='font-weight:bold;'>Data Scadenza:</td><td>" + caw.caw_dldate.Value.ToShortDateString() + "</td></tr>" +
                        "</tbody>" +
                      "</table>" +
                    "</td></tr>" +
                   "</tbody>" +
                  "</table><br>" +
                  "Cliccando su questo <a href='" + HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/Caws/Details/" + caw.caw_id + "'>link</a> puoi visualizzare la scheda del CAW.<br><br>" +
                  "<strong><span style='FONT-SIZE: 11pt'>APT TEAM</span>&nbsp;&nbsp; </strong></span><span style='FONT-SIZE: 11pt'>&nbsp;</span> <br>" +
                  "</font><font color='#333333'><span style='FONT-SIZE: 11pt'><span style='FONT-SIZE: 11pt'><span style='FONT-SIZE: 11pt'><span style='FONT-SIZE: 11pt'></span></span><span style='FONT-SIZE: 11pt'>Professional Standards</span><span style='FONT-SIZE: 11pt'></span></span><span style='FONT-SIZE: 11pt'>&nbsp;</span><span style='FONT-SIZE: 11pt'>&nbsp;</span> <br>" +
                  "<span style='FONT-SIZE: 11pt'>Direct +39 02 582010</span><span style='FONT-SIZE: 11pt'>&nbsp;</span> <br>" +
                  "<span style='FONT-SIZE: 11pt'></span><font color='#ff0000'><u><span style='FONT-SIZE: 11pt'>apt@bdo.it</span></u></font></font></div><div><font face='Trebuchet MS'></font>&nbsp;</div><div><font face='Trebuchet MS'><font color='#333333'><span style='FONT-SIZE: 11pt'>BDO Italia S.p.A.</span><span style='FONT-SIZE: 11pt'>&nbsp;</span>" +
                  "<br><span style='FONT-SIZE: 11pt'>Viale Abruzzi,94</span><br><span style='FONT-SIZE: 11pt'>20131 Milano</span><span style='FONT-SIZE: 11pt'>&nbsp;</span> " +
                  "<br><span style='FONT-SIZE: 11pt'>ITALY</span><br>" +
                  "<span style='FONT-SIZE: 11pt'>Office +39 02 582010</span></font><span style='FONT-SIZE: 11pt'>&nbsp;</span> <br>" +
                  "</font><a href='" + HttpUtility.UrlEncode("http://www.bdo.it") + "'><font color='#ff0000' face='Trebuchet MS'><span style='FONT-SIZE: 11pt'>www.bdo.it</span></font></a><br>" +
                  "<font color='#009900' face='Webdings' size='4'><br>" +
                  "<span style='FONT-SIZE: 11pt'><font color='#333333' face='Trebuchet MS'><span style='FONT-SIZE: 10pt'>Seguici su </span><a href='https://www.linkedin.com/company/bdo'><span style='FONT-SIZE: 10pt'>Linkedin</span></a>&nbsp;<font size='2'>e <a href='https://twitter.com/bdo_italia'>Twitter</a>&nbsp;<br><br><br>" +
                  "</font></font></span>P</font><span style='FONT-SIZE: 8pt'><font face='Trebuchet MS'> <font color='#333333'>Before you print think about the environment</font> </font> <br><br>" +
                  "<font color='#333333'><font face='Trebuchet MS'><span style='FONT-SIZE: 8pt'>BDO Italia S.p.A., an Italian limited liability company, is a member of BDO International Limited, a UK company limited by guarantee, and forms part of the international BDO network of independent member firms. BDO is the brand name for the BDO network and for each of the BDO Member Firms. </span><br style='FONT-SIZE: 8pt'><br style='FONT-SIZE: 8pt'>" +
                  "<span style='FONT-SIZE: 8pt'>AVVISO DI RISERVATEZZA PER LA POSTA ELETTRONICA / IMPORTANT NOTICE </span><br style='FONT-SIZE: 8pt'><br style='FONT-SIZE: 8pt'>" +
                  "<span style='FONT-SIZE: 8pt'>Le informazioni contenute nella presente comunicazione sono di carattere strettamente confidenziale e sono riservate alla sola persona o società identificata come destinataria. Nel caso non siate la persona destinataria Vi informiamo che ogni divulgazione, copia o azione intrapresa sulla base delle informazioni contenute nella presente mail è proibita e sarà perseguita nei termini di legge. Qualora riceveste questa mail per errore, del quale ci scusiamo, Vi preghiamo di darcene immediata comunicazione rispondendo a questo stesso indirizzo e-mail e di cancellarlo definitivamente dal vostro computer. </span><br style='FONT-SIZE: 8pt'><br style='FONT-SIZE: 8pt'>" +
                  "<span style='FONT-SIZE: 8pt'>The contents of this message, as well as any enclosures, are addressed personally to, and thus solely intended for the addressee. They may contain information regarding a third party. A recipient who is neither the addressee, nor empowered to receive this message on behalf of the addressee, is kindly requested to immediately inform the sender of receipt replying to this e-mail and to delete it from your system. Any use of the contents of this message and/or of the enclosures by any other person than the addressee is illegal towards the sender and the aforementioned third party.</span></font></font> </span></div>";

            HtmlEmailBody[2] = "<div><font face='Trebuchet MS'><font color='#333333'><span style='FONT-SIZE: 11pt'>Caro " + uName + ",<br><br>" +
                "ti confermaimo che il CAW &egrave; stato inserito correttamente.<br>Di seguito ti elenchiamo i dati del CAW:<br><br>" +
                 "<table> " +
                  "<tbody> " +
                    "<tr> <td> " +
                      "<table style='FONT-SIZE: 11pt'> " +
                        "<tbody>" +
                          "<tr><td style='font-weight:bold;'>Codice Cliente:</td><td>" + caw.caw_client_code + "</td></tr>" +
                          "<tr><td style='font-weight:bold;'>Partner:</td><td>" + caw.caw_partner + "</td></tr>" +
                          "<tr><td style='font-weight:bold;display: inline-block;line-height: 14px;'>Commesse:</td><td><dl style='margin-top: 0;'>" + "<dt>" + String.Join("</dt><dt>", caw.CawJobs.Select(x => x.cawjob_jn).ToArray()) + "</dt>" + "</dl></td></tr>" +
                          "<tr><td style='font-weight:bold;'>Data Bilancio:</td><td>" + caw.caw_stdate.Value.ToShortDateString() + "</td></tr>" +
                          "<tr><td style='font-weight:bold;'>Nome CAW:</td><td>" + caw.caw_name + "</td></tr>" +
                          "<tr><td style='font-weight:bold;'>Data Relazione:</td><td>" + caw.caw_reldate.Value.ToShortDateString() + "</td></tr>" +
                          "<tr><td style='font-weight:bold;'>Data Scadenza:</td><td>" + caw.caw_dldate.Value.ToShortDateString() + "</td></tr>" +
                          "<tr><td style='font-weight:bold;'>Data archiviazione:</td><td>" + caw.caw_archdate + "</td></tr>" +
                        "</tbody>" +
                      "</table>" +
                    "</td></tr>" +
                   "</tbody>" +
                  "</table><br>" +
                  "Cliccando su questo <a href='" + HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/Caws/Details/" + caw.caw_id + "'>link</a> puoi visualizzare la scheda del CAW.<br><br>" +
                  "<strong><span style='FONT-SIZE: 11pt'>APT TEAM</span>&nbsp;&nbsp; </strong></span><span style='FONT-SIZE: 11pt'>&nbsp;</span> <br>" +
                  "</font><font color='#333333'><span style='FONT-SIZE: 11pt'><span style='FONT-SIZE: 11pt'><span style='FONT-SIZE: 11pt'><span style='FONT-SIZE: 11pt'></span></span><span style='FONT-SIZE: 11pt'>Professional Standards</span><span style='FONT-SIZE: 11pt'></span></span><span style='FONT-SIZE: 11pt'>&nbsp;</span><span style='FONT-SIZE: 11pt'>&nbsp;</span> <br>" +
                  "<span style='FONT-SIZE: 11pt'>Direct +39 02 582010</span><span style='FONT-SIZE: 11pt'>&nbsp;</span> <br>" +
                  "<span style='FONT-SIZE: 11pt'></span><font color='#ff0000'><u><span style='FONT-SIZE: 11pt'>apt@bdo.it</span></u></font></font></div><div><font face='Trebuchet MS'></font>&nbsp;</div><div><font face='Trebuchet MS'><font color='#333333'><span style='FONT-SIZE: 11pt'>BDO Italia S.p.A.</span><span style='FONT-SIZE: 11pt'>&nbsp;</span>" +
                  "<br><span style='FONT-SIZE: 11pt'>Viale Abruzzi,94</span><br><span style='FONT-SIZE: 11pt'>20131 Milano</span><span style='FONT-SIZE: 11pt'>&nbsp;</span> " +
                  "<br><span style='FONT-SIZE: 11pt'>ITALY</span><br>" +
                  "<span style='FONT-SIZE: 11pt'>Office +39 02 582010</span></font><span style='FONT-SIZE: 11pt'>&nbsp;</span> <br>" +
                  "</font><a href='" + HttpUtility.UrlEncode("http://www.bdo.it") + "'><font color='#ff0000' face='Trebuchet MS'><span style='FONT-SIZE: 11pt'>www.bdo.it</span></font></a><br>" +
                  "<font color='#009900' face='Webdings' size='4'><br>" +
                  "<span style='FONT-SIZE: 11pt'><font color='#333333' face='Trebuchet MS'><span style='FONT-SIZE: 10pt'>Seguici su </span><a href='https://www.linkedin.com/company/bdo'><span style='FONT-SIZE: 10pt'>Linkedin</span></a>&nbsp;<font size='2'>e <a href='https://twitter.com/bdo_italia'>Twitter</a>&nbsp;<br><br><br>" +
                  "</font></font></span>P</font><span style='FONT-SIZE: 8pt'><font face='Trebuchet MS'> <font color='#333333'>Before you print think about the environment</font> </font> <br><br>" +
                  "<font color='#333333'><font face='Trebuchet MS'><span style='FONT-SIZE: 8pt'>BDO Italia S.p.A., an Italian limited liability company, is a member of BDO International Limited, a UK company limited by guarantee, and forms part of the international BDO network of independent member firms. BDO is the brand name for the BDO network and for each of the BDO Member Firms. </span><br style='FONT-SIZE: 8pt'><br style='FONT-SIZE: 8pt'>" +
                  "<span style='FONT-SIZE: 8pt'>AVVISO DI RISERVATEZZA PER LA POSTA ELETTRONICA / IMPORTANT NOTICE </span><br style='FONT-SIZE: 8pt'><br style='FONT-SIZE: 8pt'>" +
                  "<span style='FONT-SIZE: 8pt'>Le informazioni contenute nella presente comunicazione sono di carattere strettamente confidenziale e sono riservate alla sola persona o società identificata come destinataria. Nel caso non siate la persona destinataria Vi informiamo che ogni divulgazione, copia o azione intrapresa sulla base delle informazioni contenute nella presente mail è proibita e sarà perseguita nei termini di legge. Qualora riceveste questa mail per errore, del quale ci scusiamo, Vi preghiamo di darcene immediata comunicazione rispondendo a questo stesso indirizzo e-mail e di cancellarlo definitivamente dal vostro computer. </span><br style='FONT-SIZE: 8pt'><br style='FONT-SIZE: 8pt'>" +
                  "<span style='FONT-SIZE: 8pt'>The contents of this message, as well as any enclosures, are addressed personally to, and thus solely intended for the addressee. They may contain information regarding a third party. A recipient who is neither the addressee, nor empowered to receive this message on behalf of the addressee, is kindly requested to immediately inform the sender of receipt replying to this e-mail and to delete it from your system. Any use of the contents of this message and/or of the enclosures by any other person than the addressee is illegal towards the sender and the aforementioned third party.</span></font></font> </span></div>";


            switch (phase)
            {
                case 1:
                    return HtmlEmailBody[0];
                    //break;
                case 2:
                    return HtmlEmailBody[1];
                    //break;
                case 3:
                    return HtmlEmailBody[2];
                    //break;
                default:
                    return HtmlEmailBody[0];
                    //break;
            }

        }


    }

    
}