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
    [Route("api/planes")]
    [ApiController]
    public class PlanController : ControllerBase
    {
        private IPlanService planService;
        public PlanController(IPlanService planService)
        {
            this.planService = planService;
        }
        [HttpPost]
        public IActionResult Post([FromBody] PlanDTO planDTO)
        {
            try
            {
                var plan = this.planService.Add(planDTO);
                return Ok(plan);
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
                var planes = this.planService.GetAll();
                return Ok(planes);
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

        [HttpGet("{planId}")]
        public IActionResult GetId([FromRoute] int planId)
        {
            try
            {
                var plan = this.planService.GetId(planId);
                return Ok(plan);
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

        [HttpDelete("{planId}")]
        public IActionResult Desactivate(int planId)
        {
            try
            {
                this.planService.Remove(planId);
                return Ok("Se elimino el plan.");
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
        public IActionResult Put([FromRoute] int id, [FromBody] PlanDTO planDTOUpdate)
        {
            try
            {
                planDTOUpdate.Id = id;
                var plan = this.planService.Update(planDTOUpdate);
                return Ok(plan);
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

        [HttpPut("activar/{id}")]
        public IActionResult Activar([FromRoute] int id, [FromBody] bool activar)
        {
            try
            {
                var plan = this.planService.Activar(id, activar);
                return Ok(plan);
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
