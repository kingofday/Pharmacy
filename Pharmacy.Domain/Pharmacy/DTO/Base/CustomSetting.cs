namespace Pharmacy.Domain
{
    public class CustomSetting
    {
        public int EndUserRoleId { get; set; }
        public int DefaultGatewayId { get; set; }
        public Jwt Jwt { get; set; }
        public Delivery Delivery { get; set; }
        public ShowPaymentResult ShowPaymentResult { get; set; }
    }

    public class Jwt {
        public int TimoutInMinutes { get; set; }
        public string Key { get; set; }
        public string Issuer { get; set; }
    }
    public class Delivery
    {
        public string Price { get; set; }
        public string Add { get; set; }
    }

    public class ShowPaymentResult
    {
        public string Controller { get; set; }
        public string Action { get; set; }
        public string ReactUrl { get; set; }
    }
}
