using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using negare_kanban_api.Models;
using negare_kanban_api.Services.BoardsService;
using negare_kanban_api.Services.TokenService;

namespace negare_kanban_api.Controllers
{
  [Authorize]
  [Route("[controller]")]
  [ApiController]
  public class AuthController : ControllerBase
  {
    private readonly IAuthService _authService;
    private readonly ITokenService _tokenService;

    public AuthController(IAuthService authService, ITokenService tokenService)
    {
        _authService = authService;
        _tokenService = tokenService;
    }
    
    [AllowAnonymous]
    [HttpPost("Register")]
    public async Task<ActionResult<User>> Register(UserDTO userDTO)
    {
      try
      {
        if(userDTO.Email == null || userDTO.Password == null || userDTO.Name == null)
        {
          return BadRequest();
        }

        if(_authService.GetUser(null, userDTO.Email) != null)
        {
          throw new Exception("except_user_already_exists");
        }

        var user = await _authService.Register(userDTO);

        return Ok();
      }
      catch (Exception e)
      {
        if(e.Message != null)
        {
          return BadRequest(e.Message);
        }

        return BadRequest();
      }
    }

    [AllowAnonymous]
    [HttpPost("Login")]
    public ActionResult Login(UserDTO userDTO)
    {
      try
      {
        var user = _authService.GetUser(null, userDTO.Email);

        if(user == null || !BCrypt.Net.BCrypt.EnhancedVerify(userDTO.Password, user.HashedPassword))
        {
          throw new Exception("except_user_not_found");
        }

        var token = _tokenService.Generate(user);

        return Ok(new {
          Name = user.Name,
          Email = user.Email,
          Token = token
        });
      }
      catch (Exception e)
      {
        if(e.Message != null)
        {
          return BadRequest(e.Message);
        }

        return BadRequest();
      }
    }

    [Authorize]
    [HttpPut("Update")]
    public async Task<ActionResult> Update(UserDTO userDTO)
    {
      try
      { 
        await _authService.Update(userDTO);

        return Ok();
      }
      catch (Exception e)
      {
        if(e.Message != null)
        {
          return BadRequest(e.Message);
        }

        return BadRequest();
      }
    }

    [Authorize]
    [HttpPut("UpdatePassword")]
    public async Task<ActionResult> UpdatePassword(UserDTO userDTO)
    {
      try
      { 
        await _authService.UpdatePassword(userDTO);

        return Ok();
      }
      catch (Exception e)
      {
        if(e.Message != null)
        {
          return BadRequest(e.Message);
        }

        return BadRequest();
      }
    }
  }
}