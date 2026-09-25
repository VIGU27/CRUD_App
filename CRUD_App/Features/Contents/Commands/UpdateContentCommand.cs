using CRUD_App.Exceptions;
using CRUD_App.Models;
using CRUD_App.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CRUD_App.Features.Contents.Commands;

public class UpdateContentCommand : IRequest
{
    public UpdateContentCommand(int id, string title, string body, int authorId, Status status, Language language, int? articleId)
    {
        Id = id;
        Title = title;
        Body = body;
        AuthorId = authorId;
        Status = status;
        Language = language;
        ArticleId = articleId;
    }

    public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public int AuthorId { get; set; }
    public Status Status { get; set; }
    public Language Language { get; set; }
    public int? ArticleId { get; set; }
}

public class UpdateContentCommandHandler : IRequestHandler<UpdateContentCommand>
{
    private readonly IContentRepository _contentRepository;
    private readonly IUserRepository _userRepository;
    private readonly IArticleRepository _articleRepository;
    private readonly ILogger<UpdateContentCommandHandler> _logger;

    public UpdateContentCommandHandler(
        IContentRepository contentRepository,
        IUserRepository userRepository,
        IArticleRepository articleRepository,
        ILogger<UpdateContentCommandHandler> logger)
    {
        _contentRepository = contentRepository;
        _userRepository = userRepository;
        _articleRepository = articleRepository;
        _logger = logger;
    }

    public async Task Handle(UpdateContentCommand request, CancellationToken ct)
    {
        try
        {
            var content = await _contentRepository.GetByIdAsync(request.Id, ct)
                ?? throw new NotFoundException($"Content with Id {request.Id} was not found.");

            if (!await _userRepository.ExistsAsync(request.AuthorId, ct))
            {
                throw new ValidationException($"Author with Id {request.AuthorId} does not exist.");
            }

            if (request.ArticleId is not null && await _articleRepository.GetByIdAsync(request.ArticleId.Value, ct) is null)
            {
                throw new ValidationException($"Article with Id {request.ArticleId} does not exist.");
            }

            content.Title = request.Title;
            content.Body = request.Body;
            content.AuthorId = request.AuthorId;
            content.Status = request.Status;
            content.Language = request.Language;
            content.ArticleId = request.ArticleId;

            await _contentRepository.UpdateContent(content, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update Content with Id {ContentId}", request.Id);
            throw;
        }
    }
}
