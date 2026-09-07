using CCMSApp.Core.Common;
using MapsterMapper;

namespace CCMSApp.WebApi.Common.Mappings;

/// <summary>
/// Helper methods for mapping paginated lists.
/// </summary>
public static class MappingExtensions
{
    public static PagedList<TDestination> MapPagedList<TSource, TDestination>(
        this IMapper mapper,
        PagedList<TSource> source)
    {
        var items = mapper.Map<List<TDestination>>(source);
        return new PagedList<TDestination>(items, source.TotalCount, source.CurrentPage, source.PageSize);
    }
}
