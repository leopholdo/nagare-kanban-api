namespace negare_kanban_api.Models;

public class Board
{
  public int Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public string Color { get; set; } = string.Empty;
  public string? BackgroundImage { get; set; }
  public bool Favorite { get; set; }
  public DateTime? CreationDate { get; set; }
  public DateTime? ClosingDate { get; set; }
  public bool IsClosed { get; set; } = false;
}