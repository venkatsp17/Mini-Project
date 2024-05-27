using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models.DTO_s;
using ShoppingAppAPI.Services.Interfaces;

namespace ShoppingAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userService;
        public UserController(IUserServices userService)
        {
            _userService = userService;
        }

        [HttpPost("CustomerLogin")]
        [ProducesResponseType(typeof(LoginReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<LoginReturnDTO>> CustomerLogin(LoginDTO userLoginDTO)
        {
            try
            {
                var result = await _userService.CustomerLogin(userLoginDTO);
                return Ok(result);
            }
            catch (UnauthorizedUserException ex)
            {
                return Unauthorized(new ErrorModel(401, ex.Message));
            }
            catch (UnableToLoginException ex)
            {
                return Unauthorized(new ErrorModel(401, ex.Message));
            }
            catch(AccountInActiveException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorModel(500, $"An unexpected error occurred. {ex.Message}"));
            }
        }

        [HttpPost("CustomerRegister")]
        [ProducesResponseType(typeof(LoginReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<LoginReturnDTO>> CustomerRegister(RegisterDTO registerDTO)
        {
            try
            {
                var result = await _userService.CustomerRegister(registerDTO);
                return Ok(result);
            }
            catch (UserAlreadyExistsException ex)
            {
                return Conflict(new ErrorModel(409, ex.Message));
            }
            catch (UnableToRegisterException ex)
            {
                return UnprocessableEntity(new ErrorModel(422, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorModel(500, $"An unexpected error occurred. {ex.Message}"));
            }
        }

        [HttpPost("SellerLogin")]
        [ProducesResponseType(typeof(LoginReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<LoginReturnDTO>> SellerLogin(LoginDTO userLoginDTO)
        {
            try
            {
                var result = await _userService.SellerLogin(userLoginDTO);
                return Ok(result);
            }
            catch (UnauthorizedUserException ex)
            {
                return Unauthorized(new ErrorModel(401, ex.Message));
            }
            catch (UnableToLoginException ex)
            {
                return Unauthorized(new ErrorModel(401, ex.Message));
            }
            catch (AccountInActiveException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorModel(500, $"An unexpected error occurred. {ex.Message}"));
            }
        }

        [HttpPost("SellerRegister")]
        [ProducesResponseType(typeof(LoginReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<LoginReturnDTO>> SellerRegister(RegisterDTO registerDTO)
        {
            try
            {
                var result = await _userService.SellerRegister(registerDTO);
                return Ok(result);
            }
            catch (UserAlreadyExistsException ex)
            {
                return Conflict(new ErrorModel(409, ex.Message));
            }
            catch (UnableToRegisterException ex)
            {
                return UnprocessableEntity(new ErrorModel(422, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorModel(500, $"An unexpected error occurred. {ex.Message}"));
            }
        }
    }
}
