using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using negare_kanban_api.Models;
using negare_kanban_api.Services.BoardService;

namespace negare_kanban_api.Controllers
{
  [Authorize]
  [Route("[controller]")]
  [ApiController]
  public class BoardController : ControllerBase
  {
    private readonly IBoardService _boardService;

    public BoardController(IBoardService boardService)
    {
      _boardService = boardService;
    }

    [HttpGet("GetBoards")]
    public async Task<ActionResult<IEnumerable<Board>>> GetBoards(string? name, bool? isClosed)
    {
      return await _boardService.GetBoards(name, isClosed);
    }

    [HttpGet("GetBoard/{id}")]
    public async Task<ActionResult<Board>> GetBoard(int id)
    {
      var board = await _boardService.GetBoard(id);

      if (board == null)
      {
        return NotFound();
      }

      return board;
    }

    [HttpPost("CreateBoard")]
    public async Task<ActionResult<Board>> CreateBoard(Board boardDTO)
    {
      var board = await _boardService.CreateBoard(boardDTO);

      return board;
    }

    [HttpPut("UpdateBoard")]
    public async Task<ActionResult<Board>> UpdateBoard(Board boardDTO)
    {
      var board = await _boardService.UpdateBoard(boardDTO);

      return board;
    }

    [HttpPut("ReopenBoard/{id}")]
    public async Task<IActionResult> ReopenBoard(int id)
    {
      try
      {
        await _boardService.ReopenBoard(id);

        return NoContent();

      }
      catch (Exception e)
      {
        if (e.Message == "exception_board_not_found")
        {
          return NotFound();
        }
        else
        {
          return BadRequest();
        }
      }
    }

    [HttpPut("CloseBoard/{id}")]
    public async Task<IActionResult> CloseBoard(int id)
    {
      try
      {
        await _boardService.CloseBoard(id);

        return NoContent();

      }
      catch (Exception e)
      {
        if (e.Message == "exception_board_not_found")
        {
          return NotFound();
        }
        else
        {
          return BadRequest();
        }
      }
    }    

    [HttpDelete("DeleteBoard/{id}")]
    public async Task<IActionResult> DeleteBoard(int id)
    {
      try
      {
        await _boardService.DeleteBoard(id);

        return NoContent();

      }
      catch (Exception e)
      {
        if (e.Message == "exception_board_not_found")
        {
          return NotFound();
        }
        else
        {
          return BadRequest();
        }
      }
    }

    [HttpGet("GetUserLog")]
    public async Task<IActionResult> GetUserLog()
    {
      var userId = User.FindFirst(ClaimTypes.NameIdentifier);

      if(userId == null) 
      {
        return Unauthorized();
      }

      var logs = await _boardService.GetBoardsFromLog(int.Parse(userId.Value));

      return Ok(logs);
    }

    [HttpPut("UpdateLog/{boardId}")]
    public async Task<IActionResult> UpdateLog(int boardId)
    {
      var userId = User.FindFirst(ClaimTypes.NameIdentifier);

      if(userId == null) 
      {
        return Unauthorized();
      }

      await _boardService.UpdateBoardUserLog(boardId, int.Parse(userId.Value));

      return NoContent();
    }
  }
}
