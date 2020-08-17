using Pharmacy.Domain;

namespace Pharmacy.Dashboard
{
    public class SingleUploaderModel
    {
        public SingleUploaderModel(string id ,string name, BaseAttachment attch)
        {
            Id = id;
            Name = name;
            if (attch != null)
            {
                HaveAsset = true;
                AttachmentId = attch.AttachmentId;
                Url = attch.Url;
            }
        }
        public string Id { get; set; }
        public object AttachmentId { get; set; } = 0;
        public string Name { get; set; }
        public bool HaveAsset { get; set; } = false;
        public string Url { get; set; }
        public string Accept { get; set; } = "image/*";
    }
}
