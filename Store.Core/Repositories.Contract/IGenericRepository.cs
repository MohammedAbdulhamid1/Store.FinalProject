using Store.Core.Entites;
using Store.Core.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Repositories.Contract
{
    public interface IGenericRepository<TEntity,Tkey>where TEntity:BaseEntity<Tkey>
    {
       
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(Tkey id);
        Task<IEnumerable<TEntity>> GetAllWithSpecAsync(ISpecification<TEntity,Tkey> spec);
        Task<TEntity> GetWithSpecAsync(ISpecification<TEntity, Tkey> spec);
        Task<int> GetCountAsync(ISpecification<TEntity, Tkey>spec);
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);


    }
}
