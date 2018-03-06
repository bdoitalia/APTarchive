using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using APT_ArchV03.Helpers;
using System.Web.Mvc;
using APT_ArchV03.Models;

namespace APT_ArchV03.Controllers
{

    public class CawsController : Controller
    {
        private Db_APT_ArchEntities db = new Db_APT_ArchEntities();

        // GET: Caws
        //[Authorize(Roles = @"BDOITALIA\Administrators")]
        public ActionResult List(ProductSearchModel searchModel)
        {
            var business = new ProductBusinessLogic();
            var model = business.GetProducts(searchModel);

            
            var yearlst = model.Select(x => new SelectListItem {
                Text = x.caw_stdate.Value.Year.ToString(),
                Value = x.caw_stdate.Value.Year.ToString()
            }).Distinct();

            var officelst = model.Select(x => new SelectListItem
            {
                Text = x.caw_office.ToString(),
                Value = x.caw_office.ToString()
            }).Distinct();

            var partnerlst = model.Select(x => new SelectListItem
            {
                Text = x.caw_partner.ToString(),
                Value = x.caw_partner.ToString()
            }).Distinct();

            var managerlst = model.Select(x => new SelectListItem
            {
                Text = x.caw_manager.ToString(),
                Value = x.caw_manager.ToString()
            }).Distinct();

            var clientlst = model.Select(x => new SelectListItem
            {
                Text = x.caw_client.ToString(),
                Value = x.caw_client.ToString()
            }).Distinct();

            var statustlst = model.Select(x => new SelectListItem
            {
                Text = x.caw_status == 1 ? "Opened" : ( x.caw_status == 2 ? "Reporting" : "Closed" ),
                Value = x.caw_status.ToString()
            }).Distinct();

            //List< CawJob > cawjobs = new List<CawJob>();

            var cawjobs = (from nj in db.CawJobs
                               //where nj.Job_Code.Equals(item.cawjob_jc)
                               select nj.cawjob_jc).Distinct();

            List<SelectListItem> joblst = new List<SelectListItem>();
            foreach (var item in cawjobs)
            {
                var navjobsquery = from nj in db.NavJobs
                                   where nj.Job_Code.Equals(item)
                                   select nj.Job_Name;
                joblst.Add( new SelectListItem() { Value = item, Text = item + " - " + navjobsquery.First() } );

            }

            
            ViewData["lstYear"] = yearlst;
            ViewData["lstOffice"] = officelst;
            ViewData["lstJob"] = new SelectList(joblst,"Value","Text");
            ViewData["lstManager"] = managerlst;
            ViewData["lstPartner"] = partnerlst;
            ViewData["lstClient"] = clientlst;
            ViewData["lstStatus"] = statustlst;

            
            return View(model.ToList());
            //return View(db.Caws.ToList());
        }
        [HttpPost]
        public ActionResult ApplyFilter(string year, string client, string job, string partner, string manager, string office, string status )
        {
            ProductSearchModel searchModel = new ProductSearchModel();

            if (!string.IsNullOrEmpty(year))
            {
                searchModel.Year = Convert.ToInt16(year);
            }
            
            searchModel.Client = client;
            searchModel.Job = job;
            searchModel.Partner = partner;
            searchModel.Manager = manager;
            searchModel.Office = office;
            if (!string.IsNullOrEmpty(status))
            {
                searchModel.Status = Convert.ToInt16(status);
            }
            

            var business = new ProductBusinessLogic();
            
            var model = business.GetProducts(searchModel);
            var list = model.ToList();
            var table = list.Select( x => new {
                CawID = x.caw_id,
                CawName = x.caw_name,
                Client = x.caw_client,
                Partner = x.caw_partner,
                Manager = x.caw_manager,
                Office = x.caw_office,
                Year = x.caw_stdate.Value.Year.ToString(),
                Status = x.caw_status.ToString()

            } ).ToList();

            return Json(table, JsonRequestBehavior.AllowGet);
            //return View(model.ToList());
            //return View(db.Caws.ToList());
        }

        // GET: Caws/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Caw caw = db.Caws.Find(id);
            if (caw == null)
            {
                return HttpNotFound();
            }

            
            List<SelectListItem> items2 = new List<SelectListItem>();

            foreach (var cjitem in caw.CawJobs)
            {
                var navjobsquery = from nj in db.NavJobs
                                   where nj.Job_Code.Equals(cjitem.cawjob_jc)
                                   select nj;
                
                var items = navjobsquery.Select(a => new SelectListItem
                {
                    Value = a.Job_Code,
                    Text = a.Job_Code + " - " + a.Job_Name
                });
                
                items2.Add(new SelectListItem() { Text = items.First().Text, Value = items.First().Value });
            }

