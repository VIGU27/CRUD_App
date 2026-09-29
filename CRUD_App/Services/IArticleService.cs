using CRUD_App.Models;
using CRUD_App.ResponseModels;

namespace CRUD_App.Services;

public interface IArticleService
{
    Task<ArticleResponse> CreateArticle(Status status, CancellationToken ct = default);

    Task<List<ArticleResponse>> GetAllArticle(CancellationToken ct = default);

    Task<ArticleResponse> GetArticleById(int id, CancellationToken ct = default);

    /// <summary>Article details together with the full list of its content items.</summary>
    Task<ArticleDetailsResponse> GetArticleDetails(int id, CancellationToken ct = default);

    /// <summary>
    /// Paginated article list. Sorting by title prioritizes the article's English content title;
    /// default sort is CreatedAt. Optionally filtered by article status.
    /// </summary>
    Task<PagedResponse<ArticleListItemResponse>> FilterArticle(
        int page,
        int pageSize,
        ArticleSortBy sortBy,
        SortDirection sortDir,
        StatusFilter status,
        CancellationToken ct = default);

    Task UpdateArticle(int id, Status status, CancellationToken ct = default);

    Task DeleteArticle(int id, CancellationToken ct = default);
}
