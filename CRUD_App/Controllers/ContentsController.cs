using CRUD_App.Exceptions;
using CRUD_App.RequestModels;
using CRUD_App.ResponseModels;
using CRUD_App.Services;
using Microsoft.AspNetCore.Mvc;

namespace CRUD_App.Controllers;

/// <summary>CRUD and query endpoints for Content items.</summary>
[ApiController]
[Route("api/contents")]
[Produces("application/json")]
public class ContentsController : ControllerBase
{
    private readonly IContentService _contentService;

    public ContentsController(IContentService contentService)
    {
        _contentService = contentService;
    }

    /// <summary>
    /// Create new content
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<ContentResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<ContentResponse>>> Create(ContentCreateRequest request)
    {
        var result = await _contentService.CreateContent(
            request.Title,
            request.Body,
            request.AuthorId,
            request.Status,
            request.Language,
            request.ArticleId);

        var response = ApiResponse<ContentResponse>.Success(
            result, StatusCodes.Status201Created, $"Successfully created record with ID {result.Id}");

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, response);
    }

    /// <summary>
    /// Gets all content
    /// </summary>
    [HttpGet("getAll")]
    [ProducesResponseType(typeof(ApiResponse<List<ContentResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<List<ContentResponse>>>> GetAll()
    {
        var result = await _contentService.GetAllContent();

        return Ok(ApiResponse<List<ContentResponse>>.Success(
            result, StatusCodes.Status200OK, "Successfully retrieved records"));
    }

    /// <summary>
    /// Gets content item by Id
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<ContentResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ContentResponse>>> GetById(int id)
    {
        var result = await _contentService.GetContentById(id);

        return Ok(ApiResponse<ContentResponse>.Success(
            result, StatusCodes.Status200OK, $"Successfully retrieved record with ID {id}"));
    }

    /// <summary>
    /// Updates content
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<object>>> Update(int id, ContentUpdateRequest request)
    {
        try
        {
            await _contentService.UpdateContent(
                id, request.Title, request.Body, request.AuthorId, request.Status, request.Language, request.ArticleId);

            return Ok(ApiResponse<object>.Success(
                new { }, StatusCodes.Status200OK, $"Successfully updated record with ID {id}"));
        }
        catch (NotFoundException)
        {
            return NotFound(ApiResponse<object>.Failure(
                StatusCodes.Status404NotFound, $"Failed to update record with ID {id}"));
        }
        catch (ValidationException ex)
        {
            return BadRequest(ApiResponse<object>.Failure(
                StatusCodes.Status400BadRequest, $"Failed to update record with ID {id}: {ex.Message}"));
        }
    }

    /// <summary>
    /// Deletes content
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
    {
        try
        {
            await _contentService.DeleteContent(id);

            return Ok(ApiResponse<object>.Success(
                new { }, StatusCodes.Status200OK, $"Successfully deleted record with ID {id}"));
        }
        catch (NotFoundException)
        {
            return NotFound(ApiResponse<object>.Failure(
                StatusCodes.Status404NotFound, $"Failed to delete record with ID {id}"));
        }
    }
}
