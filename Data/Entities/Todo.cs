namespace Data.Entities;

public class Todo
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public bool IsDone { get; set; }
}