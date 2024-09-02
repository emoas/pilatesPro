using Commons.Exceptions;
using Dto;
using Microsoft.AspNetCore.Mvc;
using ServicesInterface;
using System;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        /// <summary>
        /// Get all users.
        /// </summary>
        /// <remarks>
        /// Needs Authorization Header, with Key={Authorization} And Value={Guid}
        /// </remarks>
        /// <response code="200">Returns the users</response>
        /// <response code="401">If the user is not logged in or the token format is not valid</response>
        /// <response code="403">If the token is not valid</response>
        [HttpGet("{token}")]
        //[ServiceFilter(typeof(AuthorizationAttributeFilter))]
        public IActionResult Get([FromRoute] Guid token)
        {
            try
            {
                var user = this.userService.Get(token);
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

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <remarks>
        /// Needs Authorization Header, with Key={Authorization} And Value={Guid}
        /// </remarks>
        /// <param name="userCreate"></param>
        /// <response code="200">Returns the newly user</response>
        /// <response code="400">If parameters of the DTO are invalid or the user already exists in the system</response>
        /// <response code="401">If the user is not logged in or the token format is not valid</response>
        /// <response code="403">If the token is not valid</response>
        [HttpPost]
        [ProducesResponseType(typeof(UserDTO), 200)]
        //[ServiceFilter(typeof(AuthorizationAttributeFilter))]
        public IActionResult Post([FromBody] UserDTO userCreate)
        {
            try
            {
                var user = this.userService.Add(userCreate);
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
            catch (System.Exception exception)
            {
                return StatusCode(500, "Algo salió mal.(" + exception.Message + ")");
            }
        }

        /// <summary>
        /// Updates a user.
        /// </summary>
        /// <remarks>
        /// Needs Authorization Header, with Key={Authorization} And Value={Guid}
        /// </remarks>
        /// <param name="userUpdate"></param>
        /// <response code="200">Returns the updated user</response>
        /// <response code="400">If the DTO parameter has invalid values</response>
        /// <response code="401">If the user is not logged in or the token format is not valid</response>
        /// <response code="403">If the token is not valid</response>
        /// <response code="404">If user to update do not exists in the system</response>
        [HttpPut]
        [ServiceFilter(typeof(AuthorizationAttributeFilter))]
        [ProducesResponseType(typeof(UserDTO), 200)]
        public IActionResult Put([FromBody] UserDTO userUpdate)
        {
            try
            {
                var user = this.userService.Update(userUpdate);
                return Ok(user);
            }
            catch (ValidationException ve)
            {
                return NotFound(ve.Message);
            }
            catch (ArgumentException ae)
            {
                return BadRequest(ae.Message);
            }
            catch (System.Exception exeption)
            {
                return StatusCode(500, exeption.Message);
            }
        }
        [HttpPut("changepass")]
        public IActionResult ChangePassword([FromBody] UserDTO userUpdate)
        {
            try
            {
                var user = this.userService.ChangePassword(userUpdate);
                return Ok(user);
            }
            catch (ValidationException ve)
            {
                return NotFound(ve.Message);
            }
            catch (ArgumentException ae)
            {
                return BadRequest(ae.Message);
            }
            catch (System.Exception exeption)
            {
                return StatusCode(500, exeption.Message);
            }
        }

        [HttpPut("resetpass")]
        public IActionResult ResetPassword([FromBody] UserDTO userUpdate)
        {
            try
            {
                var user = this.userService.ResetPassword(userUpdate);
                return Ok(user);
            }
            catch (ValidationException ve)
            {
                return NotFound(ve.Message);
            }
            catch (ArgumentException ae)
            {
                return BadRequest(ae.Message);
            }
            catch (System.Exception exeption)
            {
                return StatusCode(500, exeption.Message);
            }
        }

        /// <summary>
        /// Deletes a specific user.
        /// </summary>
        /// <remarks>
        /// Needs Authorization Header, with Key={Authorization} And Value={Guid}
        /// </remarks>
        /// <param name="userId"></param>
        /// <response code="200">Returns the successful confirmation message if the user is deleted</response>
        /// <response code="401">If the user is not logged in or the token format is not valid</response>
        /// <response code="403">If the token is not valid</response>
        /// <response code="404">If the user to delete do not exists in the system</response>
        [HttpDelete("{userId}")]
        [ServiceFilter(typeof(AuthorizationAttributeFilter))]
        [ProducesResponseType(typeof(string), 200)]
        public IActionResult Delete(int userId)
        {
            try
            {
                this.userService.Remove(userId);
                return Ok("Se elimino el usuario.");
            }
            catch (ValidationException ve)
            {
                return NotFound(ve.Message);
            }
        }
    }
}
