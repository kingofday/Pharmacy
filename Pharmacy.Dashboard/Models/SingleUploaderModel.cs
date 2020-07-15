using Pharmacy.Domain;

namespace Pharmacy.Dashboard
{
    public class SingleUploaderModel
    {
        public SingleUploaderModel(string id ,string name, ProductAsset productAsset)
        {
            Id = id;
            Name = name;
            if (productAsset != null)
            {
                HaveAsset = true;
                AssetId = productAsset.ProductAssetId;
                Url = productAsset.FileUrl;
            }
        }
        public string Id { get; set; }
        public int AssetId { get; set; } = 0;
        public string Name { get; set; }
        public bool HaveAsset { get; set; } = false;
        public string Url { get; set; }
        public string Accept { get; set; } = "image/*";
    }
}
