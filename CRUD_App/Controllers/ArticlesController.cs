using CRUD_App.Features.Articles.Commands;
using CRUD_App.Features.Articles.Queries;
using CRUD_App.RequestModels;
using CRUD_App.ResponseModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CRUD_App.Controllers;

/// <summary>CRUD and query endpoints for Articles.</summary>
[ApiController]
[Route("api/articles")]
[Produces("application/json")]
public class ArticlesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ArticlesController(IMediator mediator)
    {
        _mediator = mediator;
    }



    /// <summary>
    /// Create new article
    /// </summary>
    /// <remarks>
    /// please use Draft/Published/Unpublished as Status values.
    /// </remarks>
    [HttpPost]
    [ProducesResponseType(typeof(ArticleResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ArticleResponse>> Create(ArticleCreateRequest request)
    {
        var result = await _mediator.Send(new CreateArticleCommand(request.Status));
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }


    /// <summary>
    /// Gets all articles
    /// </summary>
    [HttpGet("getAll")]
    [ProducesResponseType(typeof(List<ArticleResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ArticleResponse>>> GetAll()
    {
        var result = await _mediator.Send(new GetArticlesQuery());
        return Ok(result);
    }

    /// <summary>
    /// Gets a single article by Id.
    /// </summary>
    [HttpGet("{id:int}/getById")]
    [ProducesResponseType(typeof(ArticleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ArticleResponse>> GetById(int id)
    {
        var result = await _mediator.Send(new GetArticleByIdQuery(id));
        return Ok(result);
    }

    /// <summary>
    /// Article details with list of its content items.
    /// </summary>
    [HttpGet("{id:int}/details")]
    [ProducesResponseType(typeof(ArticleDetailsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ArticleDetailsResponse>> GetDetails(int id)
    {
        var result = await _mediator.Send(new GetArticleDetailsQuery(id));
        return Ok(result);
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
    /// Language defaults to English.
    /// 
    /// Filter by Status. (Draft/Published/Unpublished)
    /// </remarks>
    [HttpPost("filter")]
    [ProducesResponseType(typeof(PagedResponse<ArticleListItemResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResponse<ArticleListItemResponse>>> Filter(ArticleFilterRequest request)
    {
        var result = await _mediator.Send(new GetArticlesFilteredQuery(
            request.Page, request.PageSize, request.SortBy, request.SortDir, request.Status));

        return Ok(result);
    }



    /// <summary>
    /// Updates article
    /// </summary>
    /// /// <remarks>
    /// please use Draft/Published/Unpublished as Status values.
    /// </remarks>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, ArticleUpdateRequest request)
    {
        await _mediator.Send(new UpdateArticleCommand(id, request.Status));
        return NoContent();
    }

    /// <summary>
    /// Deletes an article. Any content referencing it is orphaned (ArticleId set to null).
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteArticleCommand(id));
        return NoContent();
    }
}
