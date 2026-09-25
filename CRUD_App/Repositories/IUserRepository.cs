namespace CRUD_App.Repositories;

public interface IUserRepository
{
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);
}
