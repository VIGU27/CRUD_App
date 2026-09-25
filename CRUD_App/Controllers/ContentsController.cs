using CRUD_App.Features.Contents.Commands;
using CRUD_App.Features.Contents.Queries;
using CRUD_App.RequestModels;
using CRUD_App.ResponseModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CRUD_App.Controllers;

[ApiController]
[Route("api/contents")]
[Produces("application/json")]
public class ContentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ContentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Create new content
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ContentResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ContentResponse>> Create(ContentCreateRequest request)
    {
        var result = await _mediator.Send(new CreateContentCommand(
            request.Title, 
            request.Body, 
            request.AuthorId, 
            request.Status, 
            request.Language, 
            request.ArticleId));

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Gets all content
    /// </summary>
    [HttpGet("getAll")]
    [ProducesResponseType(typeof(List<ContentResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ContentResponse>>> GetAll()
    {
        var result = await _mediator.Send(new GetContentsQuery());
        return Ok(result);
    }

    /// <summary>
    /// Gets content item by Id
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ContentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ContentResponse>> GetById(int id)
    {
        var result = await _mediator.Send(new GetContentByIdQuery(id));
        return Ok(result);
    }



    /// <summary>
    /// Updates content
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, ContentUpdateRequest request)
    {
        await _mediator.Send(new UpdateContentCommand(
            id, request.Title, request.Body, request.AuthorId, request.Status, request.Language, request.ArticleId));

        return NoContent();
    }

    /// <summary>
    /// Deletes content
    /// </summary>
    /// <param name="id">The content Id.</param>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteContentCommand(id));
        return NoContent();
    }
}
