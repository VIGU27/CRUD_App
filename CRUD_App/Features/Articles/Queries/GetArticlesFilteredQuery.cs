using CRUD_App.Models;
using CRUD_App.Repositories;
using CRUD_App.ResponseModels;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CRUD_App.Features.Articles.Queries;

public class GetArticlesFilteredQuery : IRequest<PagedResponse<ArticleListItemResponse>>
{
    public GetArticlesFilteredQuery(int page, int pageSize, ArticleSortBy sortBy, SortDirection sortDir, StatusFilter status)
    {
        Page = page;
        PageSize = pageSize;
        SortBy = sortBy;
        SortDir = sortDir;
        Status = status;
    }

    public int Page { get; set; }
    public int PageSize { get; set; }
    public ArticleSortBy SortBy { get; set; }
    public SortDirection SortDir { get; set; }
    public StatusFilter Status { get; set; }
}

public class GetArticlesFilteredQueryHandler
    : IRequestHandler<GetArticlesFilteredQuery, PagedResponse<ArticleListItemResponse>>
{
    private readonly IArticleRepository _articleRepository;
    private readonly ILogger<GetArticlesFilteredQueryHandler> _logger;

    public GetArticlesFilteredQueryHandler(IArticleRepository articleRepository, ILogger<GetArticlesFilteredQueryHandler> logger)
    {
        _articleRepository = articleRepository;
        _logger = logger;
    }

    public async Task<PagedResponse<ArticleListItemResponse>> Handle(GetArticlesFilteredQuery request, CancellationToken ct)
    {
        try
        {
            var page = Math.Max(request.Page, 0);
            var pageSize = Math.Clamp(request.PageSize, 1, 100);

            Status? status = request.Status == StatusFilter.All
                ? null
                : Enum.Parse<Status>(request.Status.ToString());

            var (items, totalCount) = await _articleRepository.GetFilteredArticles(
                page, pageSize, request.SortBy, request.SortDir, status, ct);

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
}
