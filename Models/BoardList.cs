namespace negare_kanban_api.Models;

public class BoardList
{
  public int Id { get; set; }
  public string? Name { get; set; }
  public int Position { get; set; }
  
  public int BoardId { get; set; }
  public Board Board { get; set; } = null!;
  public virtual List<Card> Cards { get; set; } = [];
}