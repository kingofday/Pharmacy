namespace Pharmacy.Domain
{
    public class BaseCustomSetting
    {
        public string NotifierUrl { get; set; }
        public string NotifierToken { get; set; }
        public Delivery Delivery { get; set; }
    }

    public class Delivery
    {
        public string Price { get; set; }
        public string Add { get; set; }
    }
}
