using CRUD_App.Data;
using Microsoft.EntityFrameworkCore;

namespace CRUD_App.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;

    public UserRepository(AppDbContext db)
    {
        _db = db;
    }

    public Task<bool> ExistsAsync(int id, CancellationToken ct = default) =>
        _db.Users.AnyAsync(u => u.Id == id, ct);
}
