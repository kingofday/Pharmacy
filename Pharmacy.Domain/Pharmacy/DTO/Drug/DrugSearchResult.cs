﻿namespace Pharmacy.Domain
{
    public class DrugSearchResult
    {
        public string NameFa { get; set; }
        public string NameEn { get; set; }
        public int Id { get; set; }
        public string UniqueId { get; set; }
        public int Price { get; set; }
        public int DiscountPrice { get; set; }
    }
}
