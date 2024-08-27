using Domain.Entities;
using Domain.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace webApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetAll()
        {
            var orders = await _orderService.GetAll();
            return Ok(orders);
        }

    
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrderById(int id)
        {
            var order = await _orderService.getOrderById(id);
            if (order == null)
            {
                return NotFound("Order not found.");
            }
            return Ok(order);
        }

     
        [HttpPost]
        public async Task<ActionResult> AddOrder([FromBody] Order order)
        {
            if (ModelState.IsValid)
            {
                await _orderService.AddOrder(order);
                return CreatedAtAction(nameof(GetOrderById), new { id = order.OrderId }, order);
            }
            return BadRequest(ModelState);
        }

      
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateOrder(int id, [FromBody] Order order)
        {
            if (id != order.OrderId)
            {
                return BadRequest("Order ID mismatch");
            }

            var existingOrder = await _orderService.getOrderById(id);
            if (existingOrder == null)
            {
                return NotFound();
            }

            await _orderService.AddOrder(order);
            return NoContent();
        }

  
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrderById(int id)
        {
            var existingOrder = await _orderService.getOrderById(id);
            if (existingOrder == null)
            {
                return NotFound();
            }

            await _orderService.deleteOrderById(id);
            return NoContent();
        }

        


    }
}
