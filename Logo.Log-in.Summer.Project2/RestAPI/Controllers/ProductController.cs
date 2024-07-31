using Microsoft.AspNetCore.Mvc;
using BusinessAPI;
using DatabaseAPI;
using System.Threading.Tasks;
using System.Collections.Generic;
using StackExchange.Profiling;

namespace RestAPI.Controllers
{
   
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        
        [HttpGet("LIST")]
        public async Task<ActionResult<IEnumerable<Product>>> Get()
        {
            using (MiniProfiler.Current.Step("Get All Products"))
            {
                var products = await _productService.GetAllProductsAsync();
                return Ok(products);
            }
        }

       
        [HttpGet("IDSEARCH{id}")]
        public async Task<ActionResult<Product>> Get(int id)
        {
            using (MiniProfiler.Current.Step($"Get Product By ID: {id}"))
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null)
                {
                    return NotFound();
                }
                return Ok(product);
            }
        }

       
        [HttpPost("CREATE")]
        public async Task<ActionResult<Product>> Post(Product product)
        {
            using (MiniProfiler.Current.Step("Create New Product"))
            {
                await _productService.AddProductAsync(product);
                return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
            }
        }

       
        [HttpPut("UPDATE{id}")]
        public async Task<IActionResult> Put(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            using (MiniProfiler.Current.Step($"Update Product ID: {id}"))
            {
                var existingProduct = await _productService.GetProductByIdAsync(id);
                if (existingProduct == null)
                {
                    return NotFound();
                }

                await _productService.UpdateProductAsync(product);
                return NoContent();
            }
        }

        
        [HttpDelete("DELETE{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            using (MiniProfiler.Current.Step($"Delete Product ID: {id}"))
            {
                var existingProduct = await _productService.GetProductByIdAsync(id);
                if (existingProduct == null)
                {
                    return NotFound();
                }

                await _productService.DeleteProductAsync(id);
                return NoContent();
            }
        }
    }
}
