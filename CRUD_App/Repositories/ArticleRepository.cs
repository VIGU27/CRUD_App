using CRUD_App.Data;
using CRUD_App.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUD_App.Repositories;

public class ArticleRepository : IArticleRepository
{
    private readonly AppDbContext _db;

    public ArticleRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Article> AddArticle(Article article, CancellationToken ct = default)
    {
        _db.Articles.Add(article);
        await _db.SaveChangesAsync(ct);
        return article;
    }
    public async Task<List<Article>> GetAllArticle(CancellationToken ct = default) =>
        await _db.Articles.OrderByDescending(a => a.CreatedAt).ToListAsync(ct);



    public Task<Article?> GetArticleById(int id, CancellationToken ct = default) =>
        _db.Articles.FirstOrDefaultAsync(a => a.Id == id, ct);

    public Task<Article?> GetArticleWithContents(int id, CancellationToken ct = default) =>
        _db.Articles
            .Include(a => a.Contents).ThenInclude(c => c.Author)
            .FirstOrDefaultAsync(a => a.Id == id, ct);

    public Task UpdateArticle(Article article, CancellationToken ct = default) => _db.SaveChangesAsync(ct);

    public async Task DeleteArticle(Article article, CancellationToken ct = default)
    {
        _db.Articles.Remove(article);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<(List<ArticleListRow> Items, int TotalCount)> GetFilteredArticles(
        int page,
        int pageSize,
        ArticleSortBy sortBy,
        SortDirection sortDir,
        Status? status,
        CancellationToken ct = default)
    {
        var query = _db.Articles.Select(a => new
        {
            a.Id,
            a.Status,
            a.CreatedAt,
            Primary = a.Contents
                .OrderByDescending(c => c.Language == Language.English)
                .ThenBy(c => c.CreatedAt)
                .Select(c => new { c.Title, AuthorUsername = c.Author.Username })
                .FirstOrDefault()
        });

        if (status is not null)
        {
            query = query.Where(a => a.Status == status.Value);
        }

        query = (sortBy, sortDir) switch
        {
            (ArticleSortBy.Title, SortDirection.Asc) => query.OrderBy(a => a.Primary!.Title),
            (ArticleSortBy.Title, SortDirection.Desc) => query.OrderByDescending(a => a.Primary!.Title),
            (_, SortDirection.Asc) => query.OrderBy(a => a.CreatedAt),
            _ => query.OrderByDescending(a => a.CreatedAt)
        };

        var totalCount = await query.CountAsync(ct);

        var items = await query
            .Skip(page * pageSize)
            .Take(pageSize)
            .Select(a => new ArticleListRow
            {
                Id = a.Id,
                Status = a.Status,
                CreatedAt = a.CreatedAt,
                Title = a.Primary == null ? null : a.Primary.Title,
                Author = a.Primary == null ? null : a.Primary.AuthorUsername
            })
            .ToListAsync(ct);

        return (items, totalCount);
    }
}
