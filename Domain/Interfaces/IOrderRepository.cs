﻿using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IOrderRepository
    {
        public void AddOrder(Order order);

        public List<Order> GetAll();

        public int GetMaxOrderId();

        public int GetOrderIdByCustomerId(int id);

        public Order getOrderById(int id);

        public void deleteOrderById(int id);

        public int getCustomerIdByOrderId(int id);

    }
}
