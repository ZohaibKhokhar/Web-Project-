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

        public void AddOrderItem(OrderItem orderItem)
        {
            _orderItemRepository.AddOrderItem(orderItem);
        }

        public List<OrderItem> GetAllOrderItems()
        {
            return _orderItemRepository.GetAllOrderItems();
        }

        public List<OrderItem> GetAllByOrderId(int orderId)
        {
            return _orderItemRepository.GetAllByOrderId(orderId);
        }

        public void deleteByOrderId(int orderId)
        {
            _orderItemRepository.deleteByOrderId(orderId);
        }
    }
}
