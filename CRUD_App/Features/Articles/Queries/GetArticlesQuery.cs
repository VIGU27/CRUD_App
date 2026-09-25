using CRUD_App.Repositories;
using CRUD_App.ResponseModels;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CRUD_App.Features.Articles.Queries;

public class GetArticlesQuery : IRequest<List<ArticleResponse>>
{

};

public class GetArticlesQueryHandler : IRequestHandler<GetArticlesQuery, List<ArticleResponse>>
{
    private readonly IArticleRepository _articleRepository;
    private readonly ILogger<GetArticlesQueryHandler> _logger;

    public GetArticlesQueryHandler(IArticleRepository articleRepository, ILogger<GetArticlesQueryHandler> logger)
    {
        _articleRepository = articleRepository;
        _logger = logger;
    }

    public async Task<List<ArticleResponse>> Handle(GetArticlesQuery request, CancellationToken ct)
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
}
