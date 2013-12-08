using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using FreePreview;

using FreePreview_Example.Models;

namespace FreePreview_Example.Controllers
{
    public class PreviewController : Controller
    {
        private ExampleContext db = new ExampleContext();

        // GET: /Preview/
        public ActionResult Index()
        {
            return View(db.PreviewSessions.ToList());
        }

        // GET: /Preview/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PreviewSession previewsession = db.PreviewSessions.Find(id);
            if (previewsession == null)
            {
                return HttpNotFound();
            }
            return View(previewsession);
        }

        // GET: /Preview/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Preview/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,CreatedDate,Active,SessionId")] PreviewSession previewsession)
        {
            if (ModelState.IsValid)
            {
                db.PreviewSessions.Add(previewsession);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(previewsession);
        }

        // GET: /Preview/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PreviewSession previewsession = db.PreviewSessions.Find(id);
            if (previewsession == null)
            {
                return HttpNotFound();
            }
            return View(previewsession);
        }

        // POST: /Preview/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,CreatedDate,Active,SessionId")] PreviewSession previewsession)
        {
            if (ModelState.IsValid)
            {
                db.Entry(previewsession).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(previewsession);
        }

        // GET: /Preview/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PreviewSession previewsession = db.PreviewSessions.Find(id);
            if (previewsession == null)
            {
                return HttpNotFound();
            }
            return View(previewsession);
        }

        // POST: /Preview/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PreviewSession previewsession = db.PreviewSessions.Find(id);
            db.PreviewSessions.Remove(previewsession);
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
