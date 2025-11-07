using DataAccessInterface.Repositories;
using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Repositories
{
    public class ClaseRepository:IClaseRepository
    {
        private readonly DbContext Context;
        private bool disposed = false;
        public ClaseRepository(DbContext context)
        {
            this.Context = context;
        }
        protected DbSet<Clase> Set
        {
            get
            {
                return Context.Set<Clase>();
            }
        }

        public void AddAndSave(Clase entity)
        {
            Set.Add(entity);
            Context.SaveChanges();
        }

        public void AddAndSave(IList<Clase> entities)
        {
            foreach (var entity in entities)
            {
                this.AddAndSave(entity);
            }
        }

        public void Delete(Clase entity)
        {
            if (Context.Entry(entity).State == EntityState.Detached)
            {
                Set.Attach(entity);
            }

            Set.Remove(entity);
            Context.SaveChanges();
        }

        public IQueryable<Clase> IncludeAll(params string[] list)
        {
            IQueryable<Clase> queryable = Set;
            var type = typeof(Clase);
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

        public IQueryable<Clase> IncludeAllAnidado(params string[] list)
        {
            IQueryable<Clase> queryable = Set.OfType<Clase>();
            var type = typeof(Clase);

            foreach (var include in list)
            {
                if (include.Contains("."))
                {
                    // Handle nested includes
                    var includeParts = include.Split('.');
                    if (includeParts.Length == 2 && includeParts[0] == "ClasesAlumno" && includeParts[1] == "Alumno")
                    {
                        queryable = queryable.Include(c => c.ClasesAlumno)
                                             .ThenInclude(a => a.Alumno);
                    }
                }
                else
                {
                    // Handle direct includes
                    var property = type.GetProperty(include);
                    if (property != null)
                    {
                        queryable = queryable.Include(include);
                    }
                }
            }

            return queryable;
        }

        public IQueryable<Clase> IncludeAllAnidadoFull(params string[] list)
        {
            IQueryable<Clase> queryable = Set.OfType<Clase>();

            if (list == null || list.Length == 0) return queryable;

            foreach (var include in list.Distinct())
            {
                // EF Core admite rutas con puntos, incluso si hay colecciones en el medio.
                queryable = queryable.Include(include);
            }

            return queryable;
        }

        public IQueryable<Clase> List()
        {
            return Set;
        }

        public void Update(Clase entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            Context.SaveChanges();
        }
    }
}
