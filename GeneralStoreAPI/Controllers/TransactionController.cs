using GeneralStoreAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using PagedList;

namespace GeneralStoreAPI.Controllers
{
    public class TransactionController : ApiController
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();


        [HttpPost]
        public async Task<IHttpActionResult> Post(Transaction transaction)
        {
            if (!ModelState.IsValid || transaction is null)
            {
                return BadRequest();
            }

            var customer = await _context.Customers.FindAsync(transaction.Id);
            if (customer != null)
            {
                //transaction.Customers.Add(customer);
                customer.Transactions.Add(transaction);
            }

            var product = await _context.Products.FindAsync(transaction.SKU);
            if (product != null)
            {
                if (product.IsInStock == true && product.NumberInInventory >= transaction.ItemCount)
                {
                    var productLeft = product.NumberInInventory - transaction.ItemCount;
                    product.NumberInInventory = productLeft;
                }
            }

            _context.Transactions.Add(transaction);

            if (await _context.SaveChangesAsync() > 0)
            {
                return Ok();
            }
            return InternalServerError();
        }


        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            var transactions = await _context.Transactions.ToListAsync();

            return Ok(transactions);
        }

        //[HttpGet]
        //public async Task<IHttpActionResult> GetAll([FromUri] int pageNumber)
        //{
        //    var transactions = await _context.Transactions.ToListAsync();

        //    if (pageNumber >= 1 && pageNumber != default)
        //    {
        //        int pageSize = 5;
        //        var result = transactions.Skip(pageNumber * pageSize).Take(pageSize);
        //        foreach (var trans in result)
        //        {
        //            //return View(transactions.ToPagedList(pageNumber, pageSize));
                    
        //            return Ok($"{trans.Id}");

        //        }
        //    }
        //    return NotFound();

        //}

        [HttpGet]
        public async Task<IHttpActionResult> Get([FromUri] int id)
        {
            if (id < 1)
            {
                return BadRequest();

            }
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction is null)
            {
                return NotFound();
            }
            return Ok(transaction);
        }

    }
}
