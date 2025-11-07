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
    [Route("api/reports")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private IReportService reportService;
        public ReportController(IReportService reportService)
        {
            this.reportService = reportService;
        }

        [HttpGet("reservas/{planId}/{desde}/{hasta}")]
        public IActionResult GetReservasPorPLan([FromRoute] int planId, DateTime desde, DateTime hasta)
        {
            try
            {
                var reservas = this.reportService.GetReservasPorPLan(planId,desde,hasta);
                return Ok(reservas);
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
        [HttpGet("reservasTipo/{tipo}/{desde}/{hasta}")]
        public IActionResult GetReservasPorTipoPLan([FromRoute] int tipo, DateTime desde, DateTime hasta)
        {
            try
            {
                var reservas = this.reportService.GetReservasPorTipoPLan(tipo, desde, hasta);
                return Ok(reservas);
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
