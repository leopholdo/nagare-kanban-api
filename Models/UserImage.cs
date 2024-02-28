namespace negare_kanban_api.Models;

// This model is only used to avoid overloading User queries
public class UserImage
{
  public int Id { get; set; }
  public byte[]? Image { get; set; }
  public string? ContentType  { get; set; }
  public string? FileName { get; set; }
  public User User { get; set; } = null!;
}