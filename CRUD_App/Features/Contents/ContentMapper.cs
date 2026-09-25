using CRUD_App.Models;
using CRUD_App.ResponseModels;

namespace CRUD_App.Features.Contents;

internal static class ContentMapper
{
    public static ContentResponse ToResponse(Content c) => new()
    {
        Id = c.Id,
        Title = c.Title,
        Body = c.Body,
        AuthorId = c.AuthorId,
        AuthorUsername = c.Author?.Username ?? string.Empty,
        Status = c.Status,
        Language = c.Language,
        CreatedAt = c.CreatedAt,
        ArticleId = c.ArticleId
    };
}
