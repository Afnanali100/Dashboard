﻿using PlantifyApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantifyApp.Core.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        public Task<IReadOnlyList<T>> GetAllAsync();


        public Task<T> GetByIdAsync(int id);
        Task Add(T entity);
        void Update(T entity);

        void Delete(T entity);
        public Task<IReadOnlyList<Posts>> GetAllPostsForSpecifcUser(string user_id);
        public Task<IReadOnlyList<Plants>> GetAllPlantsDetailsForSpecifcUser(string user_id);

    }
}
