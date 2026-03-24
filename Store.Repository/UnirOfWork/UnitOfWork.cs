using Store.Core.Entites;
using Store.Core.Entites.Context;
using Store.Core.Repositories.Contract;
using Store.Repository.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Composing.CompositionExtensions;

namespace Store.Repository.UnirOfWork
{
    public class UnitOfWork : IUnitofwork
    {
        private readonly StoreDbContext context;
        private readonly Hashtable _repositories;

        public UnitOfWork(StoreDbContext context)
        {
            this.context = context;
            _repositories = new Hashtable();
        }
        public async Task<int> completeAsync()
        {
         return await context.SaveChangesAsync();   
        }

        public IGenericRepository<TEntity, Tkey>Repositories<TEntity, Tkey>() where TEntity : BaseEntity<Tkey>
        {
            var type = typeof(TEntity).Name;
            if (!_repositories.ContainsKey(type))
            {
                var repository = new GenericRepository<TEntity, Tkey>(context);

                _repositories.Add(type, repository);
            }
                return _repositories[type] as IGenericRepository<TEntity,Tkey>;
            

        }


    }
}
