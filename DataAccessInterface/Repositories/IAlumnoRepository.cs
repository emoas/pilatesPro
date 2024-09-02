using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessInterface.Repositories
{
    public interface IAlumnoRepository
    {
        void AddAndSave(Alumno alumno);
        void Update(Alumno alumno);
        void Desactivate(Alumno alumno);

        List<Alumno> GetAll();

        IQueryable<Alumno> IncludeAll(params string[] list);
        IQueryable<Alumno> IncludeAllAnidado(params string[] list);
    }
}
