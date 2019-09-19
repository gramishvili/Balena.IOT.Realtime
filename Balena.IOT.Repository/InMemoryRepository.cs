using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Balena.IOT.Entity.Entities;

namespace Balena.IOT.Repository
{
    public class InMemoryRepository<T> : IRepository<T>  where  T : IEntity
    {
        private readonly List<T> _context;
        
        public InMemoryRepository()
        {
            _context = Activator.CreateInstance<List<T>>();
        }
        
        /// <summary>
        /// returns queryable context
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> AsQueryable()
        {
            return _context.AsQueryable();
        }

        /// <summary>
        /// creates new entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task AddAsync(T entity)
        {
            _context.Add(entity);
        }

        /// <summary>
        /// deletes entity by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteByIdAsync(Guid id)
        {
            var item = _context.FirstOrDefault(q => q.Id == id);
            await DeleteAsync(item);
        }

        /// <summary>
        /// deletes entity from storage
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task DeleteAsync(T entity)
        {
            if(entity == default(T))
                return;

            _context.Remove(entity);
        }

        /// <summary>
        /// updates entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task UpdateAsync(T entity)
        {
            if(entity == null)
                throw new ArgumentNullException();
            //Using replace for in memory purposes, that won't be a case on redis or persistent db
            _context.Remove(entity);
            _context.Add(entity);
        }

        /// <summary>
        /// finds entity by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<T> FindByIdAsync(Guid Id)
        {
            return _context.FirstOrDefault(q => q.Id == Id);
        }
    }
}