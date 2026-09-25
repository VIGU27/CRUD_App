using CRUD_App.Models;

namespace CRUD_App.ResponseModels;

public class ContentResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public int AuthorId { get; set; }
    public string AuthorUsername { get; set; } = string.Empty;
    public Status Status { get; set; }
    public Language Language { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? ArticleId { get; set; }
}
