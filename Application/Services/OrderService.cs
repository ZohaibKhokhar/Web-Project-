using Domain.Entities;
using Domain.Interfaces;
using Domain.ServiceInterfaces;

namespace Application.Services
{
    public class OrderService :IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public void AddOrder(Order order)
        {
            _orderRepository.AddOrder(order);
        }

        public List<Order> GetAll()
        {
            return _orderRepository.GetAll();
        }

        public int GetMaxOrderId()
        {
            return _orderRepository.GetMaxOrderId();
        }

        public int GetOrderIdByCustomerId(int id)
        {
            return _orderRepository.GetOrderIdByCustomerId(id);
        }

        public Order getOrderById(int id)
        {
            return _orderRepository.getOrderById(id);
        }

        public void deleteOrderById(int id)
        {
            _orderRepository.deleteOrderById(id);
        }

        public int getCustomerIdByOrderId(int id)
        {
            return _orderRepository.getCustomerIdByOrderId(id);
        }
    }
}
