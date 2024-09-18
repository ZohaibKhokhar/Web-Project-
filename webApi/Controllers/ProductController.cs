using Domain.Entities;
using Domain.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace webApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Products>>> GetAll()
        {
            var products = await _productService.GetAll();
            return Ok(products);
        }

      
        [HttpGet("{id}")]
        public async Task<ActionResult<Products>> Get(int id)
        {
            var product = await _productService.Get(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

     
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] Products product)
        {
            if (ModelState.IsValid)
            {
                await _productService.Add(product);
                return Created();
            }
            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] Products product)
        {
            if (id != product.ID)
            {
                return BadRequest("Product ID mismatch");
            }

            var existingProduct = await _productService.Get(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            await _productService.Update(product);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var product = await _productService.Get(id);
            if (product == null)
            {
                return NotFound();
            }

            await _productService.DeleteById(id);
            return NoContent();
        }

        [HttpPut("UpdateQuantity/{id}")]
        public async Task<ActionResult> UpdateQuantity(int id, [FromBody] int minusQuantity)
        {
            var product = await _productService.Get(id);
            if (product == null)
            {
                return NotFound();
            }

            await _productService.UpdateQuantity(id, minusQuantity);
            return NoContent();
        }
    }
}
