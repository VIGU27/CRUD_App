namespace CRUD_App.Models;

public class Content
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;

    public int AuthorId { get; set; }
    public User Author { get; set; } = null!;

    public Status Status { get; set; }
    public Language Language { get; set; }
    public DateTime CreatedAt { get; set; }

    public int? ArticleId { get; set; }
    public Article? Article { get; set; }
}
