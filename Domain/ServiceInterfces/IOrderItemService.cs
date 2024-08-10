using Domain.Entities;
namespace Domain.ServiceInterfaces
{
    public interface IOrderItemService
    {
        public void AddOrderItem(OrderItem orderItem);

        public List<OrderItem> GetAllOrderItems();

        public List<OrderItem> GetAllByOrderId(int orderId);

        public void deleteByOrderId(int orderId);


    }
}
