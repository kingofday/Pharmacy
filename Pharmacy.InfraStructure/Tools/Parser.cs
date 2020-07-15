using System;

namespace Pharmacy.InfraStructure
{
    public class Parser
    {
        public static string ToString(object obj) => obj == null ? string.Empty : obj.ToString().Trim().Replace("{", "").Replace("}", "");

        public static int ToInt(object obj) => int.Parse(ToString(obj));

        public static bool ToBool(object obj) => bool.Parse(ToString(obj));

        public static DateTime ToDateTime(double timeStamp, bool isUnixTimeStamp = true)
        {
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

            if (isUnixTimeStamp)
            {
                // Unix timestamp is seconds past epoch
                dateTime = dateTime.AddSeconds(timeStamp).ToLocalTime();
            }
            else
            {
                // Java timestamp is AddMilliseconds past epoch
                dateTime = dateTime.AddMilliseconds(timeStamp).ToLocalTime();
            }

            return dateTime;
        }
    }
}
