using CRUD_App.Exceptions;
using CRUD_App.Models;
using CRUD_App.Repositories;
using CRUD_App.ResponseModels;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CRUD_App.Features.Articles.Commands;

public class CreateArticleCommand : IRequest<ArticleResponse>
{
    public CreateArticleCommand(Status status)
    {
        Status = status;
    }

    public Status Status { get; set; }
}

public class CreateArticleCommandHandler : IRequestHandler<CreateArticleCommand, ArticleResponse>
{
    private readonly IArticleRepository _articleRepository;
    private readonly ILogger<CreateArticleCommandHandler> _logger;

    public CreateArticleCommandHandler(IArticleRepository articleRepository, ILogger<CreateArticleCommandHandler> logger)
    {
        _articleRepository = articleRepository;
        _logger = logger;
    }

    public async Task<ArticleResponse> Handle(CreateArticleCommand request, CancellationToken ct)
    {
        try
        {
            var article = new Article
            {
                Status = request.Status,
                CreatedAt = DateTime.UtcNow
            };

            await _articleRepository.AddArticle(article, ct);

            return ArticleMapper.ToResponse(article);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create Article with Status {Status}", request.Status);
            throw;
        }
    }
}
