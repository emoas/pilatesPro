using DataAccessInterface.Repositories;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Repositories
{
    public class ActividadRepository : IActividadRepository
    {
        private readonly DbContext Context;
        private bool disposed = false;
        public ActividadRepository(DbContext context, IMemoryCache cache)
        {
            this.Context = context;
        }
        protected DbSet<Actividad> Set
        {
            get
            {
                return Context.Set<Actividad>();
            }
        }
        public void AddAndSave(Actividad actividad)
        {
            foreach (Profesor profesor in actividad.Profesores)
            {
                if (profesor.Id != 0)
                    Context.Attach(profesor);
            }
            foreach (Local local in actividad.Locales)
            {
                if (local.Id != 0)
                    Context.Attach(local);
            }
            foreach (Clase clase in actividad.Clases)
            {
                if (clase.Id != 0)
                    Context.Attach(clase);
            }
            Set.Add(actividad);
            Context.SaveChanges();
        }

        public void Desactivate(Actividad actividad)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Actividad> GetAll()
        {
            return Set;
        }
        public IQueryable<Actividad> IncludeAll(params string[] list)
        {
            IQueryable<Actividad> queryable = Set;
            var type = typeof(Actividad);
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
        public void Update(Actividad actividad)
        {
            Context.ChangeTracker.DetectChanges();
            Context.Entry(actividad).State = EntityState.Modified;
            Context.SaveChanges();
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
