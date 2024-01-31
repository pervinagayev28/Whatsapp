using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatAppModelsLibrary.Models.BaseModels;
using Microsoft.AspNetCore.Mvc;

namespace Whatsapp.Repositories.BaseRepositories
{
    public interface IGenericRepository<TEntity, TKey>
        where TEntity : class, IPrimaryKey<TKey>
    {
        Task<IQueryable<TEntity>> GetAll();
        ValueTask<TEntity> Get(TKey Id);

        Task<TEntity> Add(TEntity entity);
        Task<TEntity> Update(TEntity entity);
        Task<TEntity> Delete(TEntity entity);
        Task Commit();

    }
}
