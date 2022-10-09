namespace SistemasSalgados.Models.Base
{
    public class QueryParams
    {
        public int Offset { get; set; } = 0;
        public bool BringAll { get; set; }
        public bool ActiveItems { get; set; } = true;
        public int PageSize { get; set; } = 40;
        public string Search { get; set; }
        public int? ItemId { get; set; }
    }
}
