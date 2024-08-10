﻿using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IProductRepository
    {
        public void Add(Products product);
        public void Update(Products product);

        public void DeleteById(int id);

        public Products GetByName(string name);
        public void UpdateQuantity(int id, int minusQuantity);

        public List<Products> GetAll();

        public Products Get(int id);



    }
}
