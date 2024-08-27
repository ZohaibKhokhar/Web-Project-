using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ServiceInterfaces;

namespace Application.Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IOrderItemRepository _orderItemRepository;

        public OrderItemService(IOrderItemRepository orderItemRepository)
        {
            _orderItemRepository = orderItemRepository;
        }

        public async Task AddOrderItem(OrderItem orderItem)
        {
            await _orderItemRepository.AddOrderItem(orderItem);
        }

        public async Task<List<OrderItem>> GetAllOrderItems()
        {
            return await _orderItemRepository.GetAllOrderItems();
        }

        public async Task<List<OrderItem>> GetAllByOrderId(int orderId)
        {
            return await _orderItemRepository.GetAllByOrderId(orderId);
        }

        public async Task deleteByOrderId(int orderId)
        {
            await _orderItemRepository.deleteByOrderId(orderId);
        }
    }
}
