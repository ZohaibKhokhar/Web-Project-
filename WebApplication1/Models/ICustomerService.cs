using WebApplication1.Models;
using WebApplication1.Models;

namespace WebApplication1.Models
{
    public interface ICustomerService
    {
        public void AddCustomer(Customer customer);

        public List<Customer> GetAllCustomers();

        public int getLastId();

        public Customer GetCustomerById(int id);

        public List<int> GetCustomerId(string currentUserName);

        public void deleteByCustomerId(int customerId);
    }
}
