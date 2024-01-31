using ChatAppModelsLibrary.Models;
using ChatAppModelsLibrary.Models.BaseModels;
using ChatAppModelsLibrary.Models.Concrets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whatsapp.Repositories.BaseRepositories;

namespace Whatsapp.UnitOfWorks.BaseUnitOfWorks
{
    public interface IUnitOfWork
    {
        IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>()
             where TEntity : class, IPrimaryKey<TKey>;

        Task Commit();
    }
}
