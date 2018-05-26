using Common.Domain.Interfaces;
using Common.Infrastructure.Log;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Common.Infrastructure.ORM.Repositories
{
    public class RepositoryBase<U> : IRepository
    {
        protected DbContext ctx;
        protected ILog log;
        protected string connectionString;

        public RepositoryBase(IUnitOfWork<U> unitOfWork, ILog log)
        {
            this.ctx = unitOfWork as DbContext;
            this.log = log;
        }

        public IRepository<T2> NewInstance<T2>() where T2 : class
        {
            var unitOfWork = this.ctx as IUnitOfWork<U>;
            return new Repository<T2, U>(unitOfWork, this.log);

        }

        public IRepository<T2> NewInstanceWithNewContext<T2>() where T2 : class
        {
            var log = FactoryLog.GetInstace();
            var newCtx = (T2)Activator.CreateInstance(typeof(T2), log);
            var unitOfWork = newCtx as IUnitOfWork<U>;
            return new Repository<T2, U>(unitOfWork, this.log);


        }
        public IEnumerable<dynamic> ExecuteDynamicQuery(string commandText, object parameters = null, CommandType commandType = CommandType.StoredProcedure, bool MARS = false)
        {
            if (MARS)
                return ctx.Database.ExecuteReaderMARS(commandText, this.connectionString, parameters, commandType);

            return ctx.Database.ExecuteReader(commandText, this.connectionString, parameters, commandType);
        }

        public int ExecuteCommand(string commandText, object parameters = null, CommandType commandType = CommandType.StoredProcedure)
        {
            return ctx.Database.ExecuteNonQuery(commandText, connectionString, parameters, commandType);
        }

        public void Commit()
        {
            this.ctx.SaveChanges();
        }

        public void Commit<T2>(T2 entity)
        {
            try
            {
                this.ctx.SaveChanges();
            }
            catch (OptimisticConcurrencyException)
            {
                ((IObjectContextAdapter)this.ctx).ObjectContext.Refresh(RefreshMode.ClientWins, entity);
                this.ctx.SaveChanges();
            }


        }

        public void ValidateOnSave(bool enabled)
        {
            this.ctx.Configuration.ValidateOnSaveEnabled = enabled;
        }



        #region async

        public Task<List<T2>> ToListAsync<T2>(IQueryable<T2> source)
        {
            return source.ToListAsync<T2>();
        }

        public Task<T2> SingleOrDefaultAsync<T2>(IQueryable<T2> source)
        {
            return source.SingleOrDefaultAsync<T2>();
        }

        public Task<int> CountAsync<T2>(IQueryable<T2> source)
        {
            return source.CountAsync<T2>();
        }

        public Task<decimal> SumAsync<T2>(IQueryable<T2> source, Expression<Func<T2,decimal>> selector)
        {
            return source.SumAsync<T2>(selector);
        }
            
        public Task<int> CommitAsync()
        {
            return this.ctx.SaveChangesAsync();
        }

        #endregion


        public void Dispose()
        {
            if (this.ctx != null)
                this.ctx.Dispose();
        }


    }
}
