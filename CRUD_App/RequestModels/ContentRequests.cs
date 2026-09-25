using System.ComponentModel.DataAnnotations;
using CRUD_App.Models;

namespace CRUD_App.RequestModels;

public class ContentCreateRequest
{
    [Required, MaxLength(255)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Body { get; set; } = string.Empty;

    [Required]
    public int AuthorId { get; set; }

    [Required]
    public Status Status { get; set; }

    [Required]
    public Language Language { get; set; }

    public int? ArticleId { get; set; }
}

public class ContentUpdateRequest
{
    [Required, MaxLength(255)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Body { get; set; } = string.Empty;

    [Required]
    public int AuthorId { get; set; }

    [Required]
    public Status Status { get; set; }

    [Required]
    public Language Language { get; set; }

    public int? ArticleId { get; set; }
}
