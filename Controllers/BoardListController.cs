using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using negare_kanban_api.Models;
using negare_kanban_api.Services.BoardListService;

namespace negare_kanban_api.Controllers
{
  [Authorize]
  [Route("[controller]")]
  [ApiController]
  public class BoardListController : ControllerBase
  {
    private readonly IBoardListService _boardListService;

    public BoardListController(IBoardListService boardListService)
    {
      _boardListService = boardListService;
    }

    [HttpGet("GetBoardLists/{boardId}")]
    public async Task<ActionResult<IEnumerable<BoardList>>> GetBoardLists(int boardId)
    {
      return await _boardListService.GetBoardLists(boardId);
    }

    [HttpPost("CreateBoardList")]
    public async Task<ActionResult<BoardListDTO>> CreateBoardList(BoardListDTO boardListDTO)
    {
      try
      {
        var boardList = await _boardListService.CreateBoardList(boardListDTO);

        return Ok(boardList);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }

    [HttpPut("UpdateBoardList")]
    public async Task<ActionResult<BoardListDTO>> UpdateBoardList(BoardListDTO boardListDTO)
    {
      var boardList = await _boardListService.UpdateBoardList(boardListDTO);

      return boardList;
    }

    [HttpDelete("DeleteBoardList/{id}")]
    public async Task<ActionResult<BoardList>> DeleteBoardList(int id)
    {
      await _boardListService.DeleteBoardList(id);

      return NoContent();
    }
  }
}