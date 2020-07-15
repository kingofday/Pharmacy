using System.ComponentModel;

namespace Pharmacy.Domain
{
    public enum AttachmentType : byte
    {
        PharmacyImage = 0,

        DrugThumbnailImage = 1,

        DrugImage = 2,

        [Description("کارت ملی(پشت و رو )")]
        NationalCard = 3,

        [Description("شناسنامه/پاسپورت/گواهینامه")]
        IdentityCard = 4,

        [Description("عکس پرسنلی")]
        PersoneliPicture = 5,
    }
}
