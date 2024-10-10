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
        private IRepository<Falta> faltaRepository;
        private IAlumnoClaseRepository alumnoClaseRepository;
        private IAgendaService agendaService;
        private IMapper mapper;
        public AlumnoService(IAlumnoClaseRepository alumnoClaseRepository,IAgendaService agendaService,IMapper mapper, IAlumnoRepository alumnoRepository, IRepository<ClaseFija> claseFijaRepository, IRepository<Plan> planRepository, IRepository<Patologia> patologiaRepository, IRepository<Clase> claseRepository, IRepository<Falta> faltaRepository)
        {
            this.alumnoRepository = alumnoRepository;
            this.patologiaRepository = patologiaRepository;
            this.planRepository = planRepository;
            this.claseRepository = claseRepository;
            this.faltaRepository = faltaRepository;
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
            if (EmailExists(alumno.Email))
            {
                throw new ArgumentException("El email ya está registrado.");
            }
            this.alumnoRepository.AddAndSave(alumno);
            return this.mapper.Map<AlumnoDTO>(alumno);
        }

        public bool EmailExists(string email)
        {
            return alumnoRepository.GetAll().Any(u => u.Email == email);
        }
        public bool EmailExists(string email, int alumnoId)
        {
            // Verificar si existe un alumno con el email excluyendo al alumno actual (por su id)
            return alumnoRepository.GetAll().Any(u => u.Email == email && u.Id != alumnoId);
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

            if (EmailExists(alumnoDTOUpdate.Email, alumnoDTOUpdate.Id))
            {
                throw new ArgumentException("El email ya está registrado en otro alumno.");
            }
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
                if (puedeAgregarFija(alumnoToUpdate,claseFija))
            {
                alumnoToUpdate.ClasesFijas.Add(claseFija);
                this.alumnoRepository.Update(alumnoToUpdate);
                if(alumnoToUpdate.Activo)
                    addClasesFijasAlumno(idAlumno, claseFijaDTO);
            }else
                throw new Exception("Se supero la cantidad de clases por semana");
            return claseFijaDTO;
        }

        private bool puedeAgregarFija(Alumno alumno, ClaseFija claseFija)
        {
            bool puede = false;
            if (alumno.Plan.Tipo == Plan.tipo.SEMANAL)
            {
                if ((alumno.ClasesFijas.Count(c => c.ActividadId != alumno.Plan.ActividadLibreId) < alumno.Plan.VecesxSemana) || alumno.Plan.VecesxSemana == 0)
                    puede = true;
                if (alumno.Plan.ActividadLibreId != null && alumno.Plan.ActividadLibreId == claseFija.ActividadId)
                    puede = true;
            }
            else if (alumno.Plan.Tipo == Plan.tipo.PASE_LIBRE)
            {
            }
            else if (alumno.Plan.Tipo == Plan.tipo.TU_PASE)
            {
            }
            return puede;
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

        public void CancelReservaManual(int alumnoId, int claseId)
        {
            Clase claseUpdate = this.claseRepository.IncludeAll("ClasesAlumno").FirstOrDefault(c => c.Id == claseId);

            var alumnoClaseAEliminar = claseUpdate.ClasesAlumno.FirstOrDefault(ac => ac.AlumnoId == alumnoId);

            if (alumnoClaseAEliminar != null)
            {
                claseUpdate.ClasesAlumno.Remove(alumnoClaseAEliminar);
                claseUpdate.CuposOtorgados--;
                this.alumnoClaseRepository.Delete(alumnoClaseAEliminar);
                if (alumnoClaseAEliminar.Tipo == AlumnoClase.tipo.FIJO)
                {
                    Alumno alumno = this.alumnoRepository.GetAll().FirstOrDefault(a => a.Id == alumnoId);
                    alumno.CuposPendientes++;
                    this.alumnoRepository.Update(alumno);
                }
            }
            this.claseRepository.Update(claseUpdate);
        }

        public void CancelReservaWeb(int alumnoId, int claseId)
        {
            Clase claseUpdate = this.claseRepository.IncludeAll("ClasesAlumno").FirstOrDefault(c => c.Id == claseId);
            Alumno alumno = this.alumnoRepository.IncludeAll().FirstOrDefault(a => a.Id == alumnoId);

            var alumnoClaseAEliminar = claseUpdate.ClasesAlumno.FirstOrDefault(ac => ac.AlumnoId == alumnoId);

            if (alumnoClaseAEliminar != null)
            {
                claseUpdate.ClasesAlumno.Remove(alumnoClaseAEliminar);
                claseUpdate.CuposOtorgados--;
                this.alumnoClaseRepository.Delete(alumnoClaseAEliminar);
                DateTime fechaActual = DateTime.Now; // Fecha y hora actual
                DateTime limiteCancelacion = claseUpdate.HorarioInicio.AddHours(-2);
                if (alumnoClaseAEliminar.Tipo == AlumnoClase.tipo.FIJO && fechaActual <= limiteCancelacion)
                { 
                    alumno.CuposPendientes++;
                    this.alumnoRepository.Update(alumno);
                }else if (alumno.PlanId == 39 && fechaActual >= limiteCancelacion)//Alumno Pase Libre
                {
                    this.AgregarFalta(alumnoId, claseId);
                }
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
        // Método original sin modificación del tipo
        public bool agregarAlumnoAClase(int alumnoId, int claseId, AlumnoClase.tipo tipo)
        {
            return agregarAlumnoAClase(alumnoId, claseId, ref tipo); // Llamar al método con ref
        }

        public bool agregarAlumnoAClase(int alumnoId, int claseId, ref AlumnoClase.tipo tipo)
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
                    if (tipo == AlumnoClase.tipo.ADMIN || puedeReservar(alumno, clase, ref tipo))
                    {
                        AlumnoClase alumnoClase = new AlumnoClase
                        {
                            AlumnoId = alumnoId,
                            Tipo = tipo
                        };
                        clase.CuposOtorgados++;
                        clase.ClasesAlumno.Add(alumnoClase);
                        this.claseRepository.Update(clase);
                        if (tipo == AlumnoClase.tipo.RECUPERACION)
                        {
                            alumno.CuposPendientes--;
                            this.alumnoRepository.Update(alumno);
                        }
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
                    return false; // El alumno ya está en la clase o esta inactivo
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción, por ejemplo, registrarla
                Console.WriteLine($"Error: {ex.Message}");
                return false; // Indicar que ocurrió un error
            }
        }

        private bool puedeReservar(Alumno alumno, Clase clase, ref AlumnoClase.tipo tipo)
        {
            bool cuposDisponibles = clase.CuposOtorgados < clase.CuposTotales;
            bool actividadEnPlan = alumno.Plan.Actividades.Any(a => a.Id == clase.ActividadId);
            bool puede = false;

            switch (alumno.Plan.Tipo)
            {
                case Plan.tipo.SEMANAL:
                    // Verificar si tiene cupos semanales o ilimitados
                    bool puedeReservarSemanal = this.GetMisReservasSemana(alumno.Id, clase.HorarioInicio) < alumno.Plan.VecesxSemana || alumno.Plan.VecesxSemana == 0;

                    if (cuposDisponibles && puedeReservarSemanal && actividadEnPlan && tipo != AlumnoClase.tipo.WEB)
                    {
                        puede = true;
                    }
                    // Actividad libre
                    if (cuposDisponibles && alumno.Plan.ActividadLibreId == clase.ActividadId)
                        puede = true;
                    //Clase de recuperacion
                    if (cuposDisponibles && actividadEnPlan && alumno.CuposPendientes>0 && (tipo== AlumnoClase.tipo.WEB || tipo == AlumnoClase.tipo.RECUPERACION)) {
                        puede = true;
                        tipo = AlumnoClase.tipo.RECUPERACION;
                    }
                    break;
                case Plan.tipo.PASE_LIBRE:
                    // Verificar si tiene cupos mensuales
                    bool puedeReservarMensual = this.GetMisReservasMes(alumno.Id, clase.HorarioInicio) < alumno.Plan.VecesxMes;
                    int faltas = this.ObtenerFaltasDelMes(alumno.Id, clase.HorarioInicio);
                    if (cuposDisponibles && puedeReservarMensual && actividadEnPlan && faltas<2)
                        puede = true;
                    break;
                case Plan.tipo.TU_PASE:
                    // Pase sin restricciones de cantidad
                    if (cuposDisponibles && actividadEnPlan)
                        puede = true;
                    break;
            }

            return puede;
        }

        public IEnumerable<AlumnoClaseDTO> GetMisReservas(int idAlumno)
        {
            DateTime today = DateTime.Now;

            // Obtener el día de inicio de la semana actual (lunes)
            int daysUntilMonday = ((int)DayOfWeek.Monday - (int)today.DayOfWeek + 7) % 7;
            DateTime startOfWeek = today.AddDays(-daysUntilMonday);

            // Obtener el sábado de la siguiente semana
            DateTime endOfNextWeek = startOfWeek.AddDays(13); // Lunes + 13 días = siguiente sábado

            // Filtrar las clases entre el lunes de la semana actual y el sábado de la siguiente semana
            var alumnoClases = this.alumnoClaseRepository.IncludeAllAnidado("Clase", "Clase.Actividad", "Clase.Local", "Alumno")
                                    .Where(ac => ac.AlumnoId == idAlumno
                                            && ac.Clase.HorarioInicio >= today
                                            && ac.Clase.HorarioInicio <= endOfNextWeek)
                                    .OrderBy(ac => ac.Clase.HorarioInicio);

            return this.mapper.Map<IEnumerable<AlumnoClaseDTO>>(alumnoClases);
        }

        public int GetMisReservasSemana(int alumnoId, DateTime dayOfWeek)
        {
            DateTime startOfWeek = dayOfWeek.StartOfWeek(DayOfWeek.Monday); // Calcula el inicio de la semana actual
            // Calcula el final de la semana sumando 6 días al inicio de la semana
            DateTime endOfWeek = startOfWeek.AddDays(6);
                var count = this.alumnoClaseRepository.IncludeAll("Clase","Alumno.Plan")
                    .Where(ac => ac.AlumnoId == alumnoId &&
                                 ac.Alumno.Plan.ActividadLibreId!=ac.Clase.ActividadId &&
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

        public IEnumerable<AlumnoClaseDTO> GetReservasPeriodo(int alumnoId, DateTime desde, DateTime hasta)
        {
            // Contar las reservas dentro del rango del mes
            var alumnoClases = this.alumnoClaseRepository.IncludeAllAnidado("Clase.Actividad", "Clase.Local")
                .Where(ac => ac.AlumnoId == alumnoId &&
                             ac.Clase.HorarioInicio >= desde &&
                             ac.Clase.HorarioFin <= hasta).OrderBy(ac => ac.Clase.HorarioInicio);

            return this.mapper.Map<IEnumerable<AlumnoClaseDTO>>(alumnoClases);
        }

        public void AgregarFalta(int alumnoId, int claseId)
        {
            Alumno alumno = this.alumnoRepository.IncludeAll("Faltas").FirstOrDefault(a => a.Id == alumnoId);
            Clase clase = this.claseRepository.IncludeAll("").FirstOrDefault(c => c.Id == claseId);
            if (alumno != null)
                {
                    // Agregar la nueva falta
                    var nuevaFalta = new Falta
                    {
                        Fecha = DateTime.Now,
                        AlumnoId = alumnoId,
                        ClaseId=claseId
                    };
                    alumno.Faltas.Add(nuevaFalta);
                    this.alumnoRepository.Update(alumno);
            }
        }
        public int ObtenerFaltasDelMes(int alumnoId, DateTime fecha)
        {
                // Obtener el mes y año de la fecha proporcionada
                int mes = fecha.Month;
                int año = fecha.Year;
                // Obtener las faltas del alumno para el mes y año especificados
                var faltasDelMes = this.faltaRepository.List()
                    .Where(f => f.AlumnoId == alumnoId && f.Fecha.Month == mes && f.Fecha.Year == año)
                    .Count();
                return faltasDelMes;
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
