using Commons.Exceptions;
using Dto;
using Microsoft.AspNetCore.Mvc;
using ServicesInterface;
using System;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/sessions")]
    public class SessionController : ControllerBase
    {
        private readonly ISessionService sessionService;

        public SessionController(ISessionService sessionService)
        {
            this.sessionService = sessionService;
        }

        /// <summary>
        /// Login the users.
        /// </summary>
        /// <param name="userLogin"></param>
        /// <response code="200">Returns the token for the session</response>
        /// <response code="404">If the user credentials are invalid</response>
        [HttpPost]
        [ProducesResponseType(typeof(Guid), 200)]
        public IActionResult Login([FromBody] UserLoginDTO userLogin)
        {
            try
            {
                var token = this.sessionService.Login(userLogin);
                return Ok(token);
            }
            catch (ValidationException ve)
            {
                return NotFound(ve.Message);
            }
        }


        [HttpGet("{userName}")]
        public IActionResult GetUser([FromRoute] string userName)
        {
            try
            {
                var user = this.sessionService.GetUser(userName);
                return Ok(user);
            }
            catch (ValidationException ve)
            {
                return BadRequest(ve.Message);
            }
            catch (ArgumentException ae)
            {
                return BadRequest(ae.Message);
            }
        }
    }
}
