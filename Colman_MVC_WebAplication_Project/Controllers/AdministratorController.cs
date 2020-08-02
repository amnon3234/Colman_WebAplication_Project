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

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search( int userID , int categoryID , int monthNumber)
        {
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