using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Colman_MVC_WebAplication_Project.Models
{

    public class User
    {
        // User ID
        [Key]
        public int UserID { get; set; }
        
        // User Role
        public bool isEditor { get; set; }

        // User Data
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public int PhoneNumber { get; set; }

        // Many To Many connection ( -> orderHistory and -> cart )
        public IList<Cart> Carts { get; set; }
        public List<OrderHistory> orders { get; set; }
    }
}