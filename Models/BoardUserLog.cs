namespace negare_kanban_api.Models;

public class BoardUserLog
{
  public int Id { get; set; }
  public DateTime? Date { get; set; }
  public int BoardId { get; set; }
  public Board Board { get; set; } = null!;
  public int UserId { get; set; }
  public User User { get; set; } = null!;
}