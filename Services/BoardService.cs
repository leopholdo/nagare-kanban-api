using Microsoft.EntityFrameworkCore;
using negare_kanban_api.Data;
using negare_kanban_api.Models;

namespace negare_kanban_api.Services.BoardService
{
  public interface IBoardService 
  {
    Task<Board?> GetBoard(long id);
    Task<List<Board>> GetBoards(string? name, bool? isClosed);
    Task<Board> CreateBoard(Board board);
    Task<Board> UpdateBoard(Board board);
    Task ReopenBoard(long id);
    Task CloseBoard(long id);
    Task DeleteBoard(long id);
    Task<List<Board>> GetBoardsFromLog(int userId);
    Task UpdateBoardUserLog(int boardId, int userId);
  }
  
  public class BoardService:IBoardService
  {
    private readonly DataContext _context;

    public BoardService(DataContext context)
    {
      _context = context;
    }

    public async Task<Board?> GetBoard(long id)
    {
      var board = await _context.Boards.FindAsync(id);

      return board;
    }

    public async Task<List<Board>> GetBoards(string? name, bool? isClosed)
    {
      var boards = await _context.Boards.Where(b => 
        (name == null || b.Name.Contains(name)) &&
        (isClosed == null || b.IsClosed == isClosed)
      ).AsNoTracking().ToListAsync();
    
      return boards;
    }

    public async Task<Board> CreateBoard(Board board)
    {
      board.CreationDate = DateTime.Now;

      _context.Boards.Add(board);
      await _context.SaveChangesAsync();

      return board;
    }

    public async Task<Board> UpdateBoard(Board board)
    {
      // Using BULK Methods 
      await _context.Boards.Where(b => b.Id == board.Id)
        .ExecuteUpdateAsync(b => b
          .SetProperty(b => b.Name, board.Name)
          .SetProperty(b => b.Color, board.Color)
          .SetProperty(b => b.BackgroundImage, board.BackgroundImage)
          .SetProperty(b => b.Favorite, board.Favorite)
        );

      return board;
    }

    public async Task CloseBoard(long id)
    {
      await _context.Boards.Where(b => b.Id == id)
        .ExecuteUpdateAsync(b => b
          .SetProperty(b => b.IsClosed, true)
          .SetProperty(b => b.ClosingDate, DateTime.Now)
        );
    }

    public async Task ReopenBoard(long id)
    {
      await _context.Boards.Where(b => b.Id == id)
        .ExecuteUpdateAsync(b => b
          .SetProperty(b => b.IsClosed, false)
          .SetProperty(b => b.ClosingDate, (DateTime?)null)
        );
    }    

    public async Task DeleteBoard(long id)
    {
      await _context.Boards.Where(b => b.Id == id).ExecuteDeleteAsync();
    }

    public async Task<List<Board>> GetBoardsFromLog(int userId)
    {
      var logs = await _context.BoardUserLogs
        .Include(b => b.Board)
        .Where(b => b.User.Id == userId)
        .OrderByDescending(b => b.Date)
        .ToListAsync();

      return logs.Select(l => l.Board).ToList();
    }

    public async Task UpdateBoardUserLog(int boardId, int userId)
    {
      // get the logs of the logged in user
      var logs = await _context.BoardUserLogs
        .Where(b => b.User.Id == userId)
        .OrderBy(b => b.Date) 
        .ToListAsync();

      if(logs != null)
      {
        // if there is a log with this id, update the date and return
        var actualLog = logs.FirstOrDefault(l => l.BoardId == boardId);
        if(actualLog != null) 
        {
          _context.BoardUserLogs.Where(b => b.Id == actualLog.Id)
            .ExecuteUpdate(b => b
              .SetProperty(b => b.Date, DateTime.Now)
            );

          return;
        }
        else
        {
          if(logs.Count >= 4)
          {
            // delete the oldest log
            await _context.BoardUserLogs
              .Where(b => b == logs.First())
              .ExecuteDeleteAsync();
          }
        }
      }

      _context.BoardUserLogs.Add(
        new BoardUserLog {
          Date = DateTime.Now,
          UserId = userId,
          BoardId = boardId
        }
      );

      _context.SaveChanges();
    }
  }
}