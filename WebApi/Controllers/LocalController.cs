using Commons.Exceptions;
using Dto;
using Microsoft.AspNetCore.Mvc;
using ServicesInterface;


namespace WebApi.Controllers
{
    [Route("api/locales")]
    [ApiController]
    public class LocalController : ControllerBase
    {
        private ILocalService localService;
        public LocalController(ILocalService localService)
        {
            this.localService = localService;
        }
        [HttpPost]
        public IActionResult Post([FromBody] LocalDTO localDTO)
        {
            try
            {
                var local = this.localService.Add(localDTO);
                return Ok(local);
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
                var locales = this.localService.GetAll();
                return Ok(locales);
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

        [HttpGet("{localId}")]
        public IActionResult GetId([FromRoute] int localId)
        {
            try
            {
                var local = this.localService.GetId(localId);
                return Ok(local);
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

        [HttpDelete("{localId}")]
        public IActionResult Desactivate(int localId)
        {
            try
            {
                this.localService.Remove(localId);
                return Ok("Se elimino el local.");
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
        public IActionResult Put([FromRoute] int id, [FromBody] LocalDTO localDTOUpdate)
        {
            try
            {
                localDTOUpdate.Id = id;
                var local = this.localService.Update(localDTOUpdate);
                return Ok(local);
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