            var items4 = caw.CawJobs.Select(x => new SelectListItem {
                Value = x.cawjob_id.ToString(),
                Text = x.cawjob_jc + " - " + x.cawjob_jn
            });

            TempData["cawdata"] = caw;
            ViewData["LstCawJobs"] = new SelectList(items2, "Value", "Text");
            ViewData["LstCawJobs1"] = items4;
            return View(caw);
        }

        // GET: Caws/Create
        public ActionResult Create()
        {
            var clients = PopulateClients();
            string separator = " - ";
            var selectListItems = clients.Select(
                    a => new SelectListItem
                    {
                        Text = a.Client_ID + separator + a.Client_Name,
                        Value = a.Client_ID
                    }
            );            
            
            ViewData["Client"] = selectListItems;
            return View();
        }

        

        // POST: Caws/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]        
        public ActionResult Create([Bind(Include = "caw_name,caw_stdate,caw_notes")] Caw caw)
        {
            NLogHandler createNLogHandler = new NLogHandler();

            string lstcawjobs = Request[key: "LstCawJobs"];
            string tmpclnt = Request[key: "caw_client"];
            tmpclnt = tmpclnt.Substring(0,(tmpclnt.IndexOf(" - ") ));

            if (lstcawjobs is null)
            {
                ModelState.AddModelError("LstCawJobs", "Add Jobs");
            }            
            
            if (ModelState.IsValid)
            {
                //string name = Request[key: "Clients"];
                //string tmpclnt = formCollection["Client"];
                //var tmpclnt = Request[key: "Client"];


                var clntquery = from c in db.NavClients
                                where c.Client_ID.Equals(tmpclnt)
                                select c;


                var tmpmgr = Request[key: "Manager"];
                var mgrquery = from m in db.NavResources
                                where m.Staff_NO.Equals(tmpmgr)
                                select m;
                //string lstcawjobs = Request[key: "LstCawJobs"];
                
                string[] tmpcawjobs = lstcawjobs.Split(',');               
                

                foreach (string tmpcawjob in tmpcawjobs)
                {
                    string tmpcawjob_jc = tmpcawjob.Substring(0,tmpcawjob.IndexOf(" - "));
                    string tmpcawjob_jn = tmpcawjob.Substring(tmpcawjob.IndexOf(" - ") + 3 );

                    var cawJob = new CawJob();
                    cawJob.cawjob_jc = tmpcawjob_jc;
                    cawJob.cawjob_jn = tmpcawjob_jn;
                    caw.CawJobs.Add(cawJob);                    

                }

                caw.caw_client = clntquery.First().Client_Name;
                caw.caw_client_code = tmpclnt;

                caw.caw_partner = Request[key: "Partner"];
                caw.caw_partner_code = GetSamAccount(caw.caw_partner);

                caw.caw_manager = mgrquery.First().Resource_Name;
                caw.caw_manager_code = tmpmgr;

                string tmpdelplan = Request[key: "caw_delplan"];
                caw.caw_delplan = Convert.ToInt16(tmpdelplan);

                caw.caw_type = Request[key: "caw_type"];

                caw.caw_office = Request[key: "Office"];
                caw.caw_usrcreator = GetResourceName(User.Identity.Name);
                caw.caw_usrcreator_code = User.Identity.Name;
                caw.caw_status = 1;
                caw.caw_crdate = DateTime.Now;                            

                db.Caws.Add(caw);
                db.SaveChanges();

                EmailHandler emailHandler = new EmailHandler();
                //Putting on Cc the user behind the request

                string usernamerequest = HttpContext.User.Identity.Name;

                string cc = db.NavResources.FirstOrDefault(u => u.User_name == usernamerequest).Email;

                string to = db.NavResources.FirstOrDefault(x => x.User_name == caw.caw_usrcreator_code).Email;

                //Send email
                emailHandler.EmailSender(to, "New CAW for client " + caw.caw_client_code, emailHandler.BodyConstructor(1, caw));

                createNLogHandler.APTLoggerUser("APT " + caw.caw_name +  " created", "Info");
                //createNLogHandler.APTLoggerUser("User: " + caw.caw_usrcreator_code, "Info");

                return RedirectToAction("List");

            }
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            var clients = PopulateClients();
            string separator = " - ";
            var selectListItems = clients.Select(
                    a => new SelectListItem
                    {
                        Text = a.Client_ID + separator + a.Client_Name,
                        Value = a.Client_ID
                    }
            );

            //Log errors

            ViewData["Client"] = selectListItems;

            return View(caw);
        }

