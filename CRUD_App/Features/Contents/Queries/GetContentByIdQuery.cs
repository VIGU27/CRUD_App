using CRUD_App.Exceptions;
using CRUD_App.Repositories;
using CRUD_App.ResponseModels;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CRUD_App.Features.Contents.Queries;

public class GetContentByIdQuery : IRequest<ContentResponse>
{
    public GetContentByIdQuery(int id)
    {
        Id = id;
    }

    public int Id { get; set; }
}

public class GetContentByIdQueryHandler : IRequestHandler<GetContentByIdQuery, ContentResponse>
{
    private readonly IContentRepository _contentRepository;
    private readonly ILogger<GetContentByIdQueryHandler> _logger;

    public GetContentByIdQueryHandler(IContentRepository contentRepository, ILogger<GetContentByIdQueryHandler> logger)
    {
        _contentRepository = contentRepository;
        _logger = logger;
    }

    public async Task<ContentResponse> Handle(GetContentByIdQuery request, CancellationToken ct)
    {
        try
        {
            var content = await _contentRepository.GetByIdAsync(request.Id, ct)
                ?? throw new NotFoundException($"Content with Id {request.Id} was not found.");

            return ContentMapper.ToResponse(content);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get Content with Id {ContentId}", request.Id);
            throw;
        }
    }
}
