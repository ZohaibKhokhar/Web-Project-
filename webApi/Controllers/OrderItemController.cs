using Domain.Entities;
using Domain.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace webApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly IOrderItemService _orderItemService;

        public OrderItemController(IOrderItemService orderItemService)
        {
            _orderItemService = orderItemService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderItem>>> GetAllOrderItems()
        {
            var orderItems = await _orderItemService.GetAllOrderItems();
            return Ok(orderItems);
        }

        [HttpGet("{orderId}")]
        public async Task<ActionResult<IEnumerable<OrderItem>>> GetOrderItemsByOrderId(int orderId)
        {
            var orderItems = await _orderItemService.GetAllByOrderId(orderId);
            if (orderItems == null || orderItems.Count == 0)
            {
                return NotFound("No order items found for the specified order ID.");
            }
            return Ok(orderItems);
        }

        [HttpPost]
        public async Task<ActionResult> AddOrderItem([FromBody] OrderItem orderItem)
        {
            if (ModelState.IsValid)
            {
                await _orderItemService.AddOrderItem(orderItem);
                return CreatedAtAction(nameof(GetOrderItemsByOrderId), new { orderId = orderItem.OrderId }, orderItem);
            }
            return BadRequest(ModelState);
        }

    
        [HttpDelete("{orderId}")]
        public async Task<ActionResult> DeleteOrderItemsByOrderId(int orderId)
        {
            var orderItems = await _orderItemService.GetAllByOrderId(orderId);
            if (orderItems == null || orderItems.Count == 0)
            {
                return NotFound("No order items found for the specified order ID.");
            }

            await _orderItemService.deleteByOrderId(orderId);
            return NoContent();
        }
    }
}
