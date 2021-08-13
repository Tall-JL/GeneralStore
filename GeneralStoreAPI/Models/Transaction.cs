using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GeneralStoreAPI.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }
        
        [Required]
        [ForeignKey(nameof(Customer))]
        public int? Id { get; set; }
        public virtual Customer Customer { get; set; }

        [Required]
        [ForeignKey(nameof(Product))]
        public string SKU { get; set; }
        public virtual Product Product { get; set; }

        [Required]
        public int ItemCount { get; set; }
        public DateTime DateOfTransaction { get { return DateTime.Now; } }
       //public virtual List<Product> Products { get; set; } = new List<Product>();
        //public virtual List<Customer> Customers { get; set; } = new List<Customer>();
    }
}