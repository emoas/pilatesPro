using Commons.Exceptions;
using Dto;
using Microsoft.AspNetCore.Mvc;
using ServicesInterface;
using System;

namespace WebApi.Controllers
{
    [Route("api/actividades")]
    [ApiController]
    public class ActividadController : ControllerBase
    {
        private IActividadService actividadService;
        public ActividadController(IActividadService actividadService)
        {
            this.actividadService = actividadService;
        }
        [HttpPost]
        public IActionResult Post([FromBody] ActividadDTO actividadDTO)
        {
            try
            {
                var profe = this.actividadService.Add(actividadDTO);
                return Ok(profe);
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
                var actividades = this.actividadService.GetAll();
                return Ok(actividades);
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

        [HttpGet("{actividadId}")]
        public IActionResult GetId([FromRoute] int actividadId)
        {
            try
            {
                var actividad = this.actividadService.GetId(actividadId);
                return Ok(actividad);
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
        [HttpGet("light/{actividadId}")]
        public IActionResult GetLightId([FromRoute] int actividadId)
        {
            try
            {
                var actividad = this.actividadService.GetLightId(actividadId);
                return Ok(actividad);
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
        [HttpGet("profesores/{actividadId}")]
        public IActionResult GetProfesores([FromRoute] int actividadId)
        {
            try
            {
                var profesores = this.actividadService.GetProfesores(actividadId);
                return Ok(profesores);
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
        [HttpGet("clases/{actividadId}/{desde}/{hasta}")]
        public IActionResult GetClases([FromRoute] int actividadId, DateTime desde, DateTime hasta)
        {
            try
            {
                var clases = this.actividadService.GetClases(actividadId, desde,hasta);
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

        [HttpGet("local/{localId}")]
        public IActionResult GetPorLocal([FromRoute] int localId)
        {
            try
            {
                var actividades = this.actividadService.GetPorLocal(localId);
                return Ok(actividades);
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
        [HttpDelete("{actividadId}")]
        public IActionResult Desactivate(int actividadId)
        {
            try
            {
                this.actividadService.Remove(actividadId);
                return Ok("Se elimino la actividad.");
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
        [HttpPut("{id}")]
        public IActionResult Put([FromRoute] int id, [FromBody] ActividadDTO actividadDTOUpdate)
        {
            try
            {
                actividadDTOUpdate.Id = id;
                var actividad = this.actividadService.Update(actividadDTOUpdate);
                return Ok(actividad);
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
        [HttpPut("activar/{id}")]
        public IActionResult Activar([FromRoute] int id, [FromBody] bool activar)
        {
            try
            {
                var actividad = this.actividadService.Activar(id,activar);
                return Ok(actividad);
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
    }
}
