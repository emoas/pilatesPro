﻿using AutoMapper;
using Commons.Exceptions;
using DataAccessInterface.Repositories;
using Domain;
using Domain.Alumnos;
using Domain.Logs;
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
        private ILogService logService;
        private IMapper mapper;
        public AlumnoService(IAlumnoClaseRepository alumnoClaseRepository, ILogService logService,IAgendaService agendaService,IMapper mapper, IAlumnoRepository alumnoRepository, IRepository<ClaseFija> claseFijaRepository, IRepository<Plan> planRepository, IRepository<Patologia> patologiaRepository, IRepository<Clase> claseRepository, IRepository<Falta> faltaRepository)
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
            this.logService = logService;
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
                try
                {
                    agregarAlumnoAClase(idAlumno, clase.Id, AlumnoClase.tipo.FIJO);
                }
                catch (Exception ex)
                {
                    // Manejar la excepción, por ejemplo, registrarla
                    Console.WriteLine($"Error: {ex.Message}");
                }
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

        private void CancelAlumnoClase(int alumnoId, int claseId)
        {
            Clase claseUpdate = this.claseRepository.IncludeAll("ClasesAlumno").FirstOrDefault(c => c.Id == claseId);

            var alumnoClaseAEliminar = claseUpdate.ClasesAlumno.FirstOrDefault(ac => ac.AlumnoId == alumnoId);

            if (alumnoClaseAEliminar != null)
            {
                //claseUpdate.ClasesAlumno.Remove(alumnoClaseAEliminar);
                alumnoClaseAEliminar.Estado= AlumnoClase.estado.CANCELADA;
                alumnoClaseAEliminar.FechaCancelacion = DateTime.Now;
                alumnoClaseAEliminar.Asistio = false;
                if (claseUpdate.CuposOtorgados > 0)
                {
                    claseUpdate.CuposOtorgados--;
                    this.claseRepository.Update(claseUpdate);
                    // Registrar un mensaje o lanzar una excepción específica si es necesario
                    // Opcional: registrar el error o realizar alguna acción
                    Logs_AddAlumnoClase logsAlumnoClase = new Logs_AddAlumnoClase
                    {
                        AlumnoId = alumnoId,
                        ClaseId = claseId,
                        Fecha = DateTime.Now,
                        Estado = Logs_AddAlumnoClase.estado.PENDIENTE,
                        Descripcion = $"Se cancelo la reservas del alumno (CancelAlumnoClase)",
                        Tipo = Logs_AddAlumnoClase.tipo.CANCELACIONADMIN
                    };
                    this.logService.AddAlumnoClase(logsAlumnoClase);
                }
            }
            this.alumnoClaseRepository.Update(alumnoClaseAEliminar);
        }
        public void CancelReservaManual(int alumnoId, int claseId)
        {
            Clase claseUpdate = this.claseRepository.IncludeAll("ClasesAlumno").FirstOrDefault(c => c.Id == claseId);
            // Hacer una copia superficial de los valores actuales de claseUpdate
            Clase aux = new Clase
            {
                Id = claseUpdate.Id,
                CuposOtorgados = claseUpdate.CuposOtorgados,
                // Aquí puedes copiar otras propiedades que necesites rastrear
            };
            var alumnoClaseAUpdate = claseUpdate.ClasesAlumno.FirstOrDefault(ac => ac.AlumnoId == alumnoId && ac.Estado!=AlumnoClase.estado.CANCELADA);
            if (alumnoClaseAUpdate != null)
            {
                alumnoClaseAUpdate.Estado = AlumnoClase.estado.CANCELADA;
                alumnoClaseAUpdate.FechaCancelacion = DateTime.Now;
                alumnoClaseAUpdate.Asistio = false;
                // Actualizar los cupos otorgados solo si es mayor a cero para evitar que queden negativos
                if (claseUpdate.CuposOtorgados > 0)
                {
                    claseUpdate.CuposOtorgados--;
                    this.claseRepository.Update(claseUpdate);
                    // Registrar un mensaje o lanzar una excepción específica si es necesario
                    // Opcional: registrar el error o realizar alguna acción
                    Logs_AddAlumnoClase logsAlumnoClase = new Logs_AddAlumnoClase
                    {
                        AlumnoId = alumnoId,
                        ClaseId = claseId,
                        Fecha = DateTime.Now,
                        Estado = Logs_AddAlumnoClase.estado.PENDIENTE,
                        Descripcion = $"El Admin cancelo la reserva (Cupos antes:{aux.CuposOtorgados} cupos despues: {claseUpdate.CuposOtorgados})",
                        Tipo = Logs_AddAlumnoClase.tipo.CANCELACIONADMIN
                    };
                    this.logService.AddAlumnoClase(logsAlumnoClase);
                }
                this.alumnoClaseRepository.Update(alumnoClaseAUpdate);
                //Agrego el cupo pendiente
                if (alumnoClaseAUpdate.Tipo == AlumnoClase.tipo.FIJO)
                {
                    CupoPendiente cupoPendiente = new CupoPendiente
                    {
                        AlumnoId = alumnoId,
                        FechaExpiracion = DateTime.Now.AddMonths(1),
                        Tipo = CupoPendiente.tipo.CANCELACIONADMIN,
                    };
                    this.AgregarCupoPendiente(cupoPendiente);
                }
            }          
        }

        public void CancelReservaWeb(int alumnoId, int claseId)
        {
            Clase claseUpdate = this.claseRepository.IncludeAll("ClasesAlumno").FirstOrDefault(c => c.Id == claseId);
            Alumno alumno = this.alumnoRepository.IncludeAll().FirstOrDefault(a => a.Id == alumnoId);

            var alumnoClaseAUpdate = claseUpdate.ClasesAlumno.FirstOrDefault(ac => ac.AlumnoId == alumnoId);

            if (alumnoClaseAUpdate != null)
            {
                //claseUpdate.ClasesAlumno.Remove(alumnoClaseAEliminar);
                alumnoClaseAUpdate.Estado = AlumnoClase.estado.CANCELADA;
                alumnoClaseAUpdate.FechaCancelacion = DateTime.Now;
                alumnoClaseAUpdate.Asistio = false;
                claseUpdate.CuposOtorgados--;
                this.alumnoClaseRepository.Update(alumnoClaseAUpdate);
                DateTime fechaActual = DateTime.Now; // Fecha y hora actual
                DateTime limiteCancelacion = claseUpdate.HorarioInicio.AddHours(-2);
                if (alumnoClaseAUpdate.Tipo == AlumnoClase.tipo.FIJO && fechaActual <= limiteCancelacion)
                { 
                    CupoPendiente cupoPendiente = new CupoPendiente
                    {
                        AlumnoId = alumnoId,
                        FechaExpiracion = DateTime.Now.AddMonths(1),
                        Tipo = CupoPendiente.tipo.CANCELACIONWEB,
                    };
                    this.AgregarCupoPendiente(cupoPendiente);
                }
                else if (alumno.PlanId == 39 && fechaActual >= limiteCancelacion)//Alumno Pase Libre
                {
                    this.AgregarFalta(alumnoId, claseId);
                }
            }
            // Registrar un mensaje o lanzar una excepción específica si es necesario
            // Opcional: registrar el error o realizar alguna acción
            Logs_AddAlumnoClase logsAlumnoClase = new Logs_AddAlumnoClase
            {
                AlumnoId = alumnoId,
                ClaseId = claseId,
                Fecha = DateTime.Now,
                Estado = Logs_AddAlumnoClase.estado.PENDIENTE,
                Descripcion = $"CANCELACIÓN WEB",
                Tipo= Logs_AddAlumnoClase.tipo.CANCELACIONWEB
            };
            this.logService.AddAlumnoClase(logsAlumnoClase);
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
        private void CancelarTodasLasClases(int alumnoId)
        {
            var startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);
            var alumnosClases = this.alumnoClaseRepository.IncludeAll().Where(ac => ac.AlumnoId == alumnoId && ac.Fecha>=DateTime.Now.Date && ac.Fecha<= endOfMonth).ToList();
            foreach (AlumnoClase alumnoClase in alumnosClases)
            {
                this.CancelAlumnoClase(alumnoId, alumnoClase.ClaseId);
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
                try
                {
                    bool resultado = this.agregarAlumnoAClase(claseFija.AlumnoId, claseId, AlumnoClase.tipo.FIJO);
                }
                catch (Exception ex)
                {
                    // Manejar la excepción, por ejemplo, registrarla
                    Console.WriteLine($"Error: {ex.Message}");
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
                Logs_AddAlumnoClase.tipo tipoLog;
                if (tipo == AlumnoClase.tipo.WEB)
                    tipoLog = Logs_AddAlumnoClase.tipo.RESERVAWEB;
                else
                    tipoLog = Logs_AddAlumnoClase.tipo.RESERVAADMIN;
                Alumno alumno = this.alumnoRepository.IncludeAllAnidado("Plan.Actividades").FirstOrDefault(a => a.Id == alumnoId);
                Clase clase = this.claseRepository.IncludeAll("ClasesAlumno").FirstOrDefault(c => c.Id == claseId);
                //clase.CuposOtorgados = clase.ClasesAlumno.Count();
                if (alumno == null || clase == null)
                {
                    // Registrar un mensaje o lanzar una excepción específica si es necesario
                    // Opcional: registrar el error o realizar alguna acción
                    Logs_AddAlumnoClase logsAlumnoClase = new Logs_AddAlumnoClase
                    {
                        AlumnoId = alumnoId,
                        ClaseId = claseId,
                        Fecha = DateTime.Now,
                        Estado = Logs_AddAlumnoClase.estado.PENDIENTE,
                        Descripcion = $"No se encontro la clase o el alumno",
                        Tipo=tipoLog
                    };
                    this.logService.AddAlumnoClase(logsAlumnoClase);
                    return false; // Indicar que la operación falló debido a datos no encontrados
                }
                bool existe = clase.ClasesAlumno.Any(ac => ac.AlumnoId == alumnoId && ac.Estado==AlumnoClase.estado.CONFIRMADA);
                if (!existe && alumno.Activo)
                {
                    if (tipo == AlumnoClase.tipo.ADMIN || puedeReservar(alumno, clase, ref tipo))
                    {
                        AlumnoClase alumnoClase = new AlumnoClase
                        {
                            AlumnoId = alumnoId,
                            Tipo = tipo,
                            Estado = AlumnoClase.estado.CONFIRMADA,
                            Fecha = DateTime.Now,
                        };
                        var auxCantAlumnos= clase.ClasesAlumno.Count(ac => ac.Estado == AlumnoClase.estado.CONFIRMADA);
                        if (auxCantAlumnos < clase.CuposTotales)
                        {
                            clase.ClasesAlumno.Add(alumnoClase);
                            clase.CuposOtorgados = auxCantAlumnos+1;
                            this.claseRepository.Update(clase);
                            if (tipo == AlumnoClase.tipo.RECUPERACION)
                            {
                                this.usarCupoPendiente(alumnoId);
                            }
                            return true; // Indicar que la operación fue exitosa
                        }
                        else
                        {
                            // Opcional: registrar el error o realizar alguna acción
                            Logs_AddAlumnoClase logsAlumnoClase = new Logs_AddAlumnoClase
                            {
                                AlumnoId = alumnoId,
                                ClaseId = claseId,
                                Fecha = DateTime.Now,
                                Estado = Logs_AddAlumnoClase.estado.PENDIENTE,
                                Descripcion = $"ERROR en cupos ({auxCantAlumnos})",
                                Tipo = tipoLog
                            };
                            this.logService.AddAlumnoClase(logsAlumnoClase);
                            return false;
                        }
                        
                    }
                    else
                    {
                        // Opcional: registrar el error o realizar alguna acción
                        Logs_AddAlumnoClase logsAlumnoClase = new Logs_AddAlumnoClase
                        {
                            AlumnoId = alumnoId,
                            ClaseId = claseId,
                            Fecha = DateTime.Now,
                            Estado = Logs_AddAlumnoClase.estado.PENDIENTE,
                            Descripcion = $"El alumno no pudo agregarse por su plan, tipo: ({tipo})",
                            Tipo = tipoLog
                        };
                        this.logService.AddAlumnoClase(logsAlumnoClase);
                        return false; // El plan no permite agregar el alumno
                    }
                }
                else
                {
                    Logs_AddAlumnoClase logsAlumnoClase = new Logs_AddAlumnoClase
                    {
                        AlumnoId = alumnoId,
                        ClaseId = claseId,
                        Fecha=DateTime.Now,
                        Estado= Logs_AddAlumnoClase.estado.PENDIENTE,
                        Descripcion = $"El alumno no pudo agregarse a la clase, existe: ({existe}) , activo: ({alumno.Activo}).",
                        Tipo = tipoLog
                    };
                    this.logService.AddAlumnoClase(logsAlumnoClase);
                    return false; // El alumno ya está en la clase o esta inactivo/mantenimiento
                }
        }

        private bool puedeReservar(Alumno alumno, Clase clase, ref AlumnoClase.tipo tipo)
        {
            var totalCupos = clase.ClasesAlumno.Count(ac => ac.Estado == AlumnoClase.estado.CONFIRMADA);
            bool cuposDisponibles = totalCupos < clase.CuposTotales;
            bool actividadEnPlan = alumno.Plan.Actividades.Any(a => a.Id == clase.ActividadId);

            if (!cuposDisponibles)
            {
                throw new InvalidOperationException("No hay cupos disponibles.");
            }
            if (!actividadEnPlan)
            {
                throw new InvalidOperationException("La actividad no está incluida en el plan del alumno.");
            }
            bool puede = false;

            switch (alumno.Plan.Tipo)
            {
                case Plan.tipo.SEMANAL:
                    // Verificar si tiene cupos semanales o ilimitados
                    bool puedeReservarSemanal = this.GetMisReservasSemana(alumno.Id, clase.HorarioInicio) < alumno.Plan.VecesxSemana || alumno.Plan.VecesxSemana == 0;

                    if (cuposDisponibles && puedeReservarSemanal && actividadEnPlan && tipo != AlumnoClase.tipo.RECUPERACION)
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
                    bool tieneReservaenElDia = this.ReservaToday(alumno.Id, clase.HorarioInicio.Date);
                    if (cuposDisponibles && puedeReservarMensual && actividadEnPlan && faltas<2 && !tieneReservaenElDia)
                        puede = true;
                    break;
                case Plan.tipo.TU_PASE:
                    // Pase sin restricciones de cantidad
                    if (cuposDisponibles && actividadEnPlan && tipo != AlumnoClase.tipo.WEB)
                        puede = true;
                    break;
            }
            return puede;
        }

        public IEnumerable<AlumnoClaseDTO> GetMisReservas(int idAlumno)
        {
            DateTime today = DateTime.Now;

            // Obtener el primer día del mes actual
            DateTime startOfMonth = new DateTime(today.Year, today.Month, 1);

            // Obtener el día de inicio de la semana actual (lunes)
            int daysUntilMonday = ((int)today.DayOfWeek - (int)DayOfWeek.Monday + 7) % 7;
            DateTime startOfWeek = today.AddDays(-daysUntilMonday);

            // Obtener el sábado de la siguiente semana (lunes + 12 días)
            DateTime endOfNextWeek = startOfWeek.AddDays(12); // Lunes + 12 días = siguiente sábado

            // Filtrar las clases entre el lunes de la semana actual y el sábado de la siguiente semana
            var alumnoClases = this.alumnoClaseRepository.IncludeAllAnidado("Clase", "Clase.Actividad", "Clase.Local", "Alumno")
                                    .Where(ac => ac.AlumnoId == idAlumno
                                            && ac.Clase.HorarioInicio >= startOfMonth
                                            && ac.Clase.HorarioInicio <= endOfNextWeek
                                             && (ac.Estado == AlumnoClase.estado.CONFIRMADA || ac.Estado == AlumnoClase.estado.CANCELADA))
                                    .OrderBy(ac => ac.Clase.HorarioInicio);

            return this.mapper.Map<IEnumerable<AlumnoClaseDTO>>(alumnoClases);
        }
        public bool ReservaToday(int alumnoId, DateTime day)
        {
            // Obtener solo la fecha sin la hora
            DateTime dayStart = day.Date;
            DateTime dayEnd = dayStart.AddDays(1).AddTicks(-1); // Fin del día

            var count = this.alumnoClaseRepository.IncludeAll("Clase")
                .Where(ac => ac.AlumnoId == alumnoId &&
                             ac.Clase.HorarioInicio >= dayStart &&
                             ac.Clase.HorarioInicio <= dayEnd && // Comparar dentro del mismo día
                             ac.Estado == AlumnoClase.estado.CONFIRMADA)
                .Count();

            return count > 0; // Devuelve true si hay al menos una reserva
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
                                 ac.Clase.HorarioFin <= endOfWeek
                                 && ac.Estado == AlumnoClase.estado.CONFIRMADA
                                 && ac.Tipo != AlumnoClase.tipo.RECUPERACION)
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
                             ac.Clase.HorarioFin <= endOfMonth
                             && ac.Estado == AlumnoClase.estado.CONFIRMADA)
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
            Alumno alumno = this.alumnoRepository.IncludeAll("Faltas","Plan").FirstOrDefault(a => a.Id == alumnoId);
            if (alumno != null)
                {
                    // Agregar la nueva falta
                    var nuevaFalta = new Falta
                    {
                        Fecha = DateTime.Now,
                        AlumnoId = alumnoId,
                        ClaseId=claseId
                    };
                bool tengo = alumno.Faltas.Any(f => f.ClaseId == claseId);
                if (!tengo)
                {
                    alumno.Faltas.Add(nuevaFalta);
                    this.alumnoRepository.Update(alumno);
                    int faltas = this.ObtenerFaltasDelMes(alumno.Id, DateTime.Now);
                    if (alumno.Plan.Tipo == Plan.tipo.PASE_LIBRE && faltas>=2)
                    {
                        this.CancelarTodasLasClases(alumnoId);
                    }
                }
            }
            var alumnoClaseAUpdate=this.alumnoClaseRepository.GetAll().FirstOrDefault(ac => ac.AlumnoId == alumnoId && ac.ClaseId==claseId);
            if (alumnoClaseAUpdate != null)
            {
                alumnoClaseAUpdate.Asistio = false;
                this.alumnoClaseRepository.Update(alumnoClaseAUpdate);
            }

        }

        public void QuitarFalta(int alumnoId, int claseId)
        {
            Alumno alumno = this.alumnoRepository.IncludeAll("Faltas").FirstOrDefault(a => a.Id == alumnoId);
            if (alumno != null)
            {

                var falta=alumno.Faltas.FirstOrDefault(f => f.ClaseId == claseId);
                alumno.Faltas.Remove(falta);
                this.alumnoRepository.Update(alumno);
            }
            var alumnoClaseAUpdate = this.alumnoClaseRepository.GetAll().FirstOrDefault(ac => ac.AlumnoId == alumnoId && ac.ClaseId == claseId);
            if (alumnoClaseAUpdate != null)
            {
                alumnoClaseAUpdate.Asistio = true;
                this.alumnoClaseRepository.Update(alumnoClaseAUpdate);
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

        public void AgregarCupoPendiente(CupoPendiente cupoPendiente)
        {
            Alumno alumno = this.alumnoRepository.IncludeAll("Cupos").FirstOrDefault(a => a.Id == cupoPendiente.AlumnoId);
            if (alumno != null)
            {
                // Agregar la nueva falta
                var cupo = new CupoPendiente
                {
                    FechaCreacion = DateTime.Now,
                    FechaExpiracion = cupoPendiente.FechaExpiracion,
                    AlumnoId = cupoPendiente.AlumnoId,
                    Tipo = cupoPendiente.Tipo,
                    Estado = CupoPendiente.estado.PENDIENTE,
                };
                alumno.Cupos.Add(cupo);
                alumno.CuposPendientes= alumno.Cupos.Count(c => c.Estado == CupoPendiente.estado.PENDIENTE);
                this.alumnoRepository.Update(alumno);
            }
        }

        public void usarCupoPendiente(int alumnoId)
        {
            Alumno alumno = this.alumnoRepository.IncludeAll("Cupos").FirstOrDefault(a => a.Id == alumnoId);
            if (alumno != null)
            {
                CupoPendiente cupoAux = this.ObtenerCupoPendienteMasCercano(alumno);
                if (cupoAux != null)
                {
                    cupoAux.Estado = CupoPendiente.estado.UTILIZADO;
                    if(alumno.CuposPendientes>0)
                        alumno.CuposPendientes--;
                    this.alumnoRepository.Update(alumno);
                }
                else
                {
                    throw new Exception("El alumno no posee cupos pendientes.");
                }
            }
            else
            {
                throw new Exception("El alumno no existe.");
            }
        }
        public CupoPendiente ObtenerCupoPendienteMasCercano(Alumno alumno)
        {
            // Filtrar primero los cupos pendientes con fecha de expiración y ordenarlos por la fecha más cercana
            var cupoPendienteMasCercanoConFecha = alumno.Cupos
                .Where(c => c.Estado == CupoPendiente.estado.PENDIENTE && c.FechaExpiracion.HasValue) // Solo considerar los que tienen FechaExpiracion
                .OrderBy(c => c.FechaExpiracion) // Ordenar por la fecha de expiración más cercana
                .FirstOrDefault();

            // Si no se encuentra cupo con fecha de expiración, buscar el primer cupo sin fecha
            if (cupoPendienteMasCercanoConFecha == null)
            {
                var cupoPendienteSinFecha = alumno.Cupos
                    .Where(c => c.Estado == CupoPendiente.estado.PENDIENTE && !c.FechaExpiracion.HasValue) // Solo los que no tienen FechaExpiracion
                    .FirstOrDefault();

                return cupoPendienteSinFecha;
            }

            return cupoPendienteMasCercanoConFecha;
        }

        public int CuposPendientes(int alumnoId)
        {
            Alumno alumno = this.alumnoRepository.IncludeAll("Plan").FirstOrDefault(a => a.Id == alumnoId);
            if (alumno != null && alumno.Plan.VecesxMes!=null)
            {
                return (int)(alumno.Plan.VecesxMes-this.GetMisReservasMes(alumnoId,DateTime.Now));
            }
            else
            {
                throw new Exception("El alumno no existe, o el plan no lo permite");
            }
        }
        public IEnumerable<CupoPendienteDTO> CuposRecuperacion(int alumnoId)
        {
            Alumno alumno = this.alumnoRepository.IncludeAll("Cupos").FirstOrDefault(a => a.Id == alumnoId);
            if (alumno != null)
            {
                return this.mapper.Map<IEnumerable<CupoPendienteDTO>>(alumno.Cupos.Where(c => c.Estado == CupoPendiente.estado.PENDIENTE && c.FechaExpiracion?.Date >= DateTime.Today.Date));
            }
            else
            {
                throw new Exception("El alumno no existe.");
            }
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
