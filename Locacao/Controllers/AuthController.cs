using AutoMapper;
using Locacao.Domain.Enum;
using Locacao.Domain.Model;
using Locacao.Dto;
using Locacao.Services;
using Locacao.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Locacao.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public AuthController(JwtService jwtService, IUserService userService, IMapper mapper)
        {
            _jwtService = jwtService;
            _userService = userService;
            _mapper = mapper;
        }

        /// <summary>
        /// Authenticates a user and returns a JWT token.
        /// </summary>
        /// <param name="login">The login credentials.</param>
        /// <returns>A JWT token if authentication is successful.</returns>
        /// <response code="200">Returns the JWT token.</response>
        /// <response code="401">If the credentials are invalid.</response>
        /// <response code="500">If there was an internal server error.</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            try
            {
                var user = await _userService.ValidateAndGetUserAsync(login.Username, login.Password);

                if (user == null)
                {
                    return Unauthorized(new ErrorResponse { Message = "Invalid username or password" });
                }
                                
                string role = user.Profile.ToString();

                var token = _jwtService.GenerateToken(user.Id.ToString(), role);

                return Ok(new AuthResponse
                {
                    Token = token,
                    UserId = user.Id,
                    Username = user.Username,
                    Role = role
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse { Message = $"An error occurred while processing your request: {ex.Message}" });
            }
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="registerModel">The registration information.</param>
        /// <returns>The newly created user information.</returns>
        /// <response code="201">Returns the newly created user.</response>
        /// <response code="400">If the user already exists or the data is invalid.</response>
        /// <response code="500">If there was an internal server error.</response>
        [HttpPost("register")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] LoginDto registerModel)
        {
            try
            {
                UserProfile userProfile;
                if (!Enum.IsDefined(typeof(UserProfile), registerModel.Profile))
                {
                    return BadRequest(new ErrorResponse { Message = "Invalid user profile" });
                }
                userProfile = (UserProfile)registerModel.Profile;

                var newUser = new User
                {
                    Username = registerModel.Username,
                    Password = registerModel.Password,
                    Profile = userProfile
                };

                var createdUser = await _userService.CreateUserAsync(newUser);
                var userDto = _mapper.Map<UserDto>(createdUser);

                return CreatedAtAction(nameof(Login), new { username = userDto.Username }, userDto);
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
    }
}