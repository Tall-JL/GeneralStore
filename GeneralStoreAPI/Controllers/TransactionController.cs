using GeneralStoreAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

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
                transaction.Customers.Add(customer);
            }

            var product = await _context.Products.FindAsync(transaction.SKU);
            if (product != null)
            {
                if (product.IsInStock == true && product.NumberInInventory >= transaction.ItemCount)
                {
                    _context.Products.Remove(product);
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
