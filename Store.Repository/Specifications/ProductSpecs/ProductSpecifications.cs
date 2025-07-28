namespace Store.Repository.Specifications.ProductSpecs
{
    public class ProductSpecifications
    {
        public int? BrandId { get; set; }
        public int? CategoryId { get; set; }
        public string? Sort { get; set; }
        public int _pageSize { get; set; } = 6;
        public int PageIndex { get; set; } = 1;
        private const int MaxPageSize = 50;
        private string _search;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        public string? Search
        {
            get => _search;
            set => _search = value?.Trim().ToLower();
        }
    }
}
