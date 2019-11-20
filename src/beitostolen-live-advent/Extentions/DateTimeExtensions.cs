using System;
namespace beitostolen_live_api.Extentions
{
    public static class DateTimeExtensions
    {
        public static bool BetweenDate(this DateTime currentDateTime, DateTime startDateTime, DateTime endDateTime)
        {
            return currentDateTime.Date >= startDateTime.Date && currentDateTime.Date <= endDateTime.Date;
        }
    }
}
