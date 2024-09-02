using Commons.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicesInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/agenda")]
    [ApiController]
    public class AgendaController : ControllerBase
    {
        private IAgendaService agendaService;
        public AgendaController(IAgendaService agendaService)
        {
            this.agendaService = agendaService;
        }

        [HttpGet("local/{localId}")]
        public IActionResult GetLocalId([FromRoute] int localId)
        {
            try
            {
                var agendas = this.agendaService.GetLocalId(localId);
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
                return StatusCode(500, "Algo salió mal.");
            }
        }
    }
}
