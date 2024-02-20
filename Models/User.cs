
namespace negare_kanban_api.Models;

public class User
{
  public int Id { get; set; }
  public string? Name { get; set; }
  public required string Email { get; set; }
  public required string HashedPassword { get; set; }
  public bool IsActive { get; set; } = true;
    // public List<string>? Roles { get; set; }
}