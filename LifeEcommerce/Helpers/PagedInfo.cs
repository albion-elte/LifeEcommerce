namespace LifeEcommerce.Helpers
{
    public class PagedInfo<T> where T : class
    {
        public List<T> Items { get; set; }

        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }
}
