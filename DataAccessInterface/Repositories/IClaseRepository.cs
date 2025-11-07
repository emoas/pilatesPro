using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessInterface.Repositories
{
    public interface IClaseRepository
    {
        void AddAndSave(Clase entity);

        void AddAndSave(IList<Clase> entities);

        void Update(Clase entity);

        void Delete(Clase entity);

        IQueryable<Clase> List();

        IQueryable<Clase> IncludeAll(params string[] list);
        IQueryable<Clase> IncludeAllAnidado(params string[] list);
        IQueryable<Clase> IncludeAllAnidadoFull(params string[] list);
    }
}
