namespace negare_kanban_api.Models;

public class BoardList
{
  public int Id { get; set; }
  public string? Name { get; set; }
  public int Position { get; set; }
  
  public Board Board { get; set; } = null!;
}