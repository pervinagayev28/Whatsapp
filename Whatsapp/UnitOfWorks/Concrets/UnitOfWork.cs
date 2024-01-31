using ChatAppDatabaseLibrary.Contexts;
using ChatAppModelsLibrary.Models;
using ChatAppModelsLibrary.Models.BaseModels;
using ChatAppModelsLibrary.Models.Concrets;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whatsapp.Repositories.BaseRepositories;
using Whatsapp.Repositories.ConcretRepositories;
using Whatsapp.UnitOfWorks.BaseUnitOfWorks;

namespace Whatsapp.UnitOfWorks.Concrets
{




    internal class UnitOfWork : IUnitOfWork
    {
        //Context
        private readonly ChatAppDb context;
        private Dictionary<Type, object> repositories;


        public UnitOfWork()
        {
            context = new();
            repositories = new();


        }


        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>()
                    where TEntity : class, IPrimaryKey<TKey>
        {
            if (repositories.ContainsKey(typeof(TEntity)))
                return (IGenericRepository<TEntity, TKey>)repositories[typeof(TEntity)];


            var repository = new GenericRepository<TEntity,TKey>(context);
            repositories.Add(typeof(TEntity), repository);
            return repository;
        }

        public async Task Commit() =>
            await context.SaveChangesAsync();


    }
}
