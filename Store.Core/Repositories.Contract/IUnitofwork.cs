using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Store.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Repositories.Contract
{
    public interface IUnitofwork
    {
        Task<int> completeAsync();
        IGenericRepository<TEntity,Tkey>Repositories<TEntity,Tkey>() where TEntity:BaseEntity<Tkey>;
    }
}
