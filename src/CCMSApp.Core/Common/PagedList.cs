namespace CCMSApp.Core.Common;

/// <summary>
/// A paginated list of items.
/// </summary>
/// <typeparam name="T">The type of the items.</typeparam>
public class PagedList<T> : List<T>
{
    public PagedList(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
    {
        TotalCount = totalCount;
        PageSize = pageSize;
        CurrentPage = pageNumber;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        AddRange(items);
    }

    public int CurrentPage { get; }

    public int TotalPages { get; }

    public int PageSize { get; }

    public int TotalCount { get; }

    public bool HasPrevious => CurrentPage > 1;

    public bool HasNext => CurrentPage < TotalPages;
}
