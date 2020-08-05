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
    public class UsersController : Controller
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

        // GET: Users
        public ActionResult Index()
        {
            if (!this.checkIfAdmin())
                return RedirectToAction("index", "Home");

            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (!this.checkIfAdmin())
                return RedirectToAction("index", "Home");

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

        // GET: Users/Create
        public ActionResult Create()
        {
            if (!this.checkIfAdmin())
                return RedirectToAction("index", "Home");

            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserID,isEditor,UserName,Password,Address,PhoneNumber")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!this.checkIfAdmin())
                return RedirectToAction("index", "Home");

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

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,isEditor,UserName,Password,Address,PhoneNumber")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!this.checkIfAdmin())
                return RedirectToAction("index", "Home");

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

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Search(string username)
        {
            if (!this.checkIfAdmin())
                return RedirectToAction("index", "Home");

            User currUser = db.Users.FirstOrDefault(user => user.UserName == username);

            return currUser == null ? RedirectToAction("Index") : RedirectToAction("Details", new { id = currUser.UserID });
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
