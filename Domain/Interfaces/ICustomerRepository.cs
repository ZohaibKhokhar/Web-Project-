﻿using Domain.Entities;
namespace Domain.Interfaces
{
    public interface ICustomerRepository
    {
        public void AddCustomer(Customer customer);

        public List<Customer> GetAllCustomers();

        public int getLastId();

        public Customer GetCustomerById(int id);

        public List<int> GetCustomerId(string currentUserName);

        public void deleteByCustomerId(int customerId);
    }
}