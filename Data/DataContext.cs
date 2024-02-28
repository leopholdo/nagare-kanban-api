using Microsoft.EntityFrameworkCore;
using negare_kanban_api.Models;

namespace negare_kanban_api.Data;

public class DataContext : DbContext
{
  public DataContext(DbContextOptions<DataContext> options): base(options)
  {
  }

  public DbSet<User> Users { get; set; } = null!;
  public DbSet<UserImage> UserImages { get; set; } = null!;
  public DbSet<Board> Boards { get; set; } = null!;
  public DbSet<BoardUserLog> BoardUserLogs { get; set; } = null!;
  public DbSet<BoardList> BoardLists { get; set; } = null!;
  public DbSet<Tag> Tags { get; set; } = null!;
  public DbSet<Card> Cards { get; set; } = null!;
  public DbSet<Models.Action> Actions { get; set; } = null!;
  public DbSet<Checklist> Checklists { get; set; } = null!;
  public DbSet<ChecklistItem> ChecklistItems { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<UserImage>()
        .Property(e => e.Image)
        .HasColumnType("bytea");
      
      modelBuilder.Entity<Card>()
        .HasMany(e => e.Tags)
        .WithMany();

      modelBuilder.Entity<Checklist>()
        .HasMany(e => e.Users)
        .WithMany();
    }
}