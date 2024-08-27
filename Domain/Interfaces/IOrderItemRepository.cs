using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IOrderItemRepository
    {
        Task AddOrderItem(OrderItem orderItem);

        Task<List<OrderItem>> GetAllOrderItems();

        Task<List<OrderItem>> GetAllByOrderId(int orderId);

        Task deleteByOrderId(int orderId);
    }
}
