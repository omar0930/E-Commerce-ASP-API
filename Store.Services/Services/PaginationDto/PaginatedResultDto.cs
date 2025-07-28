namespace Store.Services.Services
{
    public class PaginatedResultDto<TEntity>
    {
        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public IEnumerable<TEntity> Items { get; set; }
        public PaginatedResultDto(int totalCount, int pageIndex, int pageSize, IEnumerable<TEntity> items)
        {
            TotalCount = totalCount;
            PageIndex = pageIndex;
            PageSize = pageSize;
            Items = items;
        }
    }
}
