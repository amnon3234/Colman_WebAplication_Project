using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Colman_MVC_WebAplication_Project.Data;
using Colman_MVC_WebAplication_Project.Models;

namespace Colman_MVC_WebAplication_Project.Controllers
{
    public class AdministratorController : Controller
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

        public ActionResult Index()
        {
            if (!this.checkIfAdmin())
                return RedirectToAction("index", "Home");

            return View();
        }

        public ActionResult Search( int userID , int categoryID , int monthNumber)
        {

            if (!this.checkIfAdmin())
                return RedirectToAction("index", "Home");

            var orders = db.OrderHistory.Include(o => o.Product)
                .Include(o => o.User).Where(order => order.UserID == userID && order.MonthNumber == monthNumber
                 && order.Product.CategoryID == categoryID).ToList();

            return View(orders.Count > 0 ? orders :
                    new List<OrderHistory>()
                    {
                        new OrderHistory()
                        {
                            UserID = userID,
                            ProductID = 0
                        }
                    });
        }
    }
}