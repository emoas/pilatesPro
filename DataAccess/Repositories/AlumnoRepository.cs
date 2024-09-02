using DataAccessInterface.Repositories;
using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Repositories
{
    public class AlumnoRepository : IAlumnoRepository
    {
        private readonly DbContext Context;
        private bool disposed = false;
        public AlumnoRepository(DbContext context)
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
        public void AddAndSave(Alumno alumno)
        {
            if (alumno.PatologíasQuePresenta != null)
            {
                foreach (Patologia patologia in alumno.PatologíasQuePresenta)
                {
                    if (patologia.Id != 0)
                        Context.Attach(patologia);
                }
            }
            if (alumno.ClasesFijas != null)
            {
                foreach (ClaseFija claseFija in alumno.ClasesFijas)
                {
                    if (claseFija.Id != 0)
                        Context.Attach(claseFija);
                }
            }
            Set.Add(alumno);
            Context.SaveChanges();
        }

        public void Update(Alumno entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            Context.SaveChanges();
        }

        public List<Alumno> GetAll()
        {
            return Set.OfType<Alumno>().Where(p => EF.Property<string>(p, "Discriminator") == "Alumno").ToList();
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


        public IQueryable<Alumno> IncludeAll(params string[] list)
        {
            IQueryable<Alumno> queryable = Set.OfType<Alumno>().Where(p => EF.Property<string>(p, "Discriminator") == "Alumno");
            var type = typeof(Alumno);
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

        public IQueryable<Alumno> IncludeAllAnidado(params string[] list)
        {
            IQueryable<Alumno> queryable = Set.OfType<Alumno>().Where(p => EF.Property<string>(p, "Discriminator") == "Alumno");
            var type = typeof(Alumno);

            foreach (var include in list)
            {
                if (include.Contains("."))
                {
                    // Handle nested includes
                    var includeParts = include.Split('.');
                    if (includeParts.Length == 2 && includeParts[0] == "Plan" && includeParts[1] == "Actividades")
                    {
                        queryable = queryable.Include(a => a.Plan)
                                             .ThenInclude(p => p.Actividades);
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

        public void Desactivate(Alumno alumno)
        {
            throw new NotImplementedException();
        }
    }
}
