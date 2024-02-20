using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using negare_kanban_api.Models;
using negare_kanban_api.Services.BoardsService;

namespace negare_kanban_api.Controllers
{
  [Authorize]
  [Route("[controller]")]
  [ApiController]
  public class UserController : ControllerBase
  {
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
      _userService = userService;
    }

    [HttpGet("GetUsers")]
    public ActionResult<List<UserDTO>?> GetUsers(int? id, string? email)
    {
      var users = _userService.GetUsers(id, email);

      return users;
    }

    [HttpGet("GetLoggedUserImage")]
    public IActionResult? GetLoggedUserImage()
    {
      var userId = User.FindFirst(ClaimTypes.NameIdentifier);

      if(userId == null) 
      {
        return Unauthorized();
      }

      var userImage = _userService.GetUserImage(int.Parse(userId.Value));

      if(userImage == null)
      {
        return NotFound();
      }
      else
      {
        if(userImage.Image == null || userImage.ContentType == null || userImage.FileName == null) 
        {
          return NotFound();
        }

        string base64String = Convert.ToBase64String(userImage.Image);
        
        return Ok(new {
          image = base64String,
          contentType = userImage.ContentType
        });
        // return File(userImage.Image, userImage.ContentType, userImage.FileName);
      }
    }

    [HttpPut("Update")]
    public async Task<ActionResult> Update(UserDTO userDTO)
    {
      try
      { 
        if(userDTO.Id == null) 
        {
          var userId = User.FindFirst(ClaimTypes.NameIdentifier);

          if(userId == null) 
          {
            return Unauthorized();
          }

          userDTO.Id = int.Parse(userId.Value);
        }

        var user = await _userService.Update(userDTO);

        return Ok(new {
          Email = user.Email,
          Name = user.Name
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

    [HttpPut("UpdatePassword")]
    public async Task<ActionResult> UpdatePassword(UserDTO userDTO)
    {
      try
      {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier);

        if(userId == null) 
        {
          return Unauthorized();
        }

        userDTO.Id = int.Parse(userId.Value);

        await _userService.UpdatePassword(userDTO);

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

    [HttpPut("UpdateUserImage/{userId?}")]
    public async Task<ActionResult> UpdateUserImage([FromForm] IFormFile Image, int userId = 0)
    {
      try
      {
        if(userId == 0) 
        {
          var id = User.FindFirst(ClaimTypes.NameIdentifier);

          if(id == null) 
          {
            return Unauthorized();
          }

          userId = int.Parse(id.Value);
        }

        var userImage = await _userService.UpdateUserImage(userId, Image);

        if(userImage.Image == null) 
        {
          return BadRequest();
        }

        string base64String = Convert.ToBase64String(userImage.Image);

        return Ok(new {
          image = base64String,
          contentType = userImage.ContentType
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

    [HttpPut("Disable")]
    public async Task<ActionResult> Disable()
    {
      try
      { 
        var userId = User.FindFirst(ClaimTypes.NameIdentifier);

        if(userId == null) 
        {
          return Unauthorized();
        }
        
        await _userService.Disable(int.Parse(userId.Value));

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
    [HttpPut("Activate")]
    public async Task<ActionResult> Activate(UserDTO userDTO)
    {
      try
      {                 
        await _userService.Activate(userDTO.Email, userDTO.Password);

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
