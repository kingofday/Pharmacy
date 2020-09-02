using System.ComponentModel;

namespace Pharmacy.Domain
{
    public enum AttachmentType : byte
    {
        DrugStoreLogo = 0,

        DrugThumbnailImage = 1,

        DrugImage = 2,

        [Description("عکس پرسنلی")]
        PersonelPicture = 3,

        [Description("تصویر نسخه")]
        PrescriptionImage = 4,
    }
}
