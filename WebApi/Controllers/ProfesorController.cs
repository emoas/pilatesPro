using Commons.Exceptions;
using Dto;
using Microsoft.AspNetCore.Mvc;
using ServicesInterface;
using System;

namespace WebApi.Controllers
{
    [Route("api/profesores")]
    [ApiController]
    public class ProfesorController : Controller
    {
        private IProfesorService profeService;
        public ProfesorController(IProfesorService profeService)
        {
            this.profeService = profeService;
        }
        [HttpPost]
        public IActionResult Post([FromBody] ProfesorDTO profeDTO)
        {
            try
            {
                var profe = this.profeService.Add(profeDTO);
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
                var profesores = this.profeService.GetAll();
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
            catch (System.Exception exception)
            {
                return StatusCode(500, "Algo salió mal.");
            }
        }

        [HttpGet("{profeId}")]
        public IActionResult GetId([FromRoute] int profeId)
        {
            try
            {
                var profe = this.profeService.GetId(profeId);
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
            catch (System.Exception)
            {
                return StatusCode(500, "Algo salió mal.");
            }
        }
        [HttpGet("clases/{profeId}/{desde}/{hasta}")]
        public IActionResult GetClases([FromRoute] int profeId, DateTime desde, DateTime hasta)
        {
            try
            {
                var agendas = this.profeService.GetClases(profeId, desde, hasta);
                return Ok(agendas);
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
                return StatusCode(500, "Algo salió mal: "+ exception.Message);
            }
        }

        [HttpDelete("{profeId}")]
        public IActionResult Desactivate(int profeId)
        {
            try
            {
                this.profeService.Remove(profeId);
                return Ok("Se elimino el profesor.");
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
        public IActionResult Put([FromRoute] int id, [FromBody] ProfesorDTO profeDTOUpdate)
        {
            try
            {
                profeDTOUpdate.Id = id;
                var profe = this.profeService.Update(profeDTOUpdate);
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
                return StatusCode(500, "Algo salió mal.");
            }
        }
    }
}
