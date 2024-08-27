using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.ServiceInterfaces;
using Domain.Interfaces;

namespace Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task AddCustomer(Customer customer)
        {
            if (customer != null && !string.IsNullOrEmpty(customer.Name))
            {
                await _customerRepository.AddCustomer(customer);
            }
            else
            {
                throw new ArgumentException("Customer details are invalid.");
            }
        }

        public async Task<List<Customer>> GetAllCustomers()
        {
            return await _customerRepository.GetAllCustomers();
        }

        public async Task<int> getLastId()
        {
            return await _customerRepository.getLastId();
        }

        public async Task<Customer> GetCustomerById(int id)
        {
            var customer = await _customerRepository.GetCustomerById(id);
            if (customer == null)
            {
                throw new KeyNotFoundException("Customer not found.");
            }
            return customer;
        }

        public async Task<List<int>> GetCustomerId(string currentUserName)
        {
            if (!string.IsNullOrEmpty(currentUserName))
            {
                return await _customerRepository.GetCustomerId(currentUserName);
            }
            else
            {
                throw new ArgumentException("Username cannot be empty.");
            }
        }

        public async Task deleteByCustomerId(int customerId)
        {
            var customer = await _customerRepository.GetCustomerById(customerId);
            if (customer != null)
            {
                await _customerRepository.deleteByCustomerId(customerId);
            }
            else
            {
                throw new KeyNotFoundException("Customer not found.");
            }
        }
    }
}
