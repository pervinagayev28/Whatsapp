using ChatAppDatabaseLibrary.Contexts;
using ChatAppModelsLibrary.Models.BaseModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whatsapp.Repositories.BaseRepositories;

namespace Whatsapp.Repositories.ConcretRepositories
{
    internal class GenericRepository<TEntity, TKey>
                     : IGenericRepository<TEntity, TKey>
                           where TEntity : class, IPrimaryKey<TKey>

    {

        private readonly DbContext context;
        private readonly DbSet<TEntity> set;
        public GenericRepository(DbContext context)
        {
            this.context = context;
            set = context.Set<TEntity>();

        }
        //Crud Functionally
        public async Task<IQueryable<TEntity>> GetAll() =>
             set;

        public async Task<TEntity> Add(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            await set.AddAsync(entity);
            return entity;
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            set.Update(entity);
            return entity;
        }

        public async Task<TEntity> Delete(TEntity entity)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            set.Remove(entity);
            return entity;
        }

        public async ValueTask<TEntity> Get(TKey Id)=>
            await set.FindAsync(Id);
        

        //Commit 
        public async Task Commit() =>
            await context.SaveChangesAsync();

      
    }
}
