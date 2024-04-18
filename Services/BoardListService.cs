using Microsoft.EntityFrameworkCore;
using negare_kanban_api.Data;
using negare_kanban_api.Models;

namespace negare_kanban_api.Services.BoardListService
{
  public interface IBoardListService 
  {
    Task<List<BoardList>> GetBoardLists(int boardId);
    Task<BoardListDTO> CreateBoardList(BoardListDTO boardList);
    Task<BoardListDTO> UpdateBoardList(BoardListDTO boardList);
    Task DeleteBoardList(int id);
  }
  
  public class BoardListService:IBoardListService
  {
    private readonly DataContext _context;

    public BoardListService(DataContext context)
    {
      _context = context;
    }

    public async Task<List<BoardList>> GetBoardLists(int boardId)
    {      
      var boardLists = await _context.BoardLists
        .Where(bl => bl.Board.Id == boardId)
        .Include(bl => bl.Cards.OrderBy(c => c.Position))
        .OrderBy(bl => bl.Position)
        .AsNoTracking()
        .ToListAsync();

      return boardLists;
    }
  
    public async Task<BoardListDTO> CreateBoardList(BoardListDTO boardListDTO)
    {
      var board = _context.Boards.FirstOrDefault(b => b.Id == boardListDTO.BoardId);

      if(board == null) {
        throw new Exception("except_board_not_found");
      }

      var boardList = new BoardList {
        Name = boardListDTO.Name,
        Position = boardListDTO.Position ?? 0,
        Board = board
      };
      
      _context.BoardLists.Add(boardList);

      await _context.SaveChangesAsync();

      boardListDTO.Id = boardList.Id;

      return boardListDTO;
    }

    public async Task<BoardListDTO> UpdateBoardList(BoardListDTO boardListDTO)
    {
      var boardList = _context.BoardLists.Where(bl => bl.Id == boardListDTO.Id).First();

      // Updates the position of elements that are between 
      // the old and new position of the current element
      if(boardListDTO.Position > boardList.Position)
      {
        await _context.BoardLists.Where(bl => 
          bl.Id != boardListDTO.Id &&
          bl.BoardId == boardListDTO.BoardId && 
          bl.Position <= boardListDTO.Position &&
          bl.Position > boardList.Position
        ).ExecuteUpdateAsync(bl => bl
          .SetProperty(x => x.Position, x => x.Position -1)
        );
      }
      else if(boardListDTO.Position < boardList.Position)
      {
        await _context.BoardLists.Where(bl => 
          bl.Id != boardListDTO.Id &&
          bl.BoardId == boardListDTO.BoardId && 
          bl.Position >= boardListDTO.Position &&
          bl.Position < boardList.Position
        ).ExecuteUpdateAsync(bl => bl
          .SetProperty(x => x.Position, x => x.Position +1)
        );
      }

      // Update element
      boardList.Name = boardListDTO.Name ?? boardList.Name;
      boardList.Position = boardListDTO.Position ?? boardList.Position;

      await _context.SaveChangesAsync();

      return boardListDTO;
    }

    public async Task DeleteBoardList(int id)
    {
      var boardList = _context.BoardLists.Where(bl => bl.Id == id)
        .AsNoTracking()
        .First();

      // Update Position of BoardLists that are ahead of actual BoardList
      await _context.BoardLists.Where(bl =>
        bl.BoardId == boardList.BoardId && 
        bl.Position > boardList.Position
      ).ExecuteUpdateAsync(bl => bl
        .SetProperty(x => x.Position, x => x.Position -1)
      );

      await _context.BoardLists.Where(bl => bl.Id == id).ExecuteDeleteAsync();
    }
  }
}