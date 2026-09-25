using CRUD_App.Models;

namespace CRUD_App.ResponseModels;

public class ArticleResponse
{
    public int Id { get; set; }
    public Status Status { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class ArticleListItemResponse
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Author { get; set; }
    public Status Status { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class ArticleDetailsResponse
{
    public int Id { get; set; }
    public Status Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<ContentResponse> Contents { get; set; } = new();
}

public class PagedResponse<T>
{
    public List<T> Result { get; set; } = new();
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages => PageSize == 0 ? 0 : (int)Math.Ceiling(TotalCount / (double)PageSize);
}


