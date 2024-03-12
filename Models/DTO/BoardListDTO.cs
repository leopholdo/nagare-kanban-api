namespace negare_kanban_api.Models;

public class BoardListDTO
{
  public int Id { get; set; }
  public string? Name { get; set; }
  public int? Position { get; set; }
  public int BoardId { get; set; }
}