using DataAccessInterface.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Repositories
{
   public class Repository<T> : IDisposable, IRepository<T> where T : class
    {
        protected DbContext Context { get; }
        private bool disposed = false;

        public Repository(DbContext context)
        {
            this.Context = context;
        }

        protected DbSet<T> Set
        {
            get
            {
                return Context.Set<T>();
            }
        }

        public void AddAndSave(T entity)
        {
            Set.Add(entity);
            Context.SaveChanges();
        }

        public void AddAndSave(IList<T> entities)
        {
            foreach (var entity in entities)
            {
                this.AddAndSave(entity);
            }
        }

        public void Update(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            Context.SaveChanges();
        }

        public void Delete(T entity)
        {
            if (Context.Entry(entity).State == EntityState.Detached)
            {
                Set.Attach(entity);
            }

            Set.Remove(entity);
            Context.SaveChanges();
        }

        public IQueryable<T> List()
        {
            return Set;
        }

        public IQueryable<T> IncludeAll(params string[] list)
        {
            IQueryable<T> queryable = Set;
            var type = typeof(T);
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                if (list.Contains(property.Name))
                {
                    queryable = queryable.Include(property.Name);
                }
            }
            return queryable;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
