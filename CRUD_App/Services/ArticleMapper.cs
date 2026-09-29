using CRUD_App.Models;
using CRUD_App.ResponseModels;

namespace CRUD_App.Services;

internal static class ArticleMapper
{
    public static ArticleResponse ToResponse(Article a) => new()
    {
        Id = a.Id,
        Status = a.Status,
        CreatedAt = a.CreatedAt
    };

    public static ArticleDetailsResponse ToDetailsResponse(Article a) => new()
    {
        Id = a.Id,
        Status = a.Status,
        CreatedAt = a.CreatedAt,
        Contents = a.Contents.Select(ContentMapper.ToResponse).ToList()
    };
}
