using negare_kanban_api.Data;
using negare_kanban_api.Models;

namespace negare_kanban_api.Services.BoardsService
{
  public interface IAuthService 
  {
    User? GetUser(int? id, string? email);
    Task<User> Register(UserDTO userDTO);
  }
  
  public class AuthService : IAuthService
  {
    private readonly DataContext _context;

    public AuthService(DataContext context)
    {
        _context = context;
    }

    public User? GetUser(int? id, string? email)
    {
      var user = _context.Users
        .Where(u => 
          u.Id == id ||
          u.Email == email
        )
        .FirstOrDefault();

      return user;
    }

    public async Task<User> Register(UserDTO userDTO)
    {
      string hashedPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(userDTO.Password, 12);

      User user = new User
      {
        Name = userDTO.Name,
        Email = userDTO.Email,
        HashedPassword = hashedPassword
      };

      _context.Users.Add(user);
      await _context.SaveChangesAsync();

      return user;
    }
  }
}