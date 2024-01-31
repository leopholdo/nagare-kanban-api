namespace negare_kanban_api.Models;

public class Action
{
  public int Id { get; set; }
  public string? Content { get; set; }
  public string? Type { get; set; }
  // TODO - Verificar
  public string? Data { get; set; }
  public bool Edited { get; set; }
  public DateTime CreationDate { get; set; }
  public DateTime? EditDate { get; set; }
  public User? User { get; set; }
  public Card Card { get; set; } = null!;
}