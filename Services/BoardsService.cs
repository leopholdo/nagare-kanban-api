using Microsoft.EntityFrameworkCore;
using negare_kanban_api.Data;
using negare_kanban_api.Models;

namespace negare_kanban_api.Services.BoardsService
{
  public interface IBoardsService 
  {
    Task<List<Board>> GetBoards();
  }
  
  public class BoardsService:IBoardsService
  {
    private readonly DataContext _context;

    public BoardsService(DataContext context)
    {
        _context = context;
    }

     public async Task<List<Board>> GetBoards()
    {
        var boards = await _context.Boards.ToListAsync();
        return boards;
    }
  }
}