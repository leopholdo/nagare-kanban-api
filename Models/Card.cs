namespace negare_kanban_api.Models;

public class Card
{
  public long Id { get; set; }
  public string? Name { get; set; }
  public DateTime DueDate { get; set; }
  public DateTime DueComplete { get; set; }
  public bool Closed { get; set; }
  public int Position { get; set; }
  
  public List<Tag> Tags { get; set; } = [];
  // public int? TagId { get; set; }
  // public ICollection<Tag?> Tags { get; } = new List<Tag?>();
  // public ICollection<CardTag>? CardTag { get; } = [];
}