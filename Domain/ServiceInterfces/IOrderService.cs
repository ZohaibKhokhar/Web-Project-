using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.ServiceInterfaces
{
    public interface IOrderService
    {
        Task AddOrder(Order order);

        Task<List<Order>> GetAll();

        Task<int> GetMaxOrderId();

        Task<int> GetOrderIdByCustomerId(int id);

        Task<Order> getOrderById(int id);

        Task deleteOrderById(int id);

        Task<int> getCustomerIdByOrderId(int id);
    }
}
