namespace Pharmacy.Domain
{
    public class APICustomSetting : BaseCustomSetting
    {
        public int EndUserRoleId { get; set; }
        public int DefaultGatewayId { get; set; }
        public string DashboardAddPrescriptionUrl { get; set; }
        public Jwt Jwt { get; set; }

        public ShowPaymentResult ShowPaymentResult { get; set; }
    }

    public class Jwt
    {
        public int TimoutInMinutes { get; set; }
        public string Key { get; set; }
        public string Issuer { get; set; }
    }

    public class ShowPaymentResult
    {
        public string Controller { get; set; }
        public string Action { get; set; }
        public string ReactUrl { get; set; }
    }
}
