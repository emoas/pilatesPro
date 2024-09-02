using AutoMapper;
using Commons.Exceptions;
using DataAccessInterface.Repositories;
using Domain;
using Domain.Alumnos;
using Dto;
using Dto.Alumnos;
using ServicesInterface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Services
{
    public class AlumnoService : IAlumnoService
    {
        private IAlumnoRepository alumnoRepository;
        private IRepository<Patologia> patologiaRepository;
        private IRepository<Plan> planRepository;
        private IRepository<Clase> claseRepository;
        private IRepository<ClaseFija> claseFijaRepository;
        private IRepository<AlumnoClase> alumnoClaseRepository;
        private IAgendaService agendaService;
        private IMapper mapper;
        public AlumnoService(IRepository<AlumnoClase> alumnoClaseRepository,IAgendaService agendaService,IMapper mapper, IAlumnoRepository alumnoRepository, IRepository<ClaseFija> claseFijaRepository, IRepository<Plan> planRepository, IRepository<Patologia> patologiaRepository, IRepository<Clase> claseRepository)
        {
            this.alumnoRepository = alumnoRepository;
            this.patologiaRepository = patologiaRepository;
            this.planRepository = planRepository;
            this.claseRepository = claseRepository;
            this.claseFijaRepository = claseFijaRepository;
            this.alumnoClaseRepository = alumnoClaseRepository;
            this.mapper = mapper;
            this.agendaService = agendaService;
        }

        public AlumnoDTO Add(AlumnoDTO alumnoDTO)
        {
            Alumno alumno = new Alumno
            {
                Name = alumnoDTO.Name,
                Apellido = alumnoDTO.Apellido,
                Email = alumnoDTO.Email,
                Cedula = alumnoDTO.Cedula,
                Direccion = alumnoDTO.Direccion,
                Celular = alumnoDTO.Celular,
                Ciudad = alumnoDTO.Ciudad,
                Observaciones = alumnoDTO.Observaciones,
                EmeregenciaMovil = alumnoDTO.EmeregenciaMovil,
                ContactoEmergencia = alumnoDTO.ContactoEmergencia,
                TelefonoContacto = alumnoDTO.TelefonoContacto,
                Activo = alumnoDTO.Activo,
                FechaNacimiento = alumnoDTO.FechaNacimiento
            };
            var patologias = new List<Patologia>();
            var clases = new List<Clase>();

            if (alumnoDTO.PatologíasQuePresenta != null)
            {
                foreach (PatologiaDTO patologia in alumnoDTO.PatologíasQuePresenta)
                {
                    if (patologia.Id != 0)
                    {
                        Patologia patologiaRepo = this.patologiaRepository.List().FirstOrDefault(p => p.Id == patologia.Id);
                        patologias.Add(patologiaRepo);
                    }
                }
            }
            alumno.PatologíasQuePresenta = patologias;

            var plan = this.planRepository.List().FirstOrDefault(p => p.Id == alumnoDTO.Plan.Id);
            if (plan == null)
            {
                throw new ValidationException("El plan no existe.");
            }
            alumno.Plan = plan;
            this.alumnoRepository.AddAndSave(alumno);
            return this.mapper.Map<AlumnoDTO>(alumno);
        }
        public IEnumerable<AlumnoDTO> GetAll()
        {
            var alumnos = alumnoRepository.GetAll().OrderByDescending(a => a.Activo) // Primero los activos
            .ThenBy(a => a.Name)            // Orden alfabético por nombre
            .ToList();
            return this.mapper.Map<IEnumerable<AlumnoDTO>>(alumnos);
        }

        public AlumnoDTO GetId(int alumnoId)
        {
            var alumno = this.alumnoRepository.IncludeAll("ClasesFijas", "PatologíasQuePresenta", "HorariosPuntuales", "HorariosVariables","Plan").FirstOrDefault(a => a.Id == alumnoId);
            return this.mapper.Map<AlumnoDTO>(alumno);
        }
        public IEnumerable<ClaseFijaDTO> GetFijasAlumno(int idAlumno)
        {
            var clasesFijas = this.claseFijaRepository.List().Where(c => c.AlumnoId == idAlumno);
            return this.mapper.Map<IEnumerable<ClaseFijaDTO>>(clasesFijas);
        }
        public void Desactivate(int alumnoId)
        {
            Alumno alumno = (Alumno)this.alumnoRepository.GetAll().FirstOrDefault(a => a.Id == alumnoId);
            if (alumno == null)
            {
                throw new Exception("No existe el alumno seleccionado.");
            }
            if (alumno.Activo)
            {
                //Al desactivar el alumno lo elimino las clases fijas asigandas
                alumno.Activo = false;
                this.QuitarAlumnoTodasLasClases(alumnoId);
            }
            else
            {
                alumno.Activo = true;
                this.AgregarAlumnoTodasLasClasesFijas(alumnoId);
            }
            this.alumnoRepository.Update(alumno);
        }

        public AlumnoDTO Update(AlumnoDTO alumnoDTOUpdate)
        {
            var patologias = new List<Patologia>();
            var clases = new List<Clase>();

            Alumno alumnoToUpdate = this.alumnoRepository.IncludeAll("ClasesFijas", "PatologíasQuePresenta").FirstOrDefault(p => p.Id == alumnoDTOUpdate.Id);
            if (alumnoToUpdate == null)
            {
                throw new ValidationException("No existe el alumno seleccionado.");
            }

            //agrego las patologias
            if (alumnoDTOUpdate.PatologíasQuePresenta != null)
            {
                foreach (PatologiaDTO patologia in alumnoDTOUpdate.PatologíasQuePresenta)
                {
                    if (patologia.Id != 0)
                    {
                        Patologia patologiaRepo = this.patologiaRepository.List().FirstOrDefault(p => p.Id == patologia.Id);
                        patologias.Add(patologiaRepo);
                    }
                }
            }
            alumnoToUpdate.PatologíasQuePresenta = patologias;


            var plan = this.planRepository.List().FirstOrDefault(p => p.Id == alumnoDTOUpdate.Plan.Id);
            if (plan == null)
            {
                throw new ValidationException("El plan no existe.");
            }
            alumnoToUpdate.Plan = plan;

            alumnoToUpdate.Name = alumnoDTOUpdate.Name;
            alumnoToUpdate.Apellido = alumnoDTOUpdate.Apellido;
            alumnoToUpdate.Email = alumnoDTOUpdate.Email;
            alumnoToUpdate.Cedula = alumnoDTOUpdate.Cedula;
            alumnoToUpdate.Direccion = alumnoDTOUpdate.Direccion;
            alumnoToUpdate.Celular = alumnoDTOUpdate.Celular;
            alumnoToUpdate.Ciudad = alumnoDTOUpdate.Ciudad;
            alumnoToUpdate.EmeregenciaMovil = alumnoDTOUpdate.EmeregenciaMovil;
            alumnoToUpdate.ContactoEmergencia = alumnoDTOUpdate.ContactoEmergencia;
            alumnoToUpdate.TelefonoContacto = alumnoDTOUpdate.TelefonoContacto;
            alumnoToUpdate.Activo = alumnoDTOUpdate.Activo;
            alumnoToUpdate.FechaNacimiento = alumnoDTOUpdate.FechaNacimiento;
            alumnoToUpdate.Observaciones = alumnoDTOUpdate.Observaciones;
            this.alumnoRepository.Update(alumnoToUpdate);
            return this.mapper.Map<AlumnoDTO>(alumnoToUpdate);
        }
        public ClaseFijaDTO AddClaseFija(int idAlumno, ClaseFijaDTO claseFijaDTO)
        {
            ClaseFija claseFija = new ClaseFija
            {
                ActividadId = claseFijaDTO.ActividadId,
                LocalId = claseFijaDTO.LocalId,
                Dia = claseFijaDTO.Dia,
                Hora = claseFijaDTO.Hora,
            };
            Alumno alumnoToUpdate = this.alumnoRepository.IncludeAll("ClasesFijas", "Plan").FirstOrDefault(a => a.Id == idAlumno);
                if ((alumnoToUpdate.ClasesFijas.Count < alumnoToUpdate.Plan.VecesxSemana) || alumnoToUpdate.Plan.VecesxSemana==0)
            {
                alumnoToUpdate.ClasesFijas.Add(claseFija);
                this.alumnoRepository.Update(alumnoToUpdate);
                if(alumnoToUpdate.Activo)
                    addClasesFijasAlumno(idAlumno, claseFijaDTO);
            }else
                throw new Exception("Se supero la cantidad de clases por semana");
            return claseFijaDTO;
        }

        public ClaseFijaDTO UpdateClaseFija(int id, ClaseFijaDTO claseFijaDTO)
        {
            ClaseFija claseFijaUpdate = this.claseFijaRepository.IncludeAll("Alumno").FirstOrDefault(c => c.Id == id);
            if (claseFijaUpdate == null)
            {
                throw new Exception("No existe la clase fija seleccionada.");
            }
            //elimino al alumno de la clase fija
            removeClasesFijasAlumno(id);
            claseFijaUpdate.ActividadId = claseFijaDTO.ActividadId;
            claseFijaUpdate.LocalId = claseFijaDTO.LocalId;
            claseFijaUpdate.Dia = claseFijaDTO.Dia;
            claseFijaUpdate.Hora = claseFijaDTO.Hora;
            this.claseFijaRepository.Update(claseFijaUpdate);
            if (claseFijaUpdate.Alumno.Activo)
                addClasesFijasAlumno(claseFijaUpdate.AlumnoId, claseFijaDTO);
            return this.mapper.Map<ClaseFijaDTO>(claseFijaUpdate);
        }
        public void RemoveClaseFija(int id)
        {
            ClaseFija claseFija = this.claseFijaRepository.List().FirstOrDefault(c => c.Id == id);
            if (claseFija == null)
            {
                throw new Exception("No existe la clase fija seleccionada.");
            }
            removeClasesFijasAlumno(id);
            this.claseFijaRepository.Delete(claseFija);
        }
        public void addClasesFijasAlumno(int idAlumno, ClaseFijaDTO claseFijaDTO)
        {
            //obtengo las clases a las que debo agregar el alumno
            var clases = this.agendaService.GetClasesFijas(claseFijaDTO.LocalId, claseFijaDTO.ActividadId, claseFijaDTO.Dia, claseFijaDTO.Hora);
            //agrego el alumno a las clases
            foreach (Clase clase in clases)
            {
                agregarAlumnoAClase(idAlumno, clase.Id, AlumnoClase.tipo.FIJO);
            }
        }
        private void removeClasesFijasAlumno(int idClaseFija)
        {
            ClaseFija claseFijaRemove = this.claseFijaRepository.List().FirstOrDefault(c => c.Id == idClaseFija);
            //obtengo las clases a las que debo quitar el alumno
            var clases = this.agendaService.GetClasesFijas(claseFijaRemove.LocalId, claseFijaRemove.ActividadId, claseFijaRemove.Dia, claseFijaRemove.Hora);
            //agrego el alumno a las clases
            foreach (Clase clase in clases)
            {
                Clase claseUpdate = this.claseRepository.IncludeAll("ClasesAlumno").FirstOrDefault(c => c.Id == clase.Id);

                var alumnoClaseAEliminar = claseUpdate.ClasesAlumno.FirstOrDefault(ac => ac.AlumnoId == claseFijaRemove.AlumnoId);

                if (alumnoClaseAEliminar != null)
                {
                    claseUpdate.ClasesAlumno.Remove(alumnoClaseAEliminar);
                    claseUpdate.CuposOtorgados--;
                    this.alumnoClaseRepository.Delete(alumnoClaseAEliminar);
                }
                this.claseRepository.Update(claseUpdate);
            }
        }

        public void RemoveAlumnoClase(int alumnoId, int claseId)
        {
            Clase claseUpdate = this.claseRepository.IncludeAll("ClasesAlumno").FirstOrDefault(c => c.Id == claseId);

            var alumnoClaseAEliminar = claseUpdate.ClasesAlumno.FirstOrDefault(ac => ac.AlumnoId == alumnoId);

            if (alumnoClaseAEliminar != null)
            {
                claseUpdate.ClasesAlumno.Remove(alumnoClaseAEliminar);
                claseUpdate.CuposOtorgados--;
                this.alumnoClaseRepository.Delete(alumnoClaseAEliminar);
            }
            this.claseRepository.Update(claseUpdate);
        }

        private void QuitarAlumnoTodasLasClases(int alumnoId)
        {
            var alumnosClases = this.alumnoClaseRepository.IncludeAll().Where(ac => ac.AlumnoId == alumnoId).ToList();
            foreach (AlumnoClase alumnoClase in alumnosClases)
            {
                this.RemoveAlumnoClase(alumnoId, alumnoClase.ClaseId);
            }
        }
        private void AgregarAlumnoTodasLasClasesFijas(int alumnoId)
        {
            var alumnosClasesFijasDTO = this.mapper.Map<IEnumerable<ClaseFijaDTO>>(this.claseFijaRepository.IncludeAll().Where(cf => cf.AlumnoId == alumnoId).ToList());
            foreach (ClaseFijaDTO alumnoClaseFija in alumnosClasesFijasDTO)
            {
                this.addClasesFijasAlumno(alumnoId, alumnoClaseFija);
            }  
        }
        
        public void AddAlumnoClase(AlumnoClaseDTO alumnoClaseDTO)
        {
            bool resultado = agregarAlumnoAClase(alumnoClaseDTO.AlumnoId, alumnoClaseDTO.ClaseId, (AlumnoClase.tipo)alumnoClaseDTO.Tipo);
            if (!resultado)
            {
                throw new Exception("No se puedo agregar el alumno, el plan no lo permite");
            }
        }
        public void UpdateClasesAlumno(int claseId)
        {
            // Obtener la clase
            Clase clase = this.claseRepository.IncludeAll("ClasesAlumno", "Local").FirstOrDefault(c => c.Id == claseId);

            if (clase == null)
            {
                throw new Exception($"No se encontró la clase con ID {claseId}");
            }
            // Configurar la cultura en español (España)
            CultureInfo culturaEspañola = new CultureInfo("es-ES");
            // Obtener las clases fijas
            var clasesFijas = this.claseFijaRepository.IncludeAll()
                            .Where(c => c.ActividadId == clase.ActividadId &&
                                        c.LocalId == clase.Local.Id &&
                                        c.Dia == clase.HorarioInicio.ToString("dddd", culturaEspañola) &&
                                        c.Hora == clase.HorarioInicio.ToString("HH:mm"))
                            .ToList();

            // Iterar sobre las clases fijas y agregar alumnos a la clase
            foreach (ClaseFija claseFija in clasesFijas)
            {
                bool resultado = this.agregarAlumnoAClase(claseFija.AlumnoId, claseId, AlumnoClase.tipo.FIJO);
                if (!resultado)
                {
                    // Opcional: manejar el caso donde no se pudo agregar el alumno
                    // Por ejemplo, registrar un mensaje de advertencia
                }
            }
        }

        public bool agregarAlumnoAClase(int alumnoId, int claseId, AlumnoClase.tipo tipo)
        {
            try
            {
                Alumno alumno = this.alumnoRepository.IncludeAllAnidado("Plan.Actividades").FirstOrDefault(a => a.Id == alumnoId);
                Clase clase = this.claseRepository.IncludeAll("ClasesAlumno").FirstOrDefault(c => c.Id == claseId);
                clase.CuposOtorgados = clase.ClasesAlumno.Count();
                if (alumno == null || clase == null)
                {
                    // Registrar un mensaje o lanzar una excepción específica si es necesario
                    return false; // Indicar que la operación falló debido a datos no encontrados
                }
                bool existe = clase.ClasesAlumno.Any(ac => ac.AlumnoId == alumnoId);
                if (!existe && alumno.Activo)
                {
                    if (puedeReservar(alumno, clase))
                    {
                        AlumnoClase alumnoClase = new AlumnoClase
                        {
                            AlumnoId = alumnoId,
                            Tipo = tipo
                        };
                        clase.CuposOtorgados++;
                        clase.ClasesAlumno.Add(alumnoClase);
                        this.claseRepository.Update(clase);
                        return true; // Indicar que la operación fue exitosa
                    }
                    else
                    {
                        // Opcional: registrar el error o realizar alguna acción
                        return false; // El plan no permite agregar el alumno
                    }
                }
                else
                {
                    return false; // El alumno ya está en la clase
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción, por ejemplo, registrarla
                Console.WriteLine($"Error: {ex.Message}");
                return false; // Indicar que ocurrió un error
            }
        }

        private bool puedeReservar(Alumno alumno, Clase clase)
        {
            bool puede = false;
            if (alumno.Plan.Tipo == Plan.tipo.SEMANAL)
            {
                if (clase.CuposOtorgados < clase.CuposTotales && (this.GetMisReservasSemana(alumno.Id, clase.HorarioInicio) < alumno.Plan.VecesxSemana || alumno.Plan.VecesxSemana == 0))
                    puede = true;
                if (!alumno.Plan.Actividades.Any(a => a.Id == clase.ActividadId))//si no esta en las actividades del plan
                    puede = false;
                if (alumno.Plan.ActividadLibreId == clase.ActividadId && clase.CuposOtorgados < clase.CuposTotales)
                    puede = true;
            }else if (alumno.Plan.Tipo == Plan.tipo.PASE_LIBRE)
            {
                if (clase.CuposOtorgados < clase.CuposTotales && (this.GetMisReservasMes(alumno.Id, clase.HorarioInicio) < alumno.Plan.VecesxMes))
                    puede = true;
                if (!alumno.Plan.Actividades.Any(a => a.Id == clase.ActividadId))//si no esta en las actividades del plan
                    puede = false;
            }else if (alumno.Plan.Tipo == Plan.tipo.TU_PASE)
            {
                if (clase.CuposOtorgados<clase.CuposTotales)
                    puede = true;
                if (!alumno.Plan.Actividades.Any(a => a.Id == clase.ActividadId))//si no esta en las actividades del plan
                    puede = false;
            }
            return puede;
        }

        public IEnumerable<ClaseDTO> GetMisReservas(int idAlumno)
        {
            DateTime today = DateTime.Now;
            DateTime oneMonthLater = today.AddMonths(1);
            var clases = this.claseRepository.IncludeAll("Local", "Actividad", "ClasesAlumno")
                                .Where(c => c.ClasesAlumno.Any(ac => ac.AlumnoId == idAlumno)
                                        && c.HorarioInicio >= today
                                        && c.HorarioInicio <= oneMonthLater)
                                .OrderBy(c => c.HorarioInicio);
            return this.mapper.Map<IEnumerable<ClaseDTO>>(clases);
        }

        public int GetMisReservasSemana(int alumnoId, DateTime dayOfWeek)
        {
            DateTime startOfWeek = dayOfWeek.StartOfWeek(DayOfWeek.Monday); // Calcula el inicio de la semana actual
            // Calcula el final de la semana sumando 6 días al inicio de la semana
            DateTime endOfWeek = startOfWeek.AddDays(6);
                var count = this.alumnoClaseRepository.IncludeAll("Clase")
                    .Where(ac => ac.AlumnoId == alumnoId &&
                                 ac.Clase.HorarioInicio >= startOfWeek &&
                                 ac.Clase.HorarioFin <= endOfWeek)
                    .Count();
                return count;
        }
        public int GetMisReservasMes(int alumnoId, DateTime dayOfMonth)
        {
            // Calcular el primer día del mes
            DateTime startOfMonth = new DateTime(dayOfMonth.Year, dayOfMonth.Month, 1);

            // Calcular el último día del mes sumando un mes y restando un día
            DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            // Contar las reservas dentro del rango del mes
            var count = this.alumnoClaseRepository.IncludeAll("Clase")
                .Where(ac => ac.AlumnoId == alumnoId &&
                             ac.Clase.HorarioInicio >= startOfMonth &&
                             ac.Clase.HorarioFin <= endOfMonth)
                .Count();

            return count;
        }

    }
    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime date, DayOfWeek firstDayOfWeek)
        {
            int diff = (7 + (date.DayOfWeek - firstDayOfWeek)) % 7;
            return date.AddDays(-1 * diff).Date;
        }
    }
}
