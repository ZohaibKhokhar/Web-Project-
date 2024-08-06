using WebApplication1.Models;

namespace WebApplication1.Models
{
    public interface IOrderItemService
    {
        public void AddOrderItem(OrderItem orderItem);

        public List<OrderItem> GetAllOrderItems();

        public List<OrderItem> GetAllByOrderId(int orderId);

        public void deleteByOrderId(int orderId);


    }
}
