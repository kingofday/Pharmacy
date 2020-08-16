using System.ComponentModel;

namespace Pharmacy.Domain
{
    public enum AttachmentType : byte
    {
        DrugStoreImage = 0,

        DrugThumbnailImage = 1,

        DrugImage = 2,

        [Description("عکس پرسنلی")]
        PersoneliPicture = 3,

        [Description("تصویر نسخه")]
        PrescriptionImage = 3,
    }
}
