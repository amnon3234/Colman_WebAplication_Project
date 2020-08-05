﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Colman_MVC_WebAplication_Project.Data;
using Colman_MVC_WebAplication_Project.Models;

namespace Colman_MVC_WebAplication_Project.Controllers
{
    public class CategoriesController : Controller
    {
        private StoreDataBase db = new StoreDataBase();

        private bool checkIfAdmin()
        {
            if (Session["user"] != null)
            {
                string userName = Session["user"] as string;
                User user = db.Users.FirstOrDefault(u => u.UserName == userName);
                if (user.isEditor)
                    return true;
            }

            return false;
        }

        // GET: Categories
        public ActionResult Index()
        {
            if (!this.checkIfAdmin())
                return RedirectToAction("index", "Home");

            return View(db.Categories.ToList());
        }

        // GET: Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (!this.checkIfAdmin())
                return RedirectToAction("index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            if (!this.checkIfAdmin())
                return RedirectToAction("index", "Home");

            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategoryID,CategoryName,CategoryImagePath,CategoryDescription")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!this.checkIfAdmin())
                return RedirectToAction("index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoryID,CategoryName,CategoryImagePath,CategoryDescription")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!this.checkIfAdmin())
                return RedirectToAction("index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Search(string categoryName)
        {
            if (!this.checkIfAdmin())
                return RedirectToAction("index", "Home");

            Category curr = db.Categories.FirstOrDefault(category => category.CategoryName == categoryName);
            return curr == null ? RedirectToAction("Index") : RedirectToAction("Details", new { id = curr.CategoryID });
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
