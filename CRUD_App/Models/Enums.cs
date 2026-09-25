namespace CRUD_App.Models;

public enum Language
{
    English,
    French,
    Spanish
}

public enum Status
{
    Draft,
    Published,
    Unpublished
}

/// <summary>
/// Used only by the article filter endpoint; "All" means no status filter is applied.
/// </summary>
public enum StatusFilter
{
    All,
    Draft,
    Published,
    Unpublished
}

public enum ArticleSortBy
{
    CreatedAt,
    Title
}

public enum SortDirection
{
    Asc,
    Desc
}
