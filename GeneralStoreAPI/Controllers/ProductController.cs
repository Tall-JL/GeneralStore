using GeneralStoreAPI.Models;
using GeneralStoreAPI.Models.ProductModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace GeneralStoreAPI.Controllers
{
    public class ProductController : ApiController
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        [HttpPost]
        public async Task<IHttpActionResult> Post(ProductCreate product)
        {
            if (!ModelState.IsValid || product is null)
            {
                return BadRequest();
            }

            var productEntity = new Product
            {
                SKU = product.SKU,
                Name = product.Name,
                Cost = product.Cost,
                NumberInInventory = product.NumberInInventory,

            };

            _context.Products.Add(productEntity);

            if (await _context.SaveChangesAsync() > 0)
            {
                return Ok();
            }

            return InternalServerError();
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            var products = await _context.Products.ToListAsync();

            return Ok(products);
        }

        [Route("GetOutOfStock"), HttpGet]
        public async Task<IHttpActionResult> GetOutOfStock()
        {

            foreach (var product in _context.Products)
            {
                if (product.IsInStock == false)
                {
                    //var result = await _context.Products.FindAsync(product);
                    return Ok(product);
                }
            }
            return null;
        }

        [HttpGet]
        public async Task<IHttpActionResult> Get(string sku)
        {
            if (sku is null)
            {
                return BadRequest();
            }

            var product = await _context.Products.FindAsync(sku);

            if (product is null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPut]
        public async Task<IHttpActionResult> Put([FromBody] Product newProductData, [FromUri] string sku)
        {
            if (newProductData.SKU != sku || newProductData is null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await _context.Products.FindAsync(sku);

            if (product is null)
            {
                return NotFound();
            }

            product.SKU = newProductData.SKU;
            product.Name = newProductData.Name;
            product.Cost = newProductData.Cost;
            product.NumberInInventory = newProductData.NumberInInventory;

            if (await _context.SaveChangesAsync() > 0)
            {
                return Ok();
            }

            return InternalServerError();
        }
    }
}
