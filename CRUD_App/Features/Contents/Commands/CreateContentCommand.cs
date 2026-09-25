using CRUD_App.Exceptions;
using CRUD_App.Models;
using CRUD_App.Repositories;
using CRUD_App.ResponseModels;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CRUD_App.Features.Contents.Commands;

public class CreateContentCommand : IRequest<ContentResponse>
{
    public CreateContentCommand(string title, string body, int authorId, Status status, Language language, int? articleId)
    {
        Title = title;
        Body = body;
        AuthorId = authorId;
        Status = status;
        Language = language;
        ArticleId = articleId;
    }
    public string Title { get; set; }
    public string Body { get; set; }
    public int AuthorId { get; set; }
    public Status Status { get; set; }
    public Language Language { get; set; }
    public int? ArticleId { get; set; }
}

public class CreateContentCommandHandler : IRequestHandler<CreateContentCommand, ContentResponse>
{
    private readonly IContentRepository _contentRepository;
    private readonly IUserRepository _userRepository;
    private readonly IArticleRepository _articleRepository;
    private readonly ILogger<CreateContentCommandHandler> _logger;

    public CreateContentCommandHandler(
        IContentRepository contentRepository,
        IUserRepository userRepository,
        IArticleRepository articleRepository,
        ILogger<CreateContentCommandHandler> logger)
    {
        _contentRepository = contentRepository;
        _userRepository = userRepository;
        _articleRepository = articleRepository;
        _logger = logger;
    }

    public async Task<ContentResponse> Handle(CreateContentCommand request, CancellationToken ct)
    {
        try
        {
            if (!await _userRepository.ExistsAsync(request.AuthorId, ct))
            {
                throw new ValidationException($"Author with Id {request.AuthorId} does not exist.");
            }

            if (request.ArticleId is not null && await _articleRepository.GetByIdAsync(request.ArticleId.Value, ct) is null)
            {
                throw new ValidationException($"Article with Id {request.ArticleId} does not exist.");
            }

            var content = new Content
            {
                Title = request.Title,
                Body = request.Body,
                AuthorId = request.AuthorId,
                Status = request.Status,
                Language = request.Language,
                ArticleId = request.ArticleId,
                CreatedAt = DateTime.UtcNow
            };

            await _contentRepository.AddContent(content, ct);

            return ContentMapper.ToResponse(content);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create Content with Title {Title}", request.Title);
            throw;
        }
    }
}
