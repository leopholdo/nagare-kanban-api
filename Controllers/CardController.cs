using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using negare_kanban_api.Models;
using negare_kanban_api.Services.CardService;

namespace negare_kanban_api.Controllers
{
  [Authorize]
  [Route("[controller]")]
  [ApiController]
  public class CardController : ControllerBase
  {
    private readonly ICardService _cardService;

    public CardController(ICardService cardService)
    {
      _cardService = cardService;
    }

    [HttpGet("GetCard/{id}")]
    public async Task<ActionResult<IEnumerable<Card>>> GetCard(int id)
    {
      return await _cardService.GetCard(id);
    }

    [HttpGet("GetCards/{boardListId}")]
    public async Task<ActionResult<IEnumerable<Card>>> GetCards(int boardListId)
    {
      return await _cardService.GetCards(boardListId);
    }

    [HttpPost("CreateCard")]
    public async Task<ActionResult<Card>> CreateCard(CardDTO cardDTO)
    {
      try
      {
        var newCard = await _cardService.CreateCard(cardDTO);

        return Ok(newCard);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }

    [HttpPut("TransferCards")]
    public async Task<ActionResult> TransferCards([FromQuery]int origin, [FromQuery]int target, [FromQuery]bool top)
    {
      try
      {
        await _cardService.TransferCards(origin, target, top);

        return NoContent();
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }

    [HttpPut("UpdateCardPosition")]
    public async Task<ActionResult<Card>> UpdateCardPosition(CardDTO cardDTO)
    {
      try
      {
        var card = await _cardService.UpdateCardPosition(cardDTO);

        return Ok(card);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }

    [HttpPut("UpdateCard")]
    public async Task<ActionResult> UpdateCard(CardDTO cardDTO)
    {
      try
      {
        var card = await _cardService.UpdateCard(cardDTO);

        return Ok(card);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }

    [HttpPut("CloseCard/{id}")]
    public async Task<ActionResult> CloseCard(int id)
    {
      try
      {
        await _cardService.CloseCard(id);

        return NoContent();
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }

    [HttpPut("OpenCard/{id}")]
    public async Task<ActionResult> OpenCard(int id)
    {
      try
      {
        await _cardService.OpenCard(id);

        return NoContent();
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
    
    [HttpDelete("DeleteCard/{id}")]
    public async Task<ActionResult> DeleteCard(int id)
    {
      try
      {
        await _cardService.DeleteCard(id);

        return NoContent();
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
  }
}