using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Resum_Portal.Models;

namespace Resum_Portal.Controllers
{
    public class tbl_UserController : Controller
    {
        private ProjectModel db = new ProjectModel();

        // GET: tbl_User
        public ActionResult Index()
        {
            return View(db.tbl_User.ToList());
        }

        // GET: tbl_User/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_User tbl_User = db.tbl_User.Find(id);
            if (tbl_User == null)
            {
                return HttpNotFound();
            }
            return View(tbl_User);
        }

        // GET: tbl_User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: tbl_User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Email,Name,Mobile,Gender,Address,password,Role,Status")] tbl_User tbl_User)
        {
            if (ModelState.IsValid)
            {
                db.tbl_User.Add(tbl_User);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_User);
        }

        // GET: tbl_User/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_User tbl_User = db.tbl_User.Find(id);
            if (tbl_User == null)
            {
                return HttpNotFound();
            }
            return View(tbl_User);
        }

        // POST: tbl_User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Email,Name,Mobile,Gender,Address,password,Role,Status")] tbl_User tbl_User)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_User).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_User);
        }

        // GET: tbl_User/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_User tbl_User = db.tbl_User.Find(id);
            if (tbl_User == null)
            {
                return HttpNotFound();
            }
            return View(tbl_User);
        }

        // POST: tbl_User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            tbl_User tbl_User = db.tbl_User.Find(id);
            db.tbl_User.Remove(tbl_User);
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
