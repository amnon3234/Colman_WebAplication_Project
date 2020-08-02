
namespace Colman_MVC_WebAplication_Project.Models
{

    /**
     * User's cart
     * Many to many class between the users and their chosen products
     */
    public class Cart
    {
        // User Data
        public int UserID { get; set; }
        public User User { get; set; }

        // Product Data
        public int ProductID { get; set; }
        public Product Product { get; set; }

        // How much from each item 
        public int Amount { get; set; }
    }
}