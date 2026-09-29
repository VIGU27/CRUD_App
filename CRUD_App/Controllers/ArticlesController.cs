using CRUD_App.Exceptions;
using CRUD_App.RequestModels;
using CRUD_App.ResponseModels;
using CRUD_App.Services;
using Microsoft.AspNetCore.Mvc;

namespace CRUD_App.Controllers;

/// <summary>CRUD and query endpoints for Articles.</summary>
[ApiController]
[Route("api/articles")]
[Produces("application/json")]
public class ArticlesController : ControllerBase
{
    private readonly IArticleService _articleService;

    public ArticlesController(IArticleService articleService)
    {
        _articleService = articleService;
    }

    /// <summary>
    /// Create new article
    /// </summary>
    /// <remarks>
    /// please use Draft/Published/Unpublished as Status values.
    /// </remarks>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<ArticleResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<ArticleResponse>>> Create(ArticleCreateRequest request)
    {
        var result = await _articleService.CreateArticle(request.Status);

        var response = ApiResponse<ArticleResponse>.Success(
            result, StatusCodes.Status201Created, $"Successfully created record with ID {result.Id}");

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, response);
    }

    /// <summary>
    /// Gets all articles
    /// </summary>
    [HttpGet("getAll")]
    [ProducesResponseType(typeof(ApiResponse<List<ArticleResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<List<ArticleResponse>>>> GetAll()
    {
        var result = await _articleService.GetAllArticle();

        return Ok(ApiResponse<List<ArticleResponse>>.Success(
            result, StatusCodes.Status200OK, "Successfully retrieved records"));
    }

    /// <summary>
    /// Gets article by Id.
    /// </summary>
    [HttpGet("{id:int}/getById")]
    [ProducesResponseType(typeof(ApiResponse<ArticleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ArticleResponse>>> GetById(int id)
    {
        var result = await _articleService.GetArticleById(id);

        return Ok(ApiResponse<ArticleResponse>.Success(
            result, StatusCodes.Status200OK, $"Successfully retrieved record with ID {id}"));
    }

    /// <summary>
    /// Article details with list of its content items.
    /// </summary>
    [HttpGet("{id:int}/details")]
    [ProducesResponseType(typeof(ApiResponse<ArticleDetailsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ArticleDetailsResponse>>> GetDetails(int id)
    {
        var result = await _articleService.GetArticleDetails(id);

        return Ok(ApiResponse<ArticleDetailsResponse>.Success(
            result, StatusCodes.Status200OK, $"Successfully retrieved details for record with ID {id}"));
    }

    /// <summary>
    /// Filters and sorts articles.
    /// </summary>
    /// <remarks>
    /// Sorting:
    /// - SortBy: Title or CreatedAt
    /// - SortDir: Asc or Desc
    /// - Defaults to CreatedAt descending
    ///
    /// Filter by Status. (All/Draft/Published/Unpublished)
    /// </remarks>
    [HttpPost("filter")]
    [ProducesResponseType(typeof(ApiResponse<PagedResponse<ArticleListItemResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResponse<ArticleListItemResponse>>>> Filter(ArticleFilterRequest request)
    {
        var result = await _articleService.FilterArticle(
            request.Page, request.PageSize, request.SortBy, request.SortDir, request.Status);

        return Ok(ApiResponse<PagedResponse<ArticleListItemResponse>>.Success(
            result, StatusCodes.Status200OK, "Successfully retrieved filtered records"));
    }

    /// <summary>
    /// Updates article
    /// </summary>
    /// <remarks>
    /// please use Draft/Published/Unpublished as Status values.
    /// </remarks>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<object>>> Update(int id, ArticleUpdateRequest request)
    {
        try
        {
            await _articleService.UpdateArticle(id, request.Status);

            return Ok(ApiResponse<object>.Success(
                new { }, StatusCodes.Status200OK, $"Successfully updated record with ID {id}"));
        }
        catch (NotFoundException)
        {
            return NotFound(ApiResponse<object>.Failure(
                StatusCodes.Status404NotFound, $"Failed to update record with ID {id}"));
        }
    }

    /// <summary>
    /// Deletes an article
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
    {
        try
        {
            await _articleService.DeleteArticle(id);

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
