using System.Web;
using System.Web.Mvc;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using APT_ArchV03.Models;




namespace APT_ArchV03.Controllers
{
    public class UploadController : Controller
    {
        // GET: Upload
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult Upload(HttpPostedFileBase file, Caw caw)
        {

            string allowedExtensions = ".gsa";


            if (file != null && file.ContentLength > 0)
            {
                var partner = "abongiovanni" ;
                var filename = Path.GetFileName(file.FileName);
                if (filename.Contains(allowedExtensions))
                {
                    string model = Path.Combine("\\\\bdo.priv\\apt\\mi\\" + partner + "\\Financial Audit\\Incarichi 2017", filename);
                    file.SaveAs(model);
                    ViewBag.Msg = "Uploaded Successfully";
                }
                else
                {
                    ViewBag.Msg = "Formato del file non corretto";
                }
            }
            else
            {
                ViewBag.Msg = "Uploaded Failed";
            }
            return View(caw);



        }
    }
}

    
