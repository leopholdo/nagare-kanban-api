namespace negare_kanban_api.Models;

public class UserDTO
{
  public int? Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public string Email { get; set; } = string.Empty;
  public string Password { get; set; } = string.Empty;
  public string NewPassword { get; set; } = string.Empty;
  public bool IsActive { get; set; } = true;
}