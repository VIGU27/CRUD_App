using CRUD_App.Exceptions;
using CRUD_App.Models;
using CRUD_App.Repositories;
using CRUD_App.ResponseModels;
using Microsoft.Extensions.Logging;

namespace CRUD_App.Services;

public class ContentService : IContentService
{
    private readonly IContentRepository _contentRepository;
    private readonly IUserRepository _userRepository;
    private readonly IArticleRepository _articleRepository;
    private readonly ILogger<ContentService> _logger;

    public ContentService(
        IContentRepository contentRepository,
        IUserRepository userRepository,
        IArticleRepository articleRepository,
        ILogger<ContentService> logger)
    {
        _contentRepository = contentRepository;
        _userRepository = userRepository;
        _articleRepository = articleRepository;
        _logger = logger;
    }

    public async Task<ContentResponse> CreateContent(
        string title,
        string body,
        int authorId,
        Status status,
        Language language,
        int? articleId,
        CancellationToken ct = default)
    {
        try
        {
            if (!await _userRepository.UserExists(authorId, ct))
            {
                throw new ValidationException($"Author with Id {authorId} does not exist.");
            }

            if (articleId is not null && await _articleRepository.GetArticleById(articleId.Value, ct) is null)
            {
                throw new ValidationException($"Article with Id {articleId} does not exist.");
            }

            var content = new Content
            {
                Title = title,
                Body = body,
                AuthorId = authorId,
                Status = status,
                Language = language,
                ArticleId = articleId,
                CreatedAt = DateTime.UtcNow
            };

            await _contentRepository.AddContent(content, ct);

            return ContentMapper.ToResponse(content);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create Content with Title {Title}", title);
            throw;
        }
    }

    public async Task<List<ContentResponse>> GetAllContent(CancellationToken ct = default)
    {
        try
        {
            var contents = await _contentRepository.GetAllContent(ct);
            return contents.Select(ContentMapper.ToResponse).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get all content");
            throw;
        }
    }

    public async Task<ContentResponse> GetContentById(int id, CancellationToken ct = default)
    {
        try
        {
            var content = await _contentRepository.GetContentById(id, ct)
                ?? throw new NotFoundException($"Content with Id {id} was not found.");

            return ContentMapper.ToResponse(content);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get Content with Id {ContentId}", id);
            throw;
        }
    }

    public async Task UpdateContent(
        int id,
        string title,
        string body,
        int authorId,
        Status status,
        Language language,
        int? articleId,
        CancellationToken ct = default)
    {
        try
        {
            var content = await _contentRepository.GetContentById(id, ct)
                ?? throw new NotFoundException($"Content with Id {id} was not found.");

            if (!await _userRepository.UserExists(authorId, ct))
            {
                throw new ValidationException($"Author with Id {authorId} does not exist.");
            }

            if (articleId is not null && await _articleRepository.GetArticleById(articleId.Value, ct) is null)
            {
                throw new ValidationException($"Article with Id {articleId} does not exist.");
            }

            content.Title = title;
            content.Body = body;
            content.AuthorId = authorId;
            content.Status = status;
            content.Language = language;
            content.ArticleId = articleId;

            await _contentRepository.UpdateContent(content, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update Content with Id {ContentId}", id);
            throw;
        }
    }

    public async Task DeleteContent(int id, CancellationToken ct = default)
    {
        try
        {
            var content = await _contentRepository.GetContentById(id, ct)
                ?? throw new NotFoundException($"Content with Id {id} was not found.");

            await _contentRepository.DeleteContent(content, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete Content with Id {ContentId}", id);
            throw;
        }
    }
}
