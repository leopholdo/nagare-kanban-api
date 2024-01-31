namespace negare_kanban_api.Models;

public class Board
{
  public long Id { get; set; }
  public string? Name { get; set; }
  public string? Color { get; set; }
  public string? Background { get; set; }
  public bool Favorite { get; set; }
  public DateTime CreationDate { get; set; }
}