        // GET: Caws/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Caw caw = db.Caws.Find(id);
            if (caw == null)
            {
                return HttpNotFound();
            }
            return View(caw);
        }

        // POST: Caws/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "caw_id,caw_name,caw_client,caw_partner,caw_manager,caw_office,caw_stdate,caw_reldate,caw_dldate,caw_archdate,caw_fname,caw_notes,caw_status,caw_crdate,caw_usrcreator")] Caw caw)
        {
            if (ModelState.IsValid)
            {
                db.Entry(caw).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(caw);
        }

        // GET: Caws/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Caw caw = db.Caws.Find(id);
            if (caw == null)
            {
                return HttpNotFound();
            }
            return View(caw);
        }

        // POST: Caws/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Caw caw = db.Caws.Find(id);
            db.Caws.Remove(caw);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //My Methods

        public string GetResourceName(string param)
        {

            var navresourcequery = from res in db.NavResources
                                   where res.User_name.Equals(param)
                                   select res;
            string item = navresourcequery.First().Resource_Name;


            return item;
        }

        public string GetSamAccount(string resourcename)
        {

            var navresourcequery = from res in db.NavResources
                                   where res.Resource_Name.Equals(resourcename)
                                   select res;
            string item = navresourcequery.First().User_name;
            string samaccount = item.Substring((item.IndexOf("\\")) + 1);


            return samaccount;
        }


        /*********Client List*********/

        private static List<NavClient> PopulateClients()
        {
            List<NavClient> items = new List<NavClient>();
            //Using EF Connection string
            var efConnectionString = "Db_APT_ArchEntities";
            string constr = ConfigurationManager.ConnectionStrings["Db_APT_ArchEntities"].ConnectionString;
            var builder = new EntityConnectionStringBuilder(constr);
            var regularConnectionString = builder.ProviderConnectionString;

            using (SqlConnection con = new SqlConnection(regularConnectionString))
            {
                string separator = " - ";
                string query = " SELECT [ID], [Client Name], [Client ID] FROM NavClients";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            items.Add(
                                new NavClient
                                {
                                    ID = Convert.ToInt32(sdr["ID"]),
                                    Client_Name = sdr["Client Name"].ToString(),
                                    Client_ID = sdr["Client ID"].ToString()

                                }
                            );
                        }
                    }
                    con.Close();
                }
            }
            return items;
        }
        /*********Job List*********/

        [HttpPost]
        public JsonResult GetJobs(string param)
        {
            var jobsquery = from j in db.NavJobs
                            where j.Client.Equals(param)
                            orderby j.id
                            select j;
            string separator = " - ";
            //var items = new SelectList(jobsquery, "Job_Code", "Job_Name");
            var items = jobsquery.Select(a => new SelectListItem
            {
                Value = a.Job_Code + separator + a.Job_Name,
                Text = a.Job_Code + separator + a.Job_Name
            });

            return Json(items);
        }

        /*********Partner List*********/

        [HttpPost]
        public JsonResult GetPartner(string param)
        {
            var partnerquery = from p in db.NavJobs
                               where p.Job_Code.Equals(param)
                               orderby p.id
                               select p;
            string separator = " - ";
          
            //var items = new SelectList(jobsquery, "Job_Code", "Job_Name");
            var items = partnerquery.Select(a => new SelectListItem
            {
                Value = a.Partner_Gestore,
                Text = a.Partner_Gestore
            });

            return Json(items);
        }


        [HttpPost]
        public JsonResult CompletePartner(string Prefix)
        {
             
            var PartnerQuery = (from N in db.NavJobs
                               where N.Partner_Gestore.Contains(Prefix)
                               select new { N.Partner_Gestore } ).Distinct();
            var PartnerList = PartnerQuery.Select(a => new SelectListItem
            {
                Value = a.Partner_Gestore,
                Text = a.Partner_Gestore
            }).Distinct();
            return Json(PartnerQuery, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RetrieveClients(string Prefix)
        {
            string separator = " - ";
            var clientquery = (from N in db.NavClients
                                where ( N.Client_Name.Contains(Prefix) || N.Client_ID.Contains(Prefix) )
                                select new { N.Client_Name , N.Client_ID });
            //var ClientList = clientquery.Select(a => new SelectListItem
            //{
            //    Value = a.Client_ID,
            //    Text = a.Client_Name
            //}).Distinct();
            return Json(clientquery, JsonRequestBehavior.AllowGet);
        }

        /*********Manager List*********/

        [HttpPost]
        public JsonResult GetManager(string param)
        {
            var managerquery = from m in db.NavResources
                               orderby m.Resource_Name
                               select m;
            //string separator = " - ";
            //var items = new SelectList(jobsquery, "Job_Code", "Job_Name");
            var items = managerquery.Select(a => new SelectListItem
            {
                Value = a.Staff_NO,
                Text = a.Resource_Name
            }).Distinct();

            return Json(items);
        }

        /*********Office List*********/
        [HttpPost]
        public JsonResult GetOffice(string param)
        {
            var officequery = from o in db.NavResources
                              orderby o.Region
                              select o;
            //string separator = " - ";
            //var items = new SelectList(jobsquery, "Job_Code", "Job_Name");
            var items = officequery.Select(a => new SelectListItem
            {
                Value = a.Region,
                Text = a.Region
            }).Distinct();

            return Json(items);
        }
        /****************************/

        /*********PostReportDate*********/
        [HttpPost]
        public JsonResult PostReportDate(int id, string reldate)
        {
            if (id == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { success = false, responseText = "ID is null" }, JsonRequestBehavior.AllowGet);
            }
            Caw caw = db.Caws.Find(id);
            if (caw == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { success = false, responseText = "ID is not valid" }, JsonRequestBehavior.AllowGet);
            }
            if (caw.caw_status != 1)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { success = false, responseText = "Invalid stage" }, JsonRequestBehavior.AllowGet);
            }
            DateTime temp;
            //Validate request
            if (DateTime.TryParse(reldate.ToString(), out temp))
            {
                caw.caw_reldate = temp;
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { success = false, responseText = "Action not supported" }, JsonRequestBehavior.AllowGet);
            }

            
            caw.caw_dldate = caw.caw_reldate.Value.AddDays(Convert.ToDouble(caw.caw_delplan));
            caw.caw_status = 2;

            db.Entry(caw).State = EntityState.Modified;
            db.SaveChanges();

            EmailHandler emailHandler = new EmailHandler();
            //Putting on Cc the user behind the request

            string usernamerequest = HttpContext.User.Identity.Name;

            string cc = db.NavResources.FirstOrDefault(u => u.User_name == usernamerequest).Email;

            string to = db.NavResources.FirstOrDefault(x => x.User_name == caw.caw_usrcreator_code).Email;

            //Send email
            if (cc == to)
            {
                emailHandler.EmailSender(to, "Modified - CAW for client " + caw.caw_client_code, emailHandler.BodyConstructor(2, caw));
            }
            else
            {
                emailHandler.EmailSender(to, "Modified - CAW for client " + caw.caw_client_code, emailHandler.BodyConstructor(2, caw), cc);
            }
            

            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(new { success = true, responseText = "Report date submitted successfully" }, JsonRequestBehavior.AllowGet);
        }
        /****************************/

        /*********ARCHIVE APT*********/
        [HttpPost]
        public JsonResult ArchiveAPT(int id)
        {
            Caw caw = db.Caws.Find(id);
            if (caw == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { success = false, responseText = "ID is not valid" }, JsonRequestBehavior.AllowGet);
            }
            if (caw.caw_status != 2)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { success = false, responseText = "Invalid stage" }, JsonRequestBehavior.AllowGet);
            }

            caw.caw_archdate = DateTime.Now;

            //caw.caw_dldate = caw.caw_reldate.Value.AddDays(Convert.ToDouble(caw.caw_delplan));
            caw.caw_status = 3;

            db.Entry(caw).State = EntityState.Modified;
            db.SaveChanges();

            EmailHandler emailHandler = new EmailHandler();
            //Putting on Cc the user behind the request

            string usernamerequest = HttpContext.User.Identity.Name;

            string cc = db.NavResources.FirstOrDefault(u => u.User_name == usernamerequest).Email;

            string to = db.NavResources.FirstOrDefault(x => x.User_name == caw.caw_usrcreator_code).Email;

            //Send email
            if (cc == to)
            {
                emailHandler.EmailSender(to, "Archived - CAW for client " + caw.caw_client_code, emailHandler.BodyConstructor(3, caw));
            }
            else
            {
                emailHandler.EmailSender(to, "Archived - CAW for client " + caw.caw_client_code, emailHandler.BodyConstructor(3, caw), cc);
            }

            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(new { success = true, responseText = "APT successfully closed" }, JsonRequestBehavior.AllowGet);
        }
        /****************************/

    }
}
