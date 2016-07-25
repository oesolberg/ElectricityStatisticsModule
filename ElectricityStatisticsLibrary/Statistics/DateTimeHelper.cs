using System;

namespace ElectricityStatisticsLibrary.Statistics
{
    public static class DateTimeHelper
    {

        public static DateTime AddWeeks(this DateTime inputDateTime, int numberOfWeeks)
        {
            return new DateTime(inputDateTime.Year, inputDateTime.Month, inputDateTime.Day).AddDays(numberOfWeeks * 7);
        }
        public static DateTime GetDateTimeToTheLastOfTheGivenHour(this DateTime inputDateTime)
        {
            return new DateTime(inputDateTime.Year, inputDateTime.Month, inputDateTime.Day, inputDateTime.Hour, 59, 59, 997);
        }

        public static DateTime GetDateTimeToTheLastOfTheGivenDay(this DateTime inputDateTime)
        {
            return new DateTime(inputDateTime.Year, inputDateTime.Month, inputDateTime.Day, 23, 59, 59, 997);
        }

        public static DateTime GetDateTimeForTheFirstDayOfTheGivenMonth(this DateTime inputDateTime)
        {
            return new DateTime(inputDateTime.Year,inputDateTime.Month,1);
        }
        public static DateTime GetDateTimeForTheFirstDayOfTheGivenWeek(this DateTime inputDateTime)
        {
            //Week always ends on sunday.
            if (inputDateTime.DayOfWeek == DayOfWeek.Monday)
            {
                return new DateTime(inputDateTime.Year, inputDateTime.Month, inputDateTime.Day);
            }
            if (inputDateTime.DayOfWeek == DayOfWeek.Sunday)
            {
                var firstDayOfWeekDateTime = inputDateTime.AddDays(-6);
                return new DateTime(firstDayOfWeekDateTime.Year, firstDayOfWeekDateTime.Month, firstDayOfWeekDateTime.Day);
            }

            var numberOfDaysToEndOfWeek = (int)inputDateTime.DayOfWeek - 1;
            var newDate = inputDateTime.AddDays(numberOfDaysToEndOfWeek * -1);
            //var endOfWeek=
            return new DateTime(newDate.Year, newDate.Month, newDate.Day);
        }

        public static DateTime GetDateTimeForTheLastDayOfTheGivenWeek(this DateTime inputDateTime)
        {
            //Week always ends on sunday.
            if (inputDateTime.DayOfWeek == DayOfWeek.Sunday)
            {
                return new DateTime(inputDateTime.Year, inputDateTime.Month, inputDateTime.Day, 23, 59, 59, 997);
            }
            var numberOfDaysToEndOfWeek = 7 - (int)inputDateTime.DayOfWeek;
            var newDate = inputDateTime.AddDays(numberOfDaysToEndOfWeek);
            //var endOfWeek=
            return new DateTime(newDate.Year, newDate.Month, newDate.Day, 23, 59, 59, 997);
        }
        public static DateTime GetDateTimeToTheLastOfTheGivenMonth(this DateTime inputDateTime)
        {
            var lastDayOfMonth = DateTime.DaysInMonth(inputDateTime.Year, inputDateTime.Month);
            return new DateTime(inputDateTime.Year, inputDateTime.Month, lastDayOfMonth, 23, 59, 59, 997);
        }
    }
}