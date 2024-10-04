using Domain.Alumnos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessInterface.Repositories
{
    public interface IAlumnoClaseRepository
    {
        void AddAndSave(AlumnoClase alumno);
        void Update(AlumnoClase alumno);
        void Desactivate(AlumnoClase alumno);
        void Delete(AlumnoClase alumno);
        IQueryable<AlumnoClase> GetAll();

        IQueryable<AlumnoClase> IncludeAll(params string[] list);
        IQueryable<AlumnoClase> IncludeAllAnidado(params string[] list);
    }
}
