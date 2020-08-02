using System;
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
    public class CartsController : Controller
    {
        private StoreDataBase db = new StoreDataBase();

        public ActionResult Index(int userId = 1)
        {
            var carts = db.Carts.Include(c => c.Product).Include(c => c.User).Where(c => c.UserID == userId).ToList();
            return View(carts.Count > 0
                ? carts
                : new List<Cart>()
                {
                    new Cart
                    {
                        UserID = userId,
                        ProductID = 0
                    }
                });
        }

        public ActionResult AddItem(int userId, int productId)
        {

            Cart cartItem = db.Carts.SingleOrDefault(c => c.UserID == userId && c.ProductID == productId);

            if (cartItem != null)
            {
                cartItem.Amount++;
                db.SaveChanges();
                return RedirectToAction("Index", new {userId = userId});
            }

            Cart item = new Cart
            {
                Product = db.Products.Single(p => p.ProductID == productId),
                User = db.Users.Single(u => u.UserID == userId),
                Amount = 1
            };

            db.Carts.Add(item);
            db.SaveChanges();

            return RedirectToAction("Index", new {userId = userId});
        }

        public ActionResult DeleteItem(int userId, int productId)
        {
            var cart = db.Carts.Include(c => c.Product).Include(c => c.User)
                .Where(c => c.UserID == userId && c.ProductID == productId);

            Cart item = cart.ToList()[0];

            if (item.Amount > 1)
            {
                item.Amount--;
                db.SaveChanges();
                return RedirectToAction("Index", new {userId = userId});
            }

            db.Carts.Remove(item);
            db.SaveChanges();

            return RedirectToAction("Index", new {userId = userId});
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
