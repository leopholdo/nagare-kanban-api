using Microsoft.EntityFrameworkCore;
using negare_kanban_api.Data;
using negare_kanban_api.Models;

namespace negare_kanban_api.Services.CardService 
{
  public interface ICardService 
  {
    Task<List<Card>> GetCard(int cardId);
    Task<List<Card>> GetCards(int boardListId);
    Task<CardDTO> CreateCard(CardDTO cardDTO);
    Task TransferCards(int originListId, int targetListId, bool top);
    Task<Card> UpdateCardPosition(CardDTO card);
    Task<Card?> UpdateCard(CardDTO cardDTO);
    Task CloseCard(int id);
    Task OpenCard(int id);
    Task DeleteCard(int id);
  }
  
  public class CardService:ICardService
  {
    private readonly DataContext _context;

    public CardService(DataContext context)
    {
      _context = context;
    }

    public async Task<List<Card>> GetCard(int cardId)
    {
      var card = await _context.Cards
        .Where(c => c.Id == cardId)
        .AsNoTracking()
        .ToListAsync();

      return card;
    }

    public async Task<List<Card>> GetCards(int boardListId)
    {
      var cards = await _context.Cards
        .Where(c => c.BoardListId == boardListId)
        .OrderBy(c => c.Position)
        .AsNoTracking()
        .ToListAsync();

      return cards;
    }
  
    public async Task<CardDTO> CreateCard(CardDTO cardDTO)
    {
      var boardList = _context.BoardLists
        .FirstOrDefault(bl => bl.Id == cardDTO.BoardListId);

      if(boardList == null) {
        throw new Exception("except_board_list_not_found");
      }

      var position = await _context.Cards.CountAsync(c => c.BoardListId == cardDTO.BoardListId);

      var newCard = new Card {
        Name = cardDTO.Name,
        BoardList = boardList,
        Position = position,
        BoardListId = boardList.Id
      };
      
      _context.Cards.Add(newCard);

      await _context.SaveChangesAsync();

      cardDTO.Id = newCard.Id;
      cardDTO.Position = position;

      return cardDTO;
    }

    public async Task TransferCards(int originListId, int targetListId, bool top)
    {
      var cards = await _context.Cards
        .Where(c => c.BoardListId == originListId)
        .ToListAsync();      

      if(top) 
      {
        await _context.Cards.Where(c => 
          c.BoardListId == targetListId          
        ).ExecuteUpdateAsync(c => c
          .SetProperty(x => x.Position, x => x.Position + cards.Count)
        );

        for(int i = 0; i < cards.Count; i++)
        {
          cards[i].Position = i;
          cards[i].BoardListId = targetListId;
        }
      }
      else
      {
        var maxPosition = await _context.Cards.CountAsync(c => c.BoardListId == targetListId);

        for(int i = 0; i < cards.Count; i++)
        {
          cards[i].Position = i + maxPosition;
          cards[i].BoardListId = targetListId;
        }
      }

      await _context.SaveChangesAsync();
    }

    public async Task<Card> UpdateCardPosition(CardDTO cardDTO)
    {
      var card = _context.Cards.Where(bl => bl.Id == cardDTO.Id).First();

      if(cardDTO.BoardListId != card.BoardListId)
      {
        // Updates the position of the elements in the original list 
        // that are after the element to be updated.
        await _context.Cards.Where(c => 
          c.Id != cardDTO.Id &&
          c.BoardListId == card.BoardListId && 
          c.Position >= card.Position
        ).ExecuteUpdateAsync(c => c
          .SetProperty(x => x.Position, x => x.Position -1)
        );

        // Updates the position of the elements in the new list that follow 
        // the position of the element to be inserted in the list.
        await _context.Cards.Where(c => 
          c.BoardListId == cardDTO.BoardListId && 
          c.Position >= cardDTO.Position
        ).ExecuteUpdateAsync(c => c
          .SetProperty(x => x.Position, x => x.Position +1)
        );
      }
      else {
        // Updates the position of elements that are between 
        // the old and new position of the current element
        if(cardDTO.Position > card.Position)
        {
          await _context.Cards.Where(c => 
            c.Id != cardDTO.Id &&
            c.BoardListId == cardDTO.BoardListId && 
            c.Position <= cardDTO.Position &&
            c.Position > card.Position
          ).ExecuteUpdateAsync(c => c
            .SetProperty(x => x.Position, x => x.Position -1)
          );
        }
        else if(cardDTO.Position < card.Position)
        {
          await _context.Cards.Where(c => 
            c.Id != cardDTO.Id &&
            c.BoardListId == cardDTO.BoardListId && 
            c.Position >= cardDTO.Position &&
            c.Position < card.Position
          ).ExecuteUpdateAsync(c => c
            .SetProperty(x => x.Position, x => x.Position +1)
          );
        }
      }

      // Update element's position and boardlist
      card.Position = cardDTO.Position;
      card.BoardListId = cardDTO.BoardListId;

      await _context.SaveChangesAsync();

      return card;
    }

    public async Task<Card?> UpdateCard(CardDTO cardDTO)
    {
      var card = await _context.Cards.FindAsync(cardDTO.Id);   

      if(card == null) return null;

      card.Name = cardDTO.Name ?? card.Name;
      card.Description = cardDTO.Description ?? card.Description;
      card.DueDate = cardDTO.DueDate ?? card.DueDate;
      card.DueComplete = cardDTO.DueComplete ?? card.DueComplete;

      await _context.SaveChangesAsync();

      return card;
    }

    public async Task CloseCard(int id)
    {
      var card = _context.Cards
        .Where(c => c.Id == id)
        .First();

      // Update Position of Cards that are ahead of actual Card
      await _context.Cards.Where(c =>
        c.BoardListId == card.BoardListId && 
        c.Position > card.Position
      ).ExecuteUpdateAsync(c => c
        .SetProperty(x => x.Position, x => x.Position -1)
      );

      card.isClosed = true;

      await _context.SaveChangesAsync();
    }

    public async Task OpenCard(int id)
    {
      var card = _context.Cards
        .Where(c => c.Id == id)
        .First();

      // Update Position of Cards that are ahead of actual Card
      await _context.Cards.Where(c =>
        c.Id != id &&
        c.BoardListId == card.BoardListId 
      ).ExecuteUpdateAsync(c => c
        .SetProperty(x => x.Position, x => x.Position +1)
      );

      card.Position = 0;
      card.isClosed = false;

      await _context.SaveChangesAsync();
    }

    public async Task DeleteCard(int id)
    {
      await _context.Cards.Where(c => c.Id == id).ExecuteDeleteAsync();
    }
  }
}