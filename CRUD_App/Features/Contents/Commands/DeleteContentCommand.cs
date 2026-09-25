using CRUD_App.Exceptions;
using CRUD_App.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CRUD_App.Features.Contents.Commands;

public class DeleteContentCommand : IRequest
{
    public DeleteContentCommand(int id)
    {
        Id = id;
    }

    public int Id { get; set; }
}

public class DeleteContentCommandHandler : IRequestHandler<DeleteContentCommand>
{
    private readonly IContentRepository _contentRepository;
    private readonly ILogger<DeleteContentCommandHandler> _logger;

    public DeleteContentCommandHandler(IContentRepository contentRepository, ILogger<DeleteContentCommandHandler> logger)
    {
        _contentRepository = contentRepository;
        _logger = logger;
    }

    public async Task Handle(DeleteContentCommand request, CancellationToken ct)
    {
        try
        {
            var content = await _contentRepository.GetByIdAsync(request.Id, ct)
                ?? throw new NotFoundException($"Content with Id {request.Id} was not found.");

            await _contentRepository.DeleteContent(content, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete Content with Id {ContentId}", request.Id);
            throw;
        }
    }
}
