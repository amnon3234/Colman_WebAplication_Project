using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Colman_MVC_WebAplication_Project.Models
{
    public class Product
    {
        // Product ID
        [Key]
        public int ProductID { get; set; }

        // Product Data
        [DisplayName("Name")]
        public string ProductName { get; set; }

        [DisplayName("Image")]
        public string ProductImagePath { get; set; }

        [DisplayName("Gender pref")]
        public string Gender { get; set; }

        [DisplayName("Description")]
        public string ProductDescription { get; set; }

        [DisplayName("Price")]
        public double ProductPrice { get; set; }

        [DisplayName("Order Amount")]
        public int ProductOrderCount { get; set; }

        // Single To Many Connection ( -> Category )
        public int CategoryID { get; set; }
        public Category Category { get; set; }

        // Many To Many Connection ( -> Cart and -> Order )
        public List<Cart> Carts { get; set; }
        public List<OrderHistory> orders { get; set; }
    }
}