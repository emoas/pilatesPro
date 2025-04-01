using Commons.Exceptions;
using Dto;
using Dto.Alumnos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicesInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/alumnos")]
    [ApiController]
    public class AlumnoController : Controller
    {
        private IAlumnoService alumnoService;
        public AlumnoController(IAlumnoService alumnoService)
        {
            this.alumnoService = alumnoService;
        }
        [HttpPost]
        public IActionResult Post([FromBody] AlumnoDTO alumnoDTO)
        {
            try
            {
                var alumno = this.alumnoService.Add(alumnoDTO);
                return Ok(alumno);
            }
            catch (System.ArgumentException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (ValidationException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (System.Exception exception)
            {
                return StatusCode(500, "Algo salió mal.(" + exception.Message + ")");
            }
        }
        [HttpPost("addtoclase")]
        public IActionResult AddAlumnoClase([FromBody] AlumnoClaseDTO alumnoClaseDTO)
        {
            try
            {
                this.alumnoService.AddAlumnoClase(alumnoClaseDTO);
                return Ok("Se agrego correctamente");
            }
            catch (System.ArgumentException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (ValidationException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (System.Exception exception)
            {
                return StatusCode(500, "Algo salió mal.(" + exception.Message + ")");
            }
        }
        [HttpPost("addFalta/{idAlumno}/{claseId}")]
        public IActionResult AddFalta([FromRoute] int idAlumno,int claseId)
        {
            try
            {
                this.alumnoService.AgregarFalta(idAlumno, claseId);
                return Ok("Se agrego correctamente la falta");
            }
            catch (System.ArgumentException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (ValidationException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (System.Exception exception)
            {
                return StatusCode(500, "Algo salió mal.(" + exception.Message + ")");
            }
        }

        [HttpPost("fija/{idAlumno}")]
        public IActionResult AddClaseFija([FromRoute] int idAlumno, [FromBody] ClaseFijaDTO claseFijaDTO)
        {
            try
            {
                var claseFija = this.alumnoService.AddClaseFija(idAlumno, claseFijaDTO);
                return Ok(claseFija);
            }
            catch (System.ArgumentException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (ValidationException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (System.Exception exception)
            {
                return StatusCode(500, "Algo salió mal.(" + exception.Message + ")");
            }
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var alumnos = this.alumnoService.GetAll();
                return Ok(alumnos);
            }
            catch (System.ArgumentException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (ValidationException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (System.Exception exception)
            {
                return StatusCode(500, "Algo salió mal.");
            }
        }

        [HttpGet("{alumnoId}")]
        public IActionResult GetId([FromRoute] int alumnoId)
        {
            try
            {
                var alumno = this.alumnoService.GetId(alumnoId);
                return Ok(alumno);
            }
            catch (System.ArgumentException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (ValidationException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (System.Exception)
            {
                return StatusCode(500, "Algo salió mal.");
            }
        }
        [HttpGet("fijas/{idAlumno}")]
        public IActionResult GetFijasAlumno([FromRoute] int idAlumno)
        {
            try
            {
                var clases = this.alumnoService.GetFijasAlumno(idAlumno);
                return Ok(clases);
            }
            catch (System.ArgumentException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (ValidationException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (System.Exception)
            {
                return StatusCode(500, "Algo salió mal.");
            }
        }
        [HttpGet("cuposPendientes/{idAlumno}")]
        public IActionResult GetCuposPendientes([FromRoute] int idAlumno)
        {
            try
            {
                var cupos = this.alumnoService.CuposPendientes(idAlumno);
                return Ok(cupos);
            }
            catch (System.ArgumentException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (ValidationException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (System.Exception)
            {
                return StatusCode(500, "Algo salió mal.");
            }
        }
        [HttpGet("cuposRecuperacion/{idAlumno}")]
        public IActionResult GetCuposRecuperacion([FromRoute] int idAlumno)
        {
            try
            {
                var cupos = this.alumnoService.CuposRecuperacion(idAlumno);
                return Ok(cupos);
            }
            catch (System.ArgumentException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (ValidationException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (System.Exception)
            {
                return StatusCode(500, "Algo salió mal.");
            }
        }
        [HttpGet("misreservas/{idAlumno}")]
        public IActionResult GetMisReservas([FromRoute] int idAlumno)
        {
            try
            {
                var clases = this.alumnoService.GetMisReservas(idAlumno);
                return Ok(clases);
            }
            catch (System.ArgumentException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (ValidationException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (System.Exception)
            {
                return StatusCode(500, "Algo salió mal.");
            }
        }
        [HttpGet("reservasPeriodo/{idAlumno}/{desde}/{hasta}")]
        public IActionResult GetReservasPeriodo([FromRoute] int idAlumno, DateTime desde, DateTime hasta)
        {
            try
            {
                var clases = this.alumnoService.GetReservasPeriodo(idAlumno, desde,hasta);
                return Ok(clases);
            }
            catch (System.ArgumentException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (ValidationException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (System.Exception)
            {
                return StatusCode(500, "Algo salió mal.");
            }
        }
        [HttpGet("faltas/{idAlumno}/{fecha}")]
        public IActionResult GetFaltas([FromRoute] int idAlumno,DateTime fecha)
        {
            try
            {
                var total = this.alumnoService.ObtenerFaltasDelMes(idAlumno,fecha);
                return Ok(total);
            }
            catch (System.ArgumentException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (ValidationException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (System.Exception)
            {
                return StatusCode(500, "Algo salió mal.");
            }
        }

        [HttpDelete("{alumnoId}")]
        public IActionResult Desactivate(int alumnoId)
        {
            try
            {
                this.alumnoService.Desactivate(alumnoId);
                return Ok("Se elimino el alumno.");
            }
            catch (System.ArgumentException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (ValidationException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, "Algo salió mal.");
            }
        }
        [HttpDelete("{alumnoId}/clase/{claseId}")]
        public IActionResult RemoveAlumnoClase([FromRoute] int alumnoId, int claseId)
        {
            try
            {
                this.alumnoService.RemoveAlumnoClase(alumnoId,claseId);
                return Ok("Se elimino el alumno.");
            }
            catch (System.ArgumentException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (ValidationException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (System.Exception)
            {
                return StatusCode(500, "Algo salió mal.");
            }
        }

        [HttpDelete("{alumnoId}/cancel/{claseId}")]
        public IActionResult CancelReservaWeb([FromRoute] int alumnoId, int claseId)
        {
            try
            {
                this.alumnoService.CancelReservaWeb(alumnoId, claseId);
                return Ok("Se cancelo reserva.");
            }
            catch (System.ArgumentException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (ValidationException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (System.Exception exception)
            {
                return StatusCode(500, "Algo salió mal.");
            }
        }

        [HttpDelete("cancelmanual/{idAlumnoClase}/{addFalta}")]
        public IActionResult CancelReservaManual([FromRoute] int idAlumnoClase,bool addFalta)
        {
            try
            {
                this.alumnoService.CancelReservaManual(idAlumnoClase, addFalta);
                return Ok("Se cancelo reserva.");
            }
            catch (System.ArgumentException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (ValidationException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (System.Exception exception)
            {
                return StatusCode(500, "Algo salió mal.");
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put([FromRoute] int id, [FromBody] AlumnoDTO alumnoDTOUpdate)
        {
            try
            {
                alumnoDTOUpdate.Id = id;
                var alumno = this.alumnoService.Update(alumnoDTOUpdate);
                return Ok(alumno);
            }
            catch (System.ArgumentException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (ValidationException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (System.Exception exception)
            {
                return StatusCode(500, "Algo salió mal.");
            }
        }
        [HttpPut("fija/{id}")]
        public IActionResult UpdateClaseFija([FromRoute] int id, [FromBody] ClaseFijaDTO claseFijaDTO)
        {
            try
            {
                var claseFija = this.alumnoService.UpdateClaseFija(id, claseFijaDTO);
                return Ok(claseFija);
            }
            catch (System.ArgumentException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (ValidationException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (System.Exception exception)
            {
                return StatusCode(500, "Algo salió mal.(" + exception.Message + ")");
            }
        }

        [HttpPut("updateClasesAlumno/{claseId}")]
        public IActionResult UpdateClasesAlumno([FromRoute] int claseId)
        {
            try
            {
                this.alumnoService.UpdateClasesAlumno(claseId);
                return Ok();
            }
            catch (System.ArgumentException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (ValidationException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (System.Exception exception)
            {
                return StatusCode(500, "Algo salió mal.(" + exception.Message + ")");
            }
        }
        [HttpDelete("fija/{id}")]
        public IActionResult RemoveClaseFija(int id)
        {
            try
            {
                this.alumnoService.RemoveClaseFija(id);
                return Ok("Se elimino la clase fija.");
            }
            catch (System.ArgumentException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (ValidationException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (System.Exception)
            {
                return StatusCode(500, "Algo salió mal.");
            }
        }
        [HttpDelete("delFalta/{idAlumnoClase}")]
        public IActionResult DeleteFalta([FromRoute] int idAlumnoClase)
        {
            try
            {
                this.alumnoService.QuitarFalta(idAlumnoClase);
                return Ok("Se elimino correctamente la falta");
            }
            catch (System.ArgumentException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (ValidationException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (System.Exception exception)
            {
                return StatusCode(500, "Algo salió mal.(" + exception.Message + ")");
            }
        }
        [HttpPost("licencia")]
        public IActionResult AddLicencia([FromBody] LicenciaAlumnoDTO licenciaDTO)
        {
            try
            {
                this.alumnoService.AgregarLicencia(licenciaDTO);
                return Ok("Se agrego correctamente");
            }
            catch (System.ArgumentException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (ValidationException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (System.Exception exception)
            {
                return StatusCode(500, "Algo salió mal.(" + exception.Message + ")");
            }
        }
        [HttpGet("licencia/{idAlumno}")]
        public IActionResult GetLicencia([FromRoute] int idAlumno)
        {
            try
            {
                var alumno=this.alumnoService.GetLicenciaAlumno(idAlumno);
                return Ok(alumno);
            }
            catch (System.ArgumentException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (ValidationException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (System.Exception exception)
            {
                return StatusCode(500, "Algo salió mal.(" + exception.Message + ")");
            }
        }
        [HttpGet("licencia/{idAlumno}/{fecha}")]
        public IActionResult GetEstaDeLicencia([FromRoute] int idAlumno, DateTime fecha)
        {
            try
            {
                var esta = this.alumnoService.EstaDeLicencia(idAlumno,fecha);
                return Ok(esta);
            }
            catch (System.ArgumentException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (ValidationException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (System.Exception exception)
            {
                return StatusCode(500, "Algo salió mal.(" + exception.Message + ")");
            }
        }

        [HttpDelete("licencia/{idLicencia}")]
        public IActionResult DeleteLicencia([FromRoute] int idLicencia)
        {
            try
            {
                this.alumnoService.EliminarLicencia(idLicencia);
                return Ok("Se elimino correctamente la licencia");
            }
            catch (System.ArgumentException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (ValidationException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (System.Exception exception)
            {
                return StatusCode(500, "Algo salió mal.(" + exception.Message + ")");
            }
        }
    }
}
