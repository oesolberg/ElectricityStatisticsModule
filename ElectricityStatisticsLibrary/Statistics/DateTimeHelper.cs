using System;

namespace ElectricityStatisticsLibrary.Statistics
{
    public static class DateTimeHelper
    {
        public static DateTime GetDateTimeToTheLastOfTheGivenHour(this DateTime inputDateTime)
        {
            return new DateTime(inputDateTime.Year,inputDateTime.Month,inputDateTime.Day,inputDateTime.Hour,59,59,997);
        }
    }
}