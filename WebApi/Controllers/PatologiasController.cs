using Commons.Exceptions;
using Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicesInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/patologias")]
    [ApiController]
    public class PatologiasController : ControllerBase
    {
        private IPatologiaService patologiaService;
        public PatologiasController(IPatologiaService patologiaService)
        {
            this.patologiaService = patologiaService;
        }
        [HttpPost]
        public IActionResult Post([FromBody] PatologiaDTO patologiaDTO)
        {
            try
            {
                var patologia = this.patologiaService.Add(patologiaDTO);
                return Ok(patologia);
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
        public IActionResult GetAll()
        {
            try
            {
                var patologias = this.patologiaService.GetAll();
                return Ok(patologias);
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

        [HttpGet("{patologiaId}")]
        public IActionResult GetId([FromRoute] int patologiaId)
        {
            try
            {
                var patologia = this.patologiaService.GetId(patologiaId);
                return Ok(patologia);
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

        [HttpDelete("{patologiaId}")]
        public IActionResult Desactivate(int patologiaId)
        {
            try
            {
                this.patologiaService.Remove(patologiaId);
                return Ok("Se elimino la patologia.");
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
        public IActionResult Put([FromRoute] int id, [FromBody] PatologiaDTO patologiaDTOUpdate)
        {
            try
            {
                patologiaDTOUpdate.Id = id;
                var patologia = this.patologiaService.Update(patologiaDTOUpdate);
                return Ok(patologia);
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
    }
}
