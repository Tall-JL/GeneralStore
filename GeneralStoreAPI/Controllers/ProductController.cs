﻿using GeneralStoreAPI.Models;
using GeneralStoreAPI.Models.ProductModels;
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
                Name = product.Name,
                Cost = product.Cost,
                NumberInInventory = product.NumberInInventory,
               
            };

            var transaction = await _context.Transactions.FindAsync(product.TransactionID);

            if (transaction != null)
            {
               
                productEntity.Transactions.Add(transaction);
            }

            
            _context.Products.Add(productEntity);

            if (await _context.SaveChangesAsync() > 0)
            {
                ((System.Data.Entity.Validation.DbEntityValidationException)$Exception).EntityValidationErrors;
                return Ok();
            }
            return InternalServerError();
        }

        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            var products = await _context.Products.ToListAsync();

            return Ok(products);
        }

        [HttpGet]
        public async Task<IHttpActionResult> Get(int id)
        {
            if (id < 1)
            {
                return BadRequest();
            }

            var product = await _context.Products.FindAsync(id);

            if (product is null)
            {
                return NotFound();
            }
            return Ok(product);
        }
    }
}