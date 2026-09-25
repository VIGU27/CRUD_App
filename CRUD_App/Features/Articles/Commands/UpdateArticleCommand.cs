using CRUD_App.Exceptions;
using CRUD_App.Models;
using CRUD_App.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CRUD_App.Features.Articles.Commands;

public class UpdateArticleCommand : IRequest
{
    public UpdateArticleCommand(int id, Status status)
    {
        Id = id;
        Status = status;
    }

    public int Id { get; set; }
    public Status Status { get; set; }
}

public class UpdateArticleCommandHandler : IRequestHandler<UpdateArticleCommand>
{
    private readonly IArticleRepository _articleRepository;
    private readonly ILogger<UpdateArticleCommandHandler> _logger;

    public UpdateArticleCommandHandler(IArticleRepository articleRepository, ILogger<UpdateArticleCommandHandler> logger)
    {
        _articleRepository = articleRepository;
        _logger = logger;
    }

    public async Task Handle(UpdateArticleCommand request, CancellationToken ct)
    {
        try
        {
            var result = await _articleRepository.GetByIdAsync(request.Id, ct)
                ?? throw new NotFoundException($"Article with Id {request.Id} -- not found.");

            result.Status = request.Status;

            await _articleRepository.UpdateArticle(result, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update Article with Id {ArticleId}", request.Id);
            throw;
        }
    }
}
