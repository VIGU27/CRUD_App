using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CRUD_App.Models;

namespace CRUD_App.RequestModels;

public class ArticleCreateRequest
{
    [Required]
    public Status Status { get; set; }
}

public class ArticleUpdateRequest
{
    [Required]
    public Status Status { get; set; }
}

public class ArticleFilterRequest
{
    [DefaultValue(0)]
    public int Page { get; set; } = 0;

    [DefaultValue(20)]
    public int PageSize { get; set; } = 20;

    [DefaultValue(ArticleSortBy.CreatedAt)]
    public ArticleSortBy SortBy { get; set; } = ArticleSortBy.CreatedAt;

    [DefaultValue(SortDirection.Desc)]
    public SortDirection SortDir { get; set; } = SortDirection.Desc;

    [DefaultValue(StatusFilter.All)]
    public StatusFilter Status { get; set; } = StatusFilter.All;
}
