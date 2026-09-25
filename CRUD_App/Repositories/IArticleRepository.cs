using CRUD_App.Models;

namespace CRUD_App.Repositories;

public interface IArticleRepository
{
    Task<Article> AddArticle(Article article, CancellationToken ct = default);
    Task<Article?> GetByIdAsync(int id, CancellationToken ct = default);

    Task<Article?> GetWithContentsAsync(int id, CancellationToken ct = default);
    Task<List<Article>> GetAllArticle(CancellationToken ct = default);
    Task UpdateArticle(Article article, CancellationToken ct = default);
    Task DeleteArticle(Article article, CancellationToken ct = default);

    Task<(List<ArticleListRow> Items, int TotalCount)> GetFilteredArticles(
        int page,
        int pageSize,
        ArticleSortBy sortBy,
        SortDirection sortDir,
        Status? status,
        CancellationToken ct = default);
}
