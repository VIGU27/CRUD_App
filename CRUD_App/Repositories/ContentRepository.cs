using CRUD_App.Data;
using CRUD_App.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUD_App.Repositories;

public class ContentRepository : IContentRepository
{
    private readonly AppDbContext _db;

    public ContentRepository(AppDbContext db)
    {
        _db = db;
    }

    public Task<Content?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _db.Contents.Include(c => c.Author).FirstOrDefaultAsync(c => c.Id == id, ct);

    public Task<List<Content>> GetAllContent(CancellationToken ct = default) =>
        _db.Contents.Include(c => c.Author).OrderByDescending(c => c.CreatedAt).ToListAsync(ct);

    public async Task<Content> AddContent(Content content, CancellationToken ct = default)
    {
        _db.Contents.Add(content);
        await _db.SaveChangesAsync(ct);
        await _db.Entry(content).Reference(c => c.Author).LoadAsync(ct);
        return content;
    }

    public async Task UpdateContent(Content content, CancellationToken ct = default)
    {
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteContent(Content content, CancellationToken ct = default)
    {
        _db.Contents.Remove(content);
        await _db.SaveChangesAsync(ct);
    }
}
