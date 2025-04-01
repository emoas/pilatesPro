using Commons.Exceptions;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicesInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/notyfy")]
    [ApiController]
    public class NotifyController : ControllerBase
    {
        private ILocalService localService;
        public NotifyController(ILocalService localService)
        {
            this.localService = localService;
        }

        [HttpPost("send-template-message")]
        public async Task<IActionResult> SendMessage([FromBody] WhatsAppMessage message)
        {
            try
            {
                await localService.SendMessageAsync(message);
                return Ok("Mensaje enviado exitosamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}