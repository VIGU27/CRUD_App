using CRUD_App.Models;

namespace CRUD_App.Repositories;

public interface IContentRepository
{
    Task<Content?> GetByIdAsync(int id, CancellationToken ct = default);


    Task<List<Content>> GetAllContent(CancellationToken ct = default);

    Task<Content> AddContent(Content content, CancellationToken ct = default);

    Task UpdateContent(Content content, CancellationToken ct = default);

    Task DeleteContent(Content content, CancellationToken ct = default);
}
