using Domain.Logs;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServicesInterface
{
    public interface ILogService
    {
        Logs_AddAlumnoClase AddAlumnoClase(Logs_AddAlumnoClase logsAlumnoClase);
    }
}
