using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Store.Core.Entites;
using Store.Core.Entites.Context;
using Store.Core.Repositories.Contract;
using Store.Core.Specification;
using Store.Repository.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;

namespace Store.Repository.Repositories
{
    public class GenericRepository<TEntity, Tkey> : IGenericRepository<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {
        private readonly StoreDbContext context;
        public GenericRepository(StoreDbContext context)
        {
            this.context = context;
        }

       

        public async Task AddAsync(TEntity entity)
        
         =>  await context.Set<TEntity>().AddAsync(entity);
        

        public void Delete(TEntity entity)
        
         =>  context.Set<TEntity>().Remove(entity);


        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            if (typeof(TEntity) == typeof(Product))
            {
                return (IEnumerable<TEntity>)await context.Products.OrderBy(p=>p.Name).Include(p => p.Brand).Include(p => p.Type).ToListAsync();
            }
            return await context.Set<TEntity>().ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllWithSpecAsync(ISpecification<TEntity, Tkey> spec)
        {
           return  await ApplySpecification(spec).ToListAsync();

        }

        public async Task<TEntity> GetByIdAsync(Tkey id)

        {
            if (typeof(TEntity) == typeof(Product))
                return  await context.Products.Include(p => p.Brand).Include(p => p.Type).FirstOrDefaultAsync(p =>p.Id==id as int?)as TEntity;
            return await context.Set<TEntity>().FindAsync(id);
        }

        public async Task<int> GetCountAsync(ISpecification<TEntity, Tkey> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        public async Task<TEntity> GetWithSpecAsync(ISpecification<TEntity, Tkey> spec)
        {
             return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        public void Update(TEntity entity)
        
             => context.Set<TEntity>().Update(entity);
        private  IQueryable<TEntity> ApplySpecification(ISpecification<TEntity,Tkey> spec)
        {

            return  SpecificationEvaluator<TEntity, Tkey>.GetQuery(context.Set<TEntity>(), spec);
        }
        
    }
}
