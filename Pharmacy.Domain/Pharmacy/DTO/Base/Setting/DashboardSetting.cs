namespace Pharmacy.Domain
{
    public class DashboardCustomSetting : BaseCustomSetting
    {
        public string UrlPrefix { get; set; }
        public string BaseUrl { get; set; }
        public string ReactBaseUrl { get; set; }
        public string ReactTempBasketUrl { get; set; }
    }
    public class EmailServiceConfig
    {
        public string EmailHost { get; set; }
        public string EmailUserName { get; set; }
        public string EmailPassword { get; set; }
    }

}
