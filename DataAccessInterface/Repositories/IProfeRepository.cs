using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessInterface.Repositories
{
    public interface IProfeRepository
    {
        void AddAndSave(Profesor profe);
        void Update(Profesor profe);
        void Desactivate(Profesor profe);

        List<Profesor> getProfesores();

        IQueryable<Profesor> IncludeAll(params string[] list);

    }
}
