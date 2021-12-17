using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyCrudProducts.Models;
using MyCrudProducts.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCrudProducts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IProductRepository repository;
        public ProductController(IProductRepository _context)
        {
            repository = _context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var produtos = await repository.GetAll();
            if (produtos == null)
            {
                return BadRequest();
            }
            return Ok(produtos.ToList());
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var produto = await repository.GetById(id);
            if (produto == null)
            {
                return NotFound($"O produto com o id {id} não foi encontrado");
            }
            return Ok(produto);
        }

        // POST api/<controller>  
        [HttpPost]
        public async Task<IActionResult> PostProduct([FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest("Produto é nulo");
            }
            await repository.Insert(product);
            return Ok(product);
        }

        [HttpPut]
        public async Task<IActionResult> PutProduct(Product product)
        {
            try
            {
                await repository.Update(product.ProductId, product);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return Ok("Atualização realizada com sucesso!");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            var produto = await repository.GetById(id);
            if (produto == null)
            {
                return NotFound($"O produto com o id {id} não foi encontrado");
            }
            await repository.Delete(id);
            return Ok(produto);
        }
    }
}
