namespace negare_kanban_api.Models;

public class Card
{
  public int Id { get; set; }
  public string? Name { get; set; }
  public string? Description { get; set; }
  public DateTime? DueDate { get; set; }
  public DateTime? DueComplete { get; set; }
  public bool isClosed { get; set; } = false;
  public int Position { get; set; }
  public List<Tag> Tags { get; set; } = [];
  public int BoardListId { get; set; }
  public BoardList BoardList { get; set; } = null!;
}