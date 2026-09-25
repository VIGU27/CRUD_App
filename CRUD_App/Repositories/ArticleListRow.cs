using CRUD_App.Models;

namespace CRUD_App.Repositories;

public class ArticleListRow
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Author { get; set; }
    public Status Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
