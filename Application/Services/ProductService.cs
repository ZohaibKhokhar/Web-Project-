using Domain.Entities;
using Domain.ServiceInterfaces;
using Domain.Interfaces;
namespace Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public void Add(Products product)
        {
        
            if (product.Price > 0)
            {
                _productRepository.Add(product);
            }
            else
            {
                throw new ArgumentException("Price must be greater than zero.");
            }
        }

        public void Update(Products product)
        {
       
            _productRepository.Update(product);
        }

        public void DeleteById(int id)
        {
     
            _productRepository.DeleteById(id);
        }

        public Products GetByName(string name)
        {
            return _productRepository.GetByName(name);
        }

        public void UpdateQuantity(int id, int minusQuantity)
        {
            var product = _productRepository.Get(id);
            if (product != null && product.Quantity >= minusQuantity)
            {
                _productRepository.UpdateQuantity(id, minusQuantity);
            }
            else
            {
                throw new ArgumentException("Insufficient quantity");
            }
        }

        public List<Products> GetAll()
        {
            // Maybe add sorting or filtering logic here
            return _productRepository.GetAll();
        }

        public Products Get(int id)
        {
            return _productRepository.Get(id);
        }
    }
}
