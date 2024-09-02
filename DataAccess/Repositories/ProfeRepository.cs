using DataAccessInterface.Repositories;
using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Repositories
{
    public class ProfeRepository : IProfeRepository
    {
        private readonly DbContext Context;
        private bool disposed = false;
        public ProfeRepository(DbContext context)
        {
            this.Context = context;
        }
        protected DbSet<User> Set
        {
            get
            {
                return Context.Set<User>();
            }
        }
        public void AddAndSave(Profesor profe)
        {
            foreach (Actividad actividad in profe.Actividades)
            {
                if (actividad.Id != 0)
                    Context.Attach(actividad);
            }
            foreach (Local local in profe.Locales)
            {
                if (local.Id != 0)
                    Context.Attach(local);
            }

            Set.Add(profe);
            Context.SaveChanges();
        }

        public void Update(Profesor entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            Context.SaveChanges();
        }

        public List<Profesor> getProfesores()
        {
            return Set.OfType<Profesor>().Where(p => EF.Property<string>(p, "Discriminator") == "Profesor").ToList();
        }

        public void Delete(Profesor entity)
        {
            if (Context.Entry(entity).State == EntityState.Detached)
            {
                Set.Attach(entity);
            }

            Set.Remove(entity);
            Context.SaveChanges();
        }
        public IQueryable<Profesor> IncludeAll(params string[] list)
        {
            IQueryable<Profesor> queryable = Set.OfType<Profesor>().Where(p => EF.Property<string>(p, "Discriminator") == "Profesor");
            var type = typeof(Profesor);
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

        public void Desactivate(Profesor profe)
        {
            throw new NotImplementedException();
        }
    }
}
