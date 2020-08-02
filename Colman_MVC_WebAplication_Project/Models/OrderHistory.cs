
namespace Colman_MVC_WebAplication_Project.Models
{
    /*
     * Many to many connection between the users and their purchased items.
     */
    public class OrderHistory
    {
        // User data
        public int UserID { get; set; }
        public User User { get; set; }

        // Product number
        public int ProductID { get; set; }
        public Product Product { get; set; }

        // How much from each item
        public int Amount { get; set; }

        // When bought 
        public int MonthNumber { get; set; }
    }
}