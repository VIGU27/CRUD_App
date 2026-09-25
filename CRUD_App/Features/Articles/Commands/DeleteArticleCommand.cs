using CRUD_App.Exceptions;
using CRUD_App.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CRUD_App.Features.Articles.Commands;

public class DeleteArticleCommand : IRequest
{
    public DeleteArticleCommand(int id)
    {
        Id = id;
    }

    public int Id { get; set; }
}

public class DeleteArticleCommandHandler : IRequestHandler<DeleteArticleCommand>
{
    private readonly IArticleRepository _articleRepository;
    private readonly ILogger<DeleteArticleCommandHandler> _logger;

    public DeleteArticleCommandHandler(IArticleRepository articleRepository, ILogger<DeleteArticleCommandHandler> logger)
    {
        _articleRepository = articleRepository;
        _logger = logger;
    }

    public async Task Handle(DeleteArticleCommand request, CancellationToken ct)
    {
        try
        {
            var result = await _articleRepository.GetByIdAsync(request.Id, ct)
                ?? throw new NotFoundException($"Article with Id {request.Id} -- not found.");

            await _articleRepository.DeleteArticle(result, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete Article with Id {ArticleId}", request.Id);
            throw;
        }
    }
}
