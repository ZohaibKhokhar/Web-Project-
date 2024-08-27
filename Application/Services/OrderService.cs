using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ServiceInterfaces;

namespace Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task AddOrder(Order order)
        {
            await _orderRepository.AddOrder(order);
        }

        public async Task<List<Order>> GetAll()
        {
            return await _orderRepository.GetAll();
        }

        public async Task<int> GetMaxOrderId()
        {
            return await _orderRepository.GetMaxOrderId();
        }

        public async Task<int> GetOrderIdByCustomerId(int id)
        {
            return await _orderRepository.GetOrderIdByCustomerId(id);
        }

        public async Task<Order> getOrderById(int id)
        {
            return await _orderRepository.getOrderById(id);
        }

        public async Task deleteOrderById(int id)
        {
            await _orderRepository.deleteOrderById(id);
        }

        public async Task<int> getCustomerIdByOrderId(int id)
        {
            return await _orderRepository.getCustomerIdByOrderId(id);
        }
    }
}
