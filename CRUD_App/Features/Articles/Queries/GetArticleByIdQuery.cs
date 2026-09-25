using CRUD_App.Exceptions;
using CRUD_App.Repositories;
using CRUD_App.ResponseModels;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CRUD_App.Features.Articles.Queries;
public class GetArticleByIdQuery : IRequest<ArticleResponse>
{
    public GetArticleByIdQuery(int id)
    {
        Id = id;
    }

    public int Id { get; set; }
}

public class GetArticleByIdQueryHandler : IRequestHandler<GetArticleByIdQuery, ArticleResponse>
{
    private readonly IArticleRepository _articleRepository;
    private readonly ILogger<GetArticleByIdQueryHandler> _logger;

    public GetArticleByIdQueryHandler(IArticleRepository articleRepository, ILogger<GetArticleByIdQueryHandler> logger)
    {
        _articleRepository = articleRepository;
        _logger = logger;
    }

    public async Task<ArticleResponse> Handle(GetArticleByIdQuery request, CancellationToken ct)
    {
        try
        {
            var resultt = await _articleRepository.GetByIdAsync(request.Id, ct)
                ?? throw new NotFoundException($"Article with Id {request.Id} was not found.");

            return ArticleMapper.ToResponse(resultt);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get Article with Id {ArticleId}", request.Id);
            throw;
        }
    }
}
