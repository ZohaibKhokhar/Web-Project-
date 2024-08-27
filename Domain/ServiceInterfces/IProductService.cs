using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.ServiceInterfaces
{
    public interface IProductService
    {
        Task Add(Products product);

        Task Update(Products product);

        Task DeleteById(int id);

        Task<Products> GetByName(string name);

        Task UpdateQuantity(int id, int minusQuantity);

        Task<List<Products>> GetAll();

        Task<Products> Get(int id);
    }
}
