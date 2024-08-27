using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task Add(Products product)
        {
            if (product.Price > 0)
            {
                await _productRepository.Add(product);
            }
            else
            {
                throw new ArgumentException("Price must be greater than zero.");
            }
        }

        public async Task Update(Products product)
        {
            await _productRepository.Update(product);
        }

        public async Task DeleteById(int id)
        {
            await _productRepository.DeleteById(id);
        }

        public async Task<Products> GetByName(string name)
        {
            return await _productRepository.GetByName(name);
        }

        public async Task UpdateQuantity(int id, int minusQuantity)
        {
            var product = await _productRepository.Get(id);
            if (product != null && product.Quantity >= minusQuantity)
            {
                await _productRepository.UpdateQuantity(id, minusQuantity);
            }
            else
            {
                throw new ArgumentException("Insufficient quantity");
            }
        }

        public async Task<List<Products>> GetAll()
        {
            // Maybe add sorting or filtering logic here
            return await _productRepository.GetAll();
        }

        public async Task<Products> Get(int id)
        {
            return await _productRepository.Get(id);
        }
    }
}
