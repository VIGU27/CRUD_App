using CRUD_App.Repositories;
using CRUD_App.ResponseModels;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CRUD_App.Features.Contents.Queries;

/// <summary>Returns all content rows; default order is CreatedAt desc.</summary>
public class GetContentsQuery : IRequest<List<ContentResponse>>
{
}

public class GetContentsQueryHandler : IRequestHandler<GetContentsQuery, List<ContentResponse>>
{
    private readonly IContentRepository _contentRepository;
    private readonly ILogger<GetContentsQueryHandler> _logger;

    public GetContentsQueryHandler(IContentRepository contentRepository, ILogger<GetContentsQueryHandler> logger)
    {
        _contentRepository = contentRepository;
        _logger = logger;
    }

    public async Task<List<ContentResponse>> Handle(GetContentsQuery request, CancellationToken ct)
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
}
