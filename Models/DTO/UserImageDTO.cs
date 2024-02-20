namespace negare_kanban_api.Models;

// This model is only used to avoid overloading User queries
public class UserImageDTO
{
  public int Id { get; set; }
  public IFormFile? Image { get; set; }
  public User User { get; set; } = null!;
}