using Commons.Exceptions;
using Dto;
using Microsoft.AspNetCore.Mvc;
using ServicesInterface;
using System;

namespace WebApi.Controllers
{
    [Route("api/clases")]
    [ApiController]
    public class ClaseController : ControllerBase
    {
        private IClaseService claseService;
        public ClaseController(IClaseService claseService)
        {
            this.claseService = claseService;
        }
        [HttpPost("{actividadId}")]
        public IActionResult Post([FromRoute] int actividadId, [FromBody] ClaseDTO claseDTO)
        {
            try
            {
                var clase = this.claseService.Add(actividadId,claseDTO);
                return Ok(clase);
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
        [HttpPost("copy/{localId}/{fechaDesde}/{fechaTo}")]
        public IActionResult CopyTo([FromRoute] int localId,DateTime fechaDesde, DateTime fechaTo)
        {
            try
            {
                this.claseService.CopyTo(localId,fechaDesde, fechaTo);
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

        [HttpGet("{claseId}")]
        public IActionResult GetId([FromRoute] int claseId)
        {
            try
            {
                var clase = this.claseService.GetId(claseId);
                return Ok(clase);
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

        [HttpGet("alumnos/{claseId}")]
        public IActionResult GetAlumnos([FromRoute] int claseId)
        {
            try
            {
                var alumnosClase = this.claseService.GetAlumnos(claseId);
                return Ok(alumnosClase);
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

        [HttpGet("between/{actividadId}/{fechaDesde}/{fechaTo}")]
        public IActionResult Between([FromRoute] int actividadId, DateTime fechaDesde, DateTime fechaTo)
        {
            try
            {
                var clases=this.claseService.Between(actividadId,fechaDesde, fechaTo);
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
            catch (System.Exception exception)
            {
                return StatusCode(500, "Algo salió mal.(" + exception.Message + ")");
            }
        }

        [HttpGet("between/{alumnoId}/{actividadId}/{fechaDesde}/{fechaTo}")]
        public IActionResult ActividadesParaReservar([FromRoute] int alumnoId, int actividadId, DateTime fechaDesde, DateTime fechaTo)
        {
            try
            {
                var clases = this.claseService.ActividadesParaReservar(alumnoId,actividadId, fechaDesde, fechaTo);
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
            catch (System.Exception exception)
            {
                return StatusCode(500, "Algo salió mal.(" + exception.Message + ")");
            }
        }
        [HttpPut]
        public IActionResult Put([FromBody] ClaseDTO claseDTOUpdate)
        {
            try
            {
                var clase = this.claseService.Update(claseDTOUpdate);
                return Ok(clase);
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
        [HttpDelete("{claseId}")]
        public IActionResult Delete(int claseId)
        {
            try
            {
                this.claseService.Remove(claseId);
                return Ok("Se elimino la clase.");
            }
            catch (ValidationException ve)
            {
                return NotFound(ve.Message);
            }
        }
    }
}
