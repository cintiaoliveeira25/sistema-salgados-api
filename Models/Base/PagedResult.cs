namespace SistemasSalgados.Models.Base
{
    public class PagedResult<TEntity> where TEntity : BaseEntity
    {
        public List<TEntity> Data { get; set; }
        public int Count { get; set; }
        public int Pages => (Count + PageSize - 1) / PageSize;
        public int OffSet { get; set; }
        public int PageSize { get; set; }
        public bool BringAll { get; set; }
        public bool HasMore => (Count - OffSet) > PageSize;
        public PagedResult() { }
        public PagedResult(QueryParams queryParams)
        {
            PageSize = queryParams.PageSize;
            OffSet = queryParams.Offset;
            BringAll = queryParams.BringAll;
        }
    }
}
