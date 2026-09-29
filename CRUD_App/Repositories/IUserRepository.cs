namespace CRUD_App.Repositories;

public interface IUserRepository
{
    Task<bool> UserExists(int id, CancellationToken ct = default);
}
