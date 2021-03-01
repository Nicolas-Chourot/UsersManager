using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UsersManager.Data;
using UsersManager.Models;

namespace UsersManager.Controllers
{
    public class UsersController : Controller
    {
        private UsersManagerContext db = new UsersManagerContext();
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Index()
        {
            ViewBag.Salutations = "Bonjour tout le monde";
            return View(db.Users.OrderBy(u => u.FullName).ToList());
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        public ActionResult Create()
        {
            ViewBag.Countries = db.CountriesToSelectList();
            return View(new User());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "Id, Country")] User user)
        {
            if (ModelState.IsValid)
            {
                if (db.AddUser(user))
                    return RedirectToAction("Index");
                else
                    ModelState.AddModelError("Email", "Ce courriel est utilisé par un autre usager");
            }

            return View(user);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.Countries = db.CountriesToSelectList();
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Exclude = "Country")] User user)
        {
            if (ModelState.IsValid)
            {
                if (db.UpdateUser(user))
                    return RedirectToAction("Index");
                else
                    ModelState.AddModelError("Email", "Ce courriel est utilisé par un autre usager");
            }
            ViewBag.Countries = db.CountriesToSelectList();
            return View(user);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            db.DeleteUser(id);
            return RedirectToAction("Index");
        }
    }
}
