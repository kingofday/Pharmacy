using System;

namespace Pharmacy.Domain
{
    public enum UserStatus : byte
    {
        MobileVerified = 1,
        AddDrugStore = 2,
        AddAddress = 3,
        UploadDocument = 4,
        AddBankAcount = 5
    }
}
