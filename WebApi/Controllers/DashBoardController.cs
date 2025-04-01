using Commons.Exceptions;
using Microsoft.AspNetCore.Mvc;
using ServicesInterface;
using System;

namespace WebApi.Controllers
{
    [Route("api/dashboard")]
    [ApiController]
    public class DashBoardController : ControllerBase
    {
        private IDashBoardService dashBoardService;
        public DashBoardController(IDashBoardService dashBoardService)
        {
            this.dashBoardService = dashBoardService;
        }
        [HttpGet]
        public IActionResult GetHome()
        {
            try
            {
                var dashboard = this.dashBoardService.GetHome();
                return Ok(dashboard);
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
        [HttpGet("clasesLocalFecha/{idLocal}/{fecha}")]
        public IActionResult GetClasesLocalFecha([FromRoute] int idLocal, DateTime fecha)
        {
            try
            {
                var agenda = this.dashBoardService.GetClasesLocalFecha(idLocal, fecha);
                return Ok(agenda);
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
        [HttpGet("clasesProfeFecha/{idProfe}/{fecha}")]
        public IActionResult GetClasesProfeFecha([FromRoute] int idProfe, DateTime fecha)
        {
            try
            {
                var clases = this.dashBoardService.GetClasesProfeFecha(idProfe, fecha);
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
                return StatusCode(500, "Algo salió mal.");
            }
        }
        [HttpGet("paseLibre/2faltas")]
        public IActionResult GetPaseLibre2Faltas([FromRoute] int planId, DateTime desde, DateTime hasta)
        {
            try
            {
                var reservas = this.dashBoardService.GetPaseLibre2Faltas();
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
