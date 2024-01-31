namespace negare_kanban_api.Models;

public class ChecklistItem
{
  public int Id { get; set; }
  public string? Content { get; set; }
  public DateTime DueDate { get; set; }
  public int Position { get; set; }
  public bool Completed { get; set; }
  public Checklist Checklist { get; set; } = null!;
}