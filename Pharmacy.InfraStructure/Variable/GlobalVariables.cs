using System;

namespace Pharmacy.InfraStructure
{
    public static class GlobalVariables
    {
        public static class SmsProviders
        {
            public static class LinePayamak
            {
                public static string Username = "500096998998";
                public static string Password = "80225353";
                public static string SenderId = "500096998998";
            }
        }

        public static class DeliveryProviders
        {
            public static class AloPeik
            {
                public static string Url = "https://sandbox-api.alopeyk.com/api/v2";
                public static string Token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzdWIiOjExMjkwLCJpc3MiOiJodHRwczovL3NhbmRib3gtYXBpLmFsb3BleWsuY29tL2FwaS92MiIsImlhdCI6MTU4MjgxNDk1NCwiZXhwIjo1MTgyODE0OTU0LCJuYmYiOjE1ODI4MTQ5NTQsImp0aSI6Ikpob1l6Mjh6d1VlME1Ec0QifQ.lyXrDtUXjNmouQPwpZ-dQSyWftjIsChhJdgB9FZvBhQ";
            }
        }

        public static class CacheSettings
        {
            public static string MenuModelCacheKey(Guid userId) => $"MenuModel_{userId.ToString().Replace("-", "_")}";
        }
    }
}
