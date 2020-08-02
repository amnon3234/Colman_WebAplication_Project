using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Colman_MVC_WebAplication_Project.Data;
using Colman_MVC_WebAplication_Project.Models;

namespace Colman_MVC_WebAplication_Project.Controllers
{
    public class HomeController : Controller
    {
        private StoreDataBase db = new StoreDataBase();

        // Home Page
        public ActionResult Index()
        {
            var productsList = db.Products.Include(p => p.Category)
                .OrderByDescending(p => p.ProductOrderCount).ToList();
            return View(productsList);
        }

        /**
         * <summary>
         *      Get as an input a category id and a sorting order.     
         * </summary>
         *
         * <returns>
         *      Return a Category view with the chosen category products sorted in the requested way. 
         * </returns>
         */
        public ActionResult CategoryPage(int CategoryId = 1, string sortOrder = "Sort By Popularity")
        {
            var productsList = db.Products.Include(p => p.Category)
                .Where(Product => Product.CategoryID == CategoryId);

            switch (sortOrder)
            {
                case "Sort By Price (low to high)":
                    productsList = productsList.OrderBy(p => p.ProductPrice);
                    break;

                case "Sort By Price (high to low)":
                    productsList = productsList.OrderByDescending(p => p.ProductPrice);
                    break;

                default:
                    productsList = productsList.OrderBy(p => p.ProductOrderCount);
                    break;
            }


            return View(productsList.ToList());
        }

        /**
         * <summary>
         *      Get as an input a product id.     
         * </summary>
         *
         * <returns>
         *      Return a Product view with recommendation list of products.
         * </returns>
         */
        public ActionResult ProductPage(int productId = 10)
        {
            string currUser = Session["user"] as string;
            var products = db.Products.Include(p => p.Category);
            Product currProduct = db.Products.Find(productId);
            List<Product> productsList = new List<Product>();

            // Suggest user a product base on his order history
            if (currUser != null)
            {
                User user = db.Users.FirstOrDefault(u => u.UserName == currUser);
                List<OrderHistory> userOrderHistory = db.OrderHistory.Where(o => o.UserID == user.UserID)
                    .OrderByDescending(o => o.Product.ProductOrderCount).ToList();

                foreach (OrderHistory order in userOrderHistory)
                    if (order.ProductID != currProduct.ProductID)
                    {
                        Product similarProduct = db.Products.FirstOrDefault(p => p.CategoryID == order.Product.CategoryID 
                                                   && p.ProductID != order.ProductID);
                        productsList.Add(similarProduct);
                    }
            }
            else
                productsList.AddRange(products.ToList().Where(product => product.CategoryID == currProduct.CategoryID && product.ProductID != currProduct.ProductID));

            // Insert current product in the first place.
            productsList.Insert(0, currProduct);

            // Make sure there are five products in the list.
            List<Product> temp = db.Products.OrderByDescending(p => p.ProductOrderCount).ToList();
            int i = 0;
            while (productsList.Count < 5)
            {
                if (!productsList.Contains(temp[i]))
                    productsList.Add(temp[i]);
                i++;
            }

            return View(productsList);
        }

        /**
         * <summary>
         *      Get a username and a password and check their Correctness     
         * </summary>
         *
         * <returns>
         *      Redirects to the administrator page if the user is an admin
         *      to the home page if it's a regular user and to itself with a
         *      message if there is something wrong with the username or the
         *      password.
         * </returns>
         */
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            User curUser = db.Users.FirstOrDefault(user => user.UserName == username && user.Password == password);

            if (curUser == null)
                return View(new List<string> { "The username and password you entered did not match our records. Please double-check and try again." });

            Session["User"] = username;

            if (curUser.isEditor)
                return RedirectToAction("index", "Administrator");

            return RedirectToAction("index");
        }

        /**
         * <summary>
         *      Get a username password address and a phone number and register
         *      a new user in the data base.
         * </summary>
         *
         * <returns>
         *      If something is wrong redirect to itself with a message else
         *      redirect to login page.
         * </returns>
         */
        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignUp(string username, string password, string address, int phoneNumber = 0)
        {
            if (username == "" || password == "" || address == "" || phoneNumber == 0)
                return View(new List<string> { "Information is missing" });

            User curUser = db.Users.FirstOrDefault(user => user.UserName == username);

            if (curUser != null)
                return View(new List<string> { "UserName Already exist" });


            User newUser = new User()
            {
                UserName = username,
                Password = password,
                Address = address,
                PhoneNumber = phoneNumber
            };

            db.Users.Add(newUser);
            db.SaveChanges();

            return RedirectToAction("Login");
        }

        // Log-out
        public ActionResult LogOut()
        {
            Session["user"] = null;
            return RedirectToAction("Index");
        }

        /**
         * <returns>
         *      Cart page fill with current user chosen products.
         * </returns>
         */
        public ActionResult UserCart()
        {
            string currUser = Session["user"] as string;
            if (currUser == null)
                return RedirectToAction("Index");
            User user = db.Users.FirstOrDefault(u => u.UserName == currUser);
            var carts = db.Carts.Include(c => c.Product).Include(c => c.User).Where(c => c.UserID == user.UserID);
            return View(carts.ToList());
        }

        public ActionResult AddItemFromHome(int productId)
        {
            string currUser = Session["user"] as string;
            if (currUser == null)
                return RedirectToAction("Index");
            User user = db.Users.FirstOrDefault(u => u.UserName == currUser);
            Cart cartItem = db.Carts.SingleOrDefault(c => c.UserID == user.UserID && c.ProductID == productId);

            if (cartItem != null)
            {
                cartItem.Amount++;
                db.SaveChanges();
                return RedirectToAction("UserCart");
            }

            Cart item = new Cart
            {
                Product = db.Products.Single(p => p.ProductID == productId),
                User = db.Users.Single(u => u.UserID == user.UserID),
                Amount = 1
            };

            db.Carts.Add(item);
            db.SaveChanges();

            return RedirectToAction("UserCart");
        }

        public ActionResult DeleteItemFromCart(int productId)
        {
            string currUser = Session["user"] as string;
            if (currUser == null)
                return RedirectToAction("Index");
            User user = db.Users.FirstOrDefault(u => u.UserName == currUser);
            var cart = db.Carts.Include(c => c.Product).Include(c => c.User)
                .Where(c => c.UserID == user.UserID && c.ProductID == productId);

            Cart item = cart.ToList()[0];

            if (item.Amount > 1)
            {
                item.Amount--;
                db.SaveChanges();
                return RedirectToAction("UserCart");
            }

            db.Carts.Remove(item);
            db.SaveChanges();

            return RedirectToAction("UserCart");
        }

        /**
         * <summary>
         *      Order the current user cart items.
         * </summary>
         */
        public ActionResult Order()
        {
            string currUser = Session["user"] as string;
            if (currUser == null)
                return RedirectToAction("Index");
            User user = db.Users.FirstOrDefault(u => u.UserName == currUser);
            var carts = db.Carts.Include(c => c.Product).Include(c => c.User).Where(c => c.UserID == user.UserID);

            foreach (Cart item in carts.ToList())
            {

                db.Products.Find(item.ProductID).ProductOrderCount += 1;

                db.OrderHistory.Add(new OrderHistory
                {
                    Product = item.Product,
                    User = item.User,
                    Amount = item.Amount,
                    MonthNumber = DateTime.Now.Month
                });

                db.Carts.Remove(item);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // Contact Page
        public ActionResult Contact()
        {
            ViewBag.Message = "Contact-Us";

            return View();
        }
    }
}
 