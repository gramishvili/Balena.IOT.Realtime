using System.Linq;
using System.Threading.Tasks;
using Balena.IOT.Entity.Entities;

namespace Balena.IOT.Repository
{
    public interface IRepository<T> where T : IEntity
    {
        IQueryable<T> AsQueryable();
        Task AddAsync(T entity);
        Task DeleteByIdAsync(long id);
        Task DeleteAsync(T entity);
        Task UpdateAsync(T entity);
        Task<T> FindByIdAsync(long Id);
    }
}