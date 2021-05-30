namespace Zembil.Utils
{
    public class QueryFilterParams
    {
        public int Limit { get; set; }
        public string Sort { get; set; }
        public int Pagination { get; set; }
    }
    public class QuerySearchParams
    {
        public string Name { get; set; }
        public int Category { get; set; }
    }
    public class ProductSearchQuery : QuerySearchParams
    {
        public string Brand { get; set; }
    }

    public class ShopSearchQuery : QuerySearchParams
    {
        public string Building { get; set; }
        public double NearByRadius { get; set; }
    }

    public class TrendingQuery
    {
        public int Latest { get; set; }
        public int Popular { get; set; }
    }
}