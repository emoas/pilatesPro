using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessInterface.Repositories
{
    public interface IActividadRepository
    {
        void AddAndSave(Actividad actividad);
        void Update(Actividad actividad);
        void Desactivate(Actividad actividad);
        IQueryable<Actividad> GetAll();
        IQueryable<Actividad> IncludeAll(params string[] list);
    }
}
