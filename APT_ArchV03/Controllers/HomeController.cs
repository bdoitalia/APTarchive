using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using APT_ArchV03.Helpers;
using APT_ArchV03.Models;

namespace APT_ArchV03.Controllers
{
    //[Authorize(Roles = @"BDOITALIA\APTarch")]
    //[HandleError]
    public class HomeController : Controller
    {
        private Db_APT_ArchEntities db = new Db_APT_ArchEntities(); //da all'accesso al database

        //[Authorize(Roles = @"BDOITALIA\APTarch")]
        //[Authorize(Roles = @"BDOITALIA\APTarchadmin")]
        public ActionResult Index()
        {

            if (Roles.IsUserInRole(@"BDOITALIA\APTarchAdmin"))
            {
                List<Caw> caws = new List<Caw>(); //creo lista caws
                caws = (from c in db.Caws select c).ToList(); //tramite linq creo una collection che converto in lista e assegno a variabile caws

                float expiringFifteen = caws.Where(a => a.caw_reldate >= DateTime.Now).Count(b => b.caw_reldate < DateTime.Now.AddDays(16));
                float openedlastsevendays = caws.Where(a => a.caw_crdate <= DateTime.Now).Count(b => b.caw_crdate > DateTime.Now.AddDays(-7));
                float closedlastsevendays = caws.Where(a => a.caw_archdate <= DateTime.Now).Count(b => b.caw_archdate > DateTime.Now.AddDays(-7));

                float opened = caws.Count(x => x.caw_status == 1); //metto in una variabile di tipo float tutti i caws di stato 1
                float reporting = caws.Count(x => x.caw_status == 2); //metto in una variabile di tipo float tutti i caws di stato 2
                float closed = caws.Count(x => x.caw_status == 3); //metto in una variabile di tipo float tutti i caws di stato 3
                float total = opened + reporting + closed;

                ViewData["opened"] = opened;
                ViewData["reporting"] = reporting;
                ViewData["closed"] = closed;
                ViewData["total"] = total;
                ViewData["expiringFifteen"] = expiringFifteen;
                ViewData["openedlastsevendays"] = openedlastsevendays;
                ViewData["closedlastsevendays"] = closedlastsevendays;

                opened = (opened / total * 100);
                reporting = reporting / total * 100;
                closed = closed / total * 100;

                //if (Roles.IsUserInRole(@"BDOITALIA\APTarchAdmin"))
                //{
                //    ViewData["expiringFifteen"] = 700;
                //}

                System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
                ViewData["openedperc"] = Convert.ToString(Math.Ceiling(opened), culture); //variabile opened passata alla vista tramite ViewData
                ViewData["reportingperc"] = Convert.ToString(Math.Floor(reporting), culture);
                ViewData["closedperc"] = Convert.ToString(Math.Floor(closed), culture);

                return View();
            }
            if (Roles.IsUserInRole(@"BDOITALIA\APTarch"))
            {
                return RedirectToAction("List", "Caws");
            }
            else
            {
                return RedirectToAction("Contact", "Home", new { subject = 2 });
            }
        }

        [AllowAnonymous]
        public ActionResult About()
        {
            ViewBag.Message = "APT Management ";

            return View();
        }
        [AllowAnonymous]
        public ActionResult Contact(int? subject)
        {
            ViewBag.Message = "Tell us about the problem you're having.";
            

            List<SelectListItem> DrpSubject = new List<SelectListItem>();
            DrpSubject.Add(new SelectListItem { Text = "General inquiry", Value = "General inquiry", Selected = true });
            DrpSubject.Add(new SelectListItem { Text = "Request access", Value = "Access request", Selected = subject == 2 ? true : false });

            ViewData["DrpSubject"] = DrpSubject;
            
            return View();
        }
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult Contact(string formsubject, string formbody)
        {
            ADUser aDUser = new ADUser();
            string completeusername = User.Identity.Name;
            aDUser.UserName = completeusername.Substring(completeusername.IndexOf("\\") + 1);
            try
            {
                aDUser.PopulateADUser(aDUser);
            }
            catch (Exception ex)
            {
                aDUser.FirstName = User.Identity.Name;
            }

            string contactsubject = "Contact form - " + formsubject;
            string body = "<div><font face='Trebuchet MS'><font color='#333333'><span style='FONT-SIZE: 14pt'>APT Management Contact form<br><br>Full name:" + aDUser.FirstName + " " + aDUser.LastName + "<br>Office: " + aDUser.Office + "<br></font><br><br><font color='#333333'><span style='FONT-SIZE: 11pt'>" + formbody + "</font></div>";

            EmailHandler emailHandler = new EmailHandler();

            string to = "helpdesk@bdo.it";

            string cc = db.NavResources.FirstOrDefault(u => u.User_name == HttpContext.User.Identity.Name).Email.ToLower();
                        
            try
            {
                emailHandler.EmailSender(to, contactsubject, body, cc);
                return Json(new { success = true, responseText = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                NLogHandler createNLogHandler = new NLogHandler();
                createNLogHandler.APTLoggerUser("Contact form submit error. User: " + aDUser.UserName, "Error");
                return Json(new { success = false, responseText = "Error: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
            

        }



        //public ActionResult Contact()
        //{
        //    string contactsubject = "Contact form - " + Request.Form["DrpSubject"];
        //    string body = "<div><font face='Trebuchet MS'><font color='#333333'><span style='FONT-SIZE: 14pt'>APT Management Contact form</font><br><br><font color='#333333'><span style='FONT-SIZE: 11pt'>" +  Request.Form["ContactBody"] + "</font></div>";
        //    List<SelectListItem> DrpSubject = new List<SelectListItem>();

        //    if (body == null || body == "")
        //    {
        //        ModelState.AddModelError("Body","Body is mandatory");
        //        ViewData["action"] = "BodyBlank";
        //    }

        //    int? subject = null;

        //    if (contactsubject != "General inquiry")
        //    {
        //        subject = 2;
        //    }

        //    if (ModelState.IsValid)
        //    {



        //        EmailHandler emailHandler = new EmailHandler();

        //        string to = "helpdesk@bdo.it";

        //        string cc = db.NavResources.FirstOrDefault(u => u.User_name == HttpContext.User.Identity.Name).Email.ToLower();

        //        emailHandler.EmailSender(to, contactsubject, body, cc);

        //        ModelState.Clear();

        //        ViewData["action"] = "Alert";

        //        //List<SelectListItem> DrpSubject = new List<SelectListItem>();
        //        DrpSubject.Add(new SelectListItem { Text = "General inquiry", Value = "General inquiry", Selected = true });
        //        DrpSubject.Add(new SelectListItem { Text = "Request access", Value = "Access request", Selected = subject == 2 ? true : false });

        //        ViewData["DrpSubject"] = DrpSubject;

        //        return RedirectToAction("Contact", new {subject = subject });

        //    }

        //    ModelState.Clear();
        //    DrpSubject.Add(new SelectListItem { Text = "General inquiry", Value = "General inquiry", Selected = true });
        //    DrpSubject.Add(new SelectListItem { Text = "Request access", Value = "Access request", Selected = subject == 2 ? true : false });

        //    ViewData["DrpSubject"] = DrpSubject;

        //    return View();
        //}

    }
}