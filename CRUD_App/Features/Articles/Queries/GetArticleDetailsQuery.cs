using CRUD_App.Exceptions;
using CRUD_App.Repositories;
using CRUD_App.ResponseModels;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CRUD_App.Features.Articles.Queries;

public class GetArticleDetailsQuery : IRequest<ArticleDetailsResponse>
{
    public GetArticleDetailsQuery(int id)
    {
        Id = id;
    }

    public int Id { get; set; }
}

public class GetArticleDetailsQueryHandler : IRequestHandler<GetArticleDetailsQuery, ArticleDetailsResponse>
{
    private readonly IArticleRepository _articleRepository;
    private readonly ILogger<GetArticleDetailsQueryHandler> _logger;

    public GetArticleDetailsQueryHandler(IArticleRepository articleRepository, ILogger<GetArticleDetailsQueryHandler> logger)
    {
        _articleRepository = articleRepository;
        _logger = logger;
    }

    public async Task<ArticleDetailsResponse> Handle(GetArticleDetailsQuery request, CancellationToken ct)
    {
        try
        {
            var article = await _articleRepository.GetWithContentsAsync(request.Id, ct)
                ?? throw new NotFoundException($"Article with Id {request.Id} was not found.");

            return ArticleMapper.ToDetailsResponse(article);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get details for Article with Id {ArticleId}", request.Id);
            throw;
        }
    }
}
