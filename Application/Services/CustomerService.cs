using Domain.Entities;
using Domain.ServiceInterfaces;
using Domain.Interfaces;


namespace Application.Services
{
    public class CustomerService:ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public void AddCustomer(Customer customer)
        {

            if (customer != null && !string.IsNullOrEmpty(customer.Name))
            {
                _customerRepository.AddCustomer(customer);
            }
            else
            {
                throw new ArgumentException("Customer details are invalid.");
            }
        }

        public List<Customer> GetAllCustomers()
        {
       
            return _customerRepository.GetAllCustomers();
        }

        public int getLastId()
        {
 
            return _customerRepository.getLastId();
        }

        public Customer GetCustomerById(int id)
        {
  
            var customer = _customerRepository.GetCustomerById(id);
            if (customer == null)
            {
                throw new KeyNotFoundException("Customer not found.");
            }
            return customer;
        }

        public List<int> GetCustomerId(string currentUserName)
        {
            // Business logic to get customer IDs by username
            if (!string.IsNullOrEmpty(currentUserName))
            {
                return _customerRepository.GetCustomerId(currentUserName);
            }
            else
            {
                throw new ArgumentException("Username cannot be empty.");
            }
        }

        public void deleteByCustomerId(int customerId)
        {
            // Business logic before deleting a customer by ID
            var customer = _customerRepository.GetCustomerById(customerId);
            if (customer != null)
            {
                _customerRepository.deleteByCustomerId(customerId);
            }
            else
            {
                throw new KeyNotFoundException("Customer not found.");
            }
        }
    }
}
