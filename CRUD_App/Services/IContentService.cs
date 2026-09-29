using CRUD_App.Models;
using CRUD_App.ResponseModels;

namespace CRUD_App.Services;

public interface IContentService
{
    Task<ContentResponse> CreateContent(
        string title,
        string body,
        int authorId,
        Status status,
        Language language,
        int? articleId,
        CancellationToken ct = default);

    Task<List<ContentResponse>> GetAllContent(CancellationToken ct = default);

    Task<ContentResponse> GetContentById(int id, CancellationToken ct = default);

    Task UpdateContent(
        int id,
        string title,
        string body,
        int authorId,
        Status status,
        Language language,
        int? articleId,
        CancellationToken ct = default);

    Task DeleteContent(int id, CancellationToken ct = default);
}
