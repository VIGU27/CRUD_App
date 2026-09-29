using CRUD_App.Exceptions;
using CRUD_App.Models;
using CRUD_App.Repositories;
using CRUD_App.ResponseModels;
using Microsoft.Extensions.Logging;

namespace CRUD_App.Services;

public class ArticleService : IArticleService
{
    private readonly IArticleRepository _articleRepository;
    private readonly ILogger<ArticleService> _logger;

    public ArticleService(IArticleRepository articleRepository, ILogger<ArticleService> logger)
    {
        _articleRepository = articleRepository;
        _logger = logger;
    }

    public async Task<ArticleResponse> CreateArticle(Status status, CancellationToken ct = default)
    {
        try
        {
            var article = new Article
            {
                Status = status,
                CreatedAt = DateTime.UtcNow
            };

            await _articleRepository.AddArticle(article, ct);

            return ArticleMapper.ToResponse(article);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create Article with Status {Status}", status);
            throw;
        }
    }

    public async Task<List<ArticleResponse>> GetAllArticle(CancellationToken ct = default)
    {
        try
        {
            var articles = await _articleRepository.GetAllArticle(ct);
            return articles.Select(ArticleMapper.ToResponse).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get all articles");
            throw;
        }
    }

    public async Task<ArticleResponse> GetArticleById(int id, CancellationToken ct = default)
    {
        try
        {
            var article = await _articleRepository.GetArticleById(id, ct)
                ?? throw new NotFoundException($"Article with Id {id} was not found.");

            return ArticleMapper.ToResponse(article);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get Article with Id {ArticleId}", id);
            throw;
        }
    }

    public async Task<ArticleDetailsResponse> GetArticleDetails(int id, CancellationToken ct = default)
    {
        try
        {
            var article = await _articleRepository.GetArticleWithContents(id, ct)
                ?? throw new NotFoundException($"Article with Id {id} was not found.");

            return ArticleMapper.ToDetailsResponse(article);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get details for Article with Id {ArticleId}", id);
            throw;
        }
    }

    public async Task<PagedResponse<ArticleListItemResponse>> FilterArticle(
        int page,
        int pageSize,
        ArticleSortBy sortBy,
        SortDirection sortDir,
        StatusFilter status,
        CancellationToken ct = default)
    {
        try
        {
            page = Math.Max(page, 0);
            pageSize = Math.Clamp(pageSize, 1, 100);

            Status? domainStatus = status == StatusFilter.All
                ? null
                : Enum.Parse<Status>(status.ToString());

            var (items, totalCount) = await _articleRepository.GetFilteredArticles(
                page, pageSize, sortBy, sortDir, domainStatus, ct);

            return new PagedResponse<ArticleListItemResponse>
            {
                Result = items.Select(i => new ArticleListItemResponse
                {
                    Id = i.Id,
                    Title = i.Title,
                    Author = i.Author,
                    Status = i.Status,
                    CreatedAt = i.CreatedAt
                }).ToList(),
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get filtered article list");
            throw;
        }
    }

    public async Task UpdateArticle(int id, Status status, CancellationToken ct = default)
    {
        try
        {
            var article = await _articleRepository.GetArticleById(id, ct)
                ?? throw new NotFoundException($"Article with Id {id} -- not found.");

            article.Status = status;

            await _articleRepository.UpdateArticle(article, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update Article with Id {ArticleId}", id);
            throw;
        }
    }

    public async Task DeleteArticle(int id, CancellationToken ct = default)
    {
        try
        {
            var article = await _articleRepository.GetArticleById(id, ct)
                ?? throw new NotFoundException($"Article with Id {id} was not found.");

            await _articleRepository.DeleteArticle(article, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete Article with Id {ArticleId}", id);
            throw;
        }
    }
}
