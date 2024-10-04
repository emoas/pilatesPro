using DataAccessInterface.Repositories;
using Domain.Alumnos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Repositories
{
    public class AlumnoClaseRepository : IAlumnoClaseRepository
    {
        private readonly DbContext Context;
        private bool disposed = false;
        public AlumnoClaseRepository(DbContext context, IMemoryCache cache)
        {
            this.Context = context;
        }
        protected DbSet<AlumnoClase> Set
        {
            get
            {
                return Context.Set<AlumnoClase>();
            }
        }
        public void AddAndSave(AlumnoClase alumno)
        {
            Set.Add(alumno);
            Context.SaveChanges();
        }

        public void Delete(AlumnoClase alumno)
        {
            if (Context.Entry(alumno).State == EntityState.Detached)
            {
                Set.Attach(alumno);
            }

            Set.Remove(alumno);
            Context.SaveChanges();
        }

        public void Desactivate(AlumnoClase alumno)
        {
            throw new NotImplementedException();
        }

        public IQueryable<AlumnoClase> GetAll()
        {
            return Set;
        }

        public IQueryable<AlumnoClase> IncludeAll(params string[] list)
        {
            IQueryable<AlumnoClase> queryable = Set.OfType<AlumnoClase>();
            var type = typeof(AlumnoClase);
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

        public IQueryable<AlumnoClase> IncludeAllAnidado(params string[] list)
        {
            IQueryable<AlumnoClase> queryable = Set.OfType<AlumnoClase>();
            var type = typeof(AlumnoClase);

            foreach (var include in list)
            {
                if (include.Contains("."))
                {
                    // Handle nested includes
                    var includeParts = include.Split('.');
                    if (includeParts.Length == 2 && includeParts[0] == "Clase" && includeParts[1] == "Actividad")
                    {
                        queryable = queryable.Include(c => c.Clase)
                                             .ThenInclude(a => a.Actividad);
                    }else if (includeParts.Length == 2 && includeParts[0] == "Clase" && includeParts[1] == "Local")
                    {
                        queryable = queryable.Include(c => c.Clase)
                                             .ThenInclude(a => a.Local);
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

        public void Update(AlumnoClase alumno)
        {
            Context.ChangeTracker.DetectChanges();
            Context.Entry(alumno).State = EntityState.Modified;
            Context.SaveChanges();
        }
    }
}
