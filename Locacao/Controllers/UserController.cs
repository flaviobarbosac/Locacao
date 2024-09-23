using Microsoft.AspNetCore.Mvc;
using Locacao.Dto;
using AutoMapper;
using Locacao.Domain.Model;
using Swashbuckle.AspNetCore.Annotations;
using Locacao.Services.Interface;

namespace Locacao.Controllers
{
    /// <summary>
    /// Controller for managing users.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "v1")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new User.
        /// </summary>
        /// <param name="data">The User data.</param>
        /// <returns>The newly created User.</returns>
        /// <response code="201">Returns the newly created User.</response>
        /// <response code="400">If the data is invalid.</response>
        [HttpPost]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid data", typeof(ErrorResponse))]
        public async Task<ActionResult<UserDto>> CreateUser([FromBody] UserDto data)
        {
            try
            {
                var user = _mapper.Map<User>(data);
                var createdUser = await _userService.CreateUserAsync(user);
                var userCreatedDto = _mapper.Map<UserDto>(createdUser);
                return CreatedAtAction(nameof(Get), new { id = userCreatedDto.Id }, userCreatedDto);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ErrorResponse { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse { Message = $"An error occurred while processing your request: {ex.Message}" });
            }
        }

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        /// <returns>A list of all users.</returns>
        /// <response code="200">Returns the list of users.</response>
        /// <response code="500">If there was an internal server error.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<UserDto>>> Get()
        {
            try
            {
                var result = await _userService.GetAllAsync();
                var userDtos = _mapper.Map<IEnumerable<UserDto>>(result);
                return Ok(userDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse { Message = $"An error occurred while processing your request: {ex.Message}" });
            }
        }

        /// <summary>
        /// Retrieves a specific User by id.
        /// </summary>
        /// <param name="id">The id of the User to retrieve.</param>
        /// <returns>The requested User.</returns>
        /// <response code="200">Returns the requested User.</response>
        /// <response code="404">If the User is not found.</response>
        /// <response code="500">If there was an internal server error.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserDto>> Get(Guid id)
        {
            try
            {
                var user = await _userService.GetByIdAsync(id);
                if (user == null)
                {
                    return NotFound(new ErrorResponse { Message = "User not found" });
                }
                var userDto = _mapper.Map<UserDto>(user);
                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse { Message = $"An error occurred while processing your request: {ex.Message}" });
            }
        }

        /// <summary>
        /// Updates an existing User.
        /// </summary>
        /// <param name="id">The id of the User to update.</param>
        /// <param name="data">The updated User data.</param>
        /// <returns>The updated User.</returns>
        /// <response code="200">Returns the updated User.</response>
        /// <response code="400">If the data is invalid.</response>
        /// <response code="404">If the User is not found.</response>
        /// <response code="500">If there was an internal server error.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserDto>> UpdateUser(Guid id, [FromBody] UserDto data)
        {
            try
            {
                var user = _mapper.Map<User>(data);
                user.Id = id;
                var updatedUser = await _userService.UpdateUserAsync(user);
                var updatedUserDto = _mapper.Map<UserDto>(updatedUser);
                return Ok(updatedUserDto);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new ErrorResponse { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse { Message = $"An error occurred while processing your request: {ex.Message}" });
            }
        }

        /// <summary>
        /// Deletes a specific User.
        /// </summary>
        /// <param name="id">The id of the User to delete.</param>
        /// <returns>No content.</returns>
        /// <response code="204">If the User was successfully deleted.</response>
        /// <response code="404">If the User is not found.</response>
        /// <response code="500">If there was an internal server error.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var user = await _userService.GetByIdAsync(id);
                if (user == null)
                {
                    return NotFound(new ErrorResponse { Message = "User not found" });
                }

                await _userService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse { Message = $"An error occurred while processing your request: {ex.Message}" });
            }
        }

        /// <summary>
        /// Retrieves a specific User by username.
        /// </summary>
        /// <param name="username">The username of the User to retrieve.</param>
        /// <returns>The requested User.</returns>
        /// <response code="200">Returns the requested User.</response>
        /// <response code="404">If the User is not found.</response>
        /// <response code="500">If there was an internal server error.</response>
        [HttpGet("by-username/{username}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserDto>> GetByUsername(string username)
        {
            try
            {
                var user = await _userService.GetUserByUserNameAsync(username);
                if (user == null)
                {
                    return NotFound(new ErrorResponse { Message = "User not found" });
                }
                var userDto = _mapper.Map<UserDto>(user);
                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse { Message = $"An error occurred while processing your request: {ex.Message}" });
            }
        }

        /// <summary>
        /// Validates user credentials and returns the user if valid.
        /// </summary>
        /// <param name="loginDto">The login credentials.</param>
        /// <returns>The authenticated user.</returns>
        /// <response code="200">Returns the authenticated user.</response>
        /// <response code="401">If the credentials are invalid.</response>
        /// <response code="500">If there was an internal server error.</response>
        [HttpPost("validate")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserDto>> ValidateAndGetUser([FromBody] LoginDto loginDto)
        {
            try
            {
                var user = await _userService.ValidateAndGetUserAsync(loginDto.Username, loginDto.Password);
                if (user == null)
                {
                    return Unauthorized(new ErrorResponse { Message = "Invalid username or password" });
                }
                var userDto = _mapper.Map<UserDto>(user);
                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse { Message = $"An error occurred while processing your request: {ex.Message}" });
            }
        }
    }
}