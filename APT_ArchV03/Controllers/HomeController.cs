using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using APT_ArchV03.Models;

namespace APT_ArchV03.Controllers
{
    public class HomeController : Controller
    {
        private Db_APT_ArchEntities db = new Db_APT_ArchEntities(); //da all'accesso al database

        public ActionResult Index()
        {
            
            List<Caw> caws = new List<Caw>(); //creo lista caws
            caws = (from c in db.Caws select c).ToList(); //tramite linq creo una collection che converto in lista e assegno a variabile caws
            
            float opened = caws.Count(x => x.caw_status == 1); //metto in una variabile di tipo float tutti i caws di stato 1
            float reporting = caws.Count(x => x.caw_status == 2); //metto in una variabile di tipo float tutti i caws di stato 2
            float closed = caws.Count(x => x.caw_status == 3); //metto in una variabile di tipo float tutti i caws di stato 3
            float total = opened + reporting + closed;

            ViewData["opened"] = opened;
            ViewData["reporting"] = reporting;
            ViewData["closed"] = closed;
            ViewData["total"] = total;

            opened = (opened / total * 100);
            reporting = reporting / total * 100;
            closed = closed / total * 100;

         
            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
            ViewData["openedperc"] = Convert.ToString(Math.Ceiling(opened), culture); //variabile opened passata alla vista tramite ViewData
            ViewData["reportingperc"] = Convert.ToString(Math.Floor(reporting), culture);
            ViewData["closedperc"] = Convert.ToString(Math.Floor(closed), culture);

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


    }
}