using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.ServiceInterfaces
{
    public interface ICustomerService
    {
        Task AddCustomer(Customer customer);

        Task<List<Customer>> GetAllCustomers();

        Task<int> getLastId();

        Task<Customer> GetCustomerById(int id);

        Task<List<int>> GetCustomerId(string currentUserName);

        Task deleteByCustomerId(int customerId);
    }
}
