namespace negare_kanban_api.Models;

public class Checklist
{
  public int Id { get; set; }
  public string? Name { get; set; }
  public DateTime DueDate { get; set; }
  public int Position { get; set; }

  public Card Card { get; set; } = null!;
  public List<User> Users { get; set; } = [];
}