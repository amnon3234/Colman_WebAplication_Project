using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace Colman_MVC_WebAplication_Project.Models
{
    public class Category
    {
        // Category ID
        [Key]
        public int CategoryID { get; set; }

        // Category Data
        public string CategoryName { get; set; }

        [DisplayName("Image")]
        public string CategoryImagePath { get; set; }

        [DisplayName("Description")]
        public string CategoryDescription { get; set; }

        // Single to many connection with Product
        public IList<Product> Products { get; set; }
    }
}