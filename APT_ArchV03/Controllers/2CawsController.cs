using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using APT_ArchV03.Models;

namespace APT_ArchV03.Controllers
{
    public class CawsController : Controller
    {
        private Db_APT_ArchEntities db = new Db_APT_ArchEntities();

        // GET: Caws
        public ActionResult Index()
        {
            return View(db.Caws.ToList());
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
            return View(caw);
        }

        // GET: Caws/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Caws/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "caw_id,caw_name,caw_client,caw_partner,caw_manager,caw_office,caw_stdate,caw_reldate,caw_dldate,caw_archdate,caw_fname,caw_notes,caw_status,caw_crdate,caw_usrcreator")] Caw caw)
        {
            if (ModelState.IsValid)
            {
                db.Caws.Add(caw);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

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
    }
}
