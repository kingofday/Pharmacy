namespace Pharmacy.Domain
{
    public enum DrugFilterType : byte
    {
        Newest = 0,
        BestSellers = 1,
        MostVisited = 2,
        PriceDesc = 3,
        PriceAsc = 4,
        Favorites = 5,
        Score = 6
    }
}
