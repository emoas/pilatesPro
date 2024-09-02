using Commons.Exceptions;
using Microsoft.AspNetCore.Mvc;
using ServicesInterface;


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
    }
}
