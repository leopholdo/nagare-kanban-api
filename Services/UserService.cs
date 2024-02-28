using Microsoft.AspNetCore.Authorization;
using negare_kanban_api.Data;
using negare_kanban_api.Models;

namespace negare_kanban_api.Services.BoardsService
{
  public interface IUserService 
  {
    User? GetUser(int? id, string? email);
    List<UserDTO>? GetUsers(int? id, string? email);
    UserImage? GetUserImage(int userId);
    Task<User> Update(UserDTO userDTO);
    Task<User> UpdatePassword(UserDTO userDTO);
    Task<UserImage> UpdateUserImage(int userId, IFormFile image);
    Task Disable(int id);
    Task Activate(string email, string password);
  }
  
  public class UserService : IUserService
  {
    private readonly DataContext _context;

    public UserService(DataContext context)
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

    public List<UserDTO>? GetUsers(int? id, string? email)
    {
      var user = _context.Users
        .Select(user => new UserDTO {
          Id = user.Id,
          Email = user.Email,
          Name = user.Name ?? string.Empty
        })
        .Where(u => 
          (id == null || u.Id == id) &&
          (email == null || u.Email == email)
        )
        .ToList();

      return user;
    }

    public UserImage? GetUserImage(int userId)
    {
      var userImage = _context.UserImages.Where(
        us => us.User.Id == userId
      ).FirstOrDefault();

      if(userImage == null)
      {
        return null;
      }

      return userImage;
    }

    public async Task<User> Update(UserDTO userDTO)
    {
      var user = await _context.Users.FindAsync(userDTO.Id);

      if(user == null) 
      {
        throw new Exception("except_user_not_found");
      }

      if(userDTO.Email != user.Email) {
        // Checks if the changed email already exists in the database
        bool emailExists = _context.Users.Where(
          us => us.Email == userDTO.Email
        ).FirstOrDefault() != null;

        if(emailExists) {
          throw new Exception("except_email_already_registered");
        }
      }

      user.Email = userDTO.Email;
      user.Name = userDTO.Name;

      await _context.SaveChangesAsync();

      return user;
    }

    public async Task<User> UpdatePassword(UserDTO userDTO)
    {
      var user = await _context.Users.FindAsync(userDTO.Id);

      if(user == null) 
      {
        throw new Exception("except_user_not_found");
      }

      if(!BCrypt.Net.BCrypt.EnhancedVerify(userDTO.Password, user.HashedPassword))
      {
        throw new Exception("except_password_incorrect");
      }

      user.HashedPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(userDTO.NewPassword);

      _context.Users.Update(user);
      await _context.SaveChangesAsync();

      return user;
    }

    public async Task<UserImage> UpdateUserImage(int userId, IFormFile image)
    {
      await using var memStream = new MemoryStream();
      await image.CopyToAsync(memStream);

      await memStream.FlushAsync();    

      var user = await _context.Users.FindAsync(userId);

      if(user == null)
      {
        throw new Exception("except_user_not_found");
      }
      
      var userImage = _context.UserImages.Where(
        us => us.User == user
      ).FirstOrDefault();

      if(userImage == null) 
      {
        userImage = new UserImage {
          Image = memStream.ToArray(),
          ContentType = image.ContentType,
          FileName = image.FileName,
          User = user
        };

        await _context.UserImages.AddAsync(userImage);

      }
      else 
      {
        userImage.Image = memStream.ToArray();
        userImage.ContentType = image.ContentType;
        userImage.FileName = image.FileName;

        _context.UserImages.Update(userImage);
      }

      await _context.SaveChangesAsync();

      return userImage;
    }

    public async Task Disable(int id)
    {
      var user = await _context.Users.FindAsync(id);

      if(user == null) 
      {
        throw new Exception("except_user_not_found");
      }

      user.IsActive = false;

      await _context.SaveChangesAsync();

      return;
    }
  
    public async Task Activate(string email, string password)
    {
      var user = _context.Users.Where(
        us => us.Email == email
      ).FirstOrDefault();

      if(user == null || !BCrypt.Net.BCrypt.EnhancedVerify(password, user.HashedPassword)) 
      {
        throw new Exception("except_user_not_found");
      }

      user.IsActive = true;

      await _context.SaveChangesAsync();

      return;
    }
  }
}