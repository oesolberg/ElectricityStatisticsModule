using System;
using System.Collections.Generic;
using System.Globalization;

namespace ElectricityStatisticsLibrary.Statistics
{
    public class WeeklyStatistic
    {
        private DateTime _startOfWeekDateTime;
        private DateTime? _endOfWeekDateTime;
        private readonly double _numberOfKiloWattHoursUsed = 0;
        private int _startNumberOfKiloWattsUsed;
        private int? _endNumberOfKiloWattsUsed;

        public WeeklyStatistic(DateTime startOfWeekDateTime, int numberOfKiloWattHoursUsed)
        {
            _startOfWeekDateTime = startOfWeekDateTime;
            _startNumberOfKiloWattsUsed = numberOfKiloWattHoursUsed;
        }

        public WeeklyStatistic(DateTime startOfWeekDateTime, int startNumberOfKiloWattHoursUsed, DateTime endOfWeekDateTime, int endNumberOfKiloWattHoursUsed)
        {
            _startOfWeekDateTime = startOfWeekDateTime;
            _startNumberOfKiloWattsUsed = startNumberOfKiloWattHoursUsed;
            _endOfWeekDateTime = endOfWeekDateTime;
            _endNumberOfKiloWattsUsed = endNumberOfKiloWattHoursUsed;
            _numberOfKiloWattHoursUsed = (_endNumberOfKiloWattsUsed.Value - _startNumberOfKiloWattsUsed);
        }

        public WeeklyStatistic(DateTime startDateTime, DateTime endDateTime, double startNumberOfKilowattsUsed, double endNumberOfKilowattsUsed, double kiloWattHoursPerHour)
        {
            _startOfWeekDateTime = startDateTime;
            _endOfWeekDateTime = endDateTime;
            _startNumberOfKiloWattsUsed = (int)startNumberOfKilowattsUsed;
            _endNumberOfKiloWattsUsed = (int)endNumberOfKilowattsUsed;
            _numberOfKiloWattHoursUsed = kiloWattHoursPerHour;
        }

        public DateTime DateTimeStartOfWeek => _startOfWeekDateTime;

        public DateTime? DateTimeEndOfWeek => _endOfWeekDateTime;

        public int WeekNumber =>GetWeekOfYear(_startOfWeekDateTime);

        public double KWhNumberUsed => _numberOfKiloWattHoursUsed;

        public int StartNumberOfKiloWattsUsed => _startNumberOfKiloWattsUsed;

        public int? EndNumberOfKiloWattsUsed => _endNumberOfKiloWattsUsed;

        public DateTime CreatedDateTime => DateTime.Now;

        public double GetNumberOfKiloWattHoursUsed()
        {
            return _numberOfKiloWattHoursUsed;
        }

        public DateTime GetDateTimeForWeek()
        {
            return _startOfWeekDateTime.GetDateTimeForTheFirstDayOfTheGivenWeek();
        }
        public List<WeeklyStatistic> AddDateTimeAndKiloWattHoursUsed(DateTime inputDateTime, int numberOfKiloWattHoursUsed)
        {
            var startOfCurrentWeek = _startOfWeekDateTime.GetDateTimeForTheFirstDayOfTheGivenWeek();
            var endOfCurrentWeek = _startOfWeekDateTime.GetDateTimeForTheLastDayOfTheGivenWeek();
            if (_startOfWeekDateTime < inputDateTime  && inputDateTime>=startOfCurrentWeek && inputDateTime<=endOfCurrentWeek)
            {
                AddEndOfWeekWithinSameWeekAsStart(inputDateTime, numberOfKiloWattHoursUsed);
            }
            else
            {
                return AddEndOfWeekAtDifferentTimeThanCurrentWeek(inputDateTime, numberOfKiloWattHoursUsed);
            }
            return null;


        }

        private void AddEndOfWeekWithinSameWeekAsStart(DateTime inputDateTime, int numberOfKiloWattHoursUsed)
        {
            _endOfWeekDateTime = inputDateTime;
            _endNumberOfKiloWattsUsed = numberOfKiloWattHoursUsed;
        }

        private List<WeeklyStatistic> AddEndOfWeekAtDifferentTimeThanCurrentWeek(DateTime inputDateTime, int numberOfKiloWattHoursUsed)
        {
            var listToReturn = new List<WeeklyStatistic>();
            //Best case: new datetime is the following week
            var endOfNextWeek = _startOfWeekDateTime.GetDateTimeForTheLastDayOfTheGivenWeek().AddWeeks(1);
            if ( inputDateTime<= endOfNextWeek)
            {
                //Create stats for the previous week and set the new hour
                SetEndOfWeekIfEmpty(numberOfKiloWattHoursUsed);
                var weeklyStatisticToReturn = CreateWeekStatisticsFromThis(this);
                listToReturn.Add(weeklyStatisticToReturn);
                ResetThisWeek(inputDateTime, numberOfKiloWattHoursUsed);
            }
            else
            {
                //Worst case: new datetime is later than the following hour
                var numberOfWeeksBetweenCurrentWeekAndInputDate =GetNumberOfWeeksInDifference(_startOfWeekDateTime, inputDateTime);

                var kiloWattHoursPerWeek = ((numberOfKiloWattHoursUsed - _startNumberOfKiloWattsUsed) / numberOfWeeksBetweenCurrentWeekAndInputDate);

                for (int i = 0; i < numberOfWeeksBetweenCurrentWeekAndInputDate; i++)
                {
                    listToReturn.Add(CreateWeekStatisticsForWeek(_startOfWeekDateTime, i, _startNumberOfKiloWattsUsed, kiloWattHoursPerWeek));
                }


                ResetThisWeek(inputDateTime, numberOfKiloWattHoursUsed);

            }
            return listToReturn;
        }

        private int GetNumberOfWeeksInDifference(DateTime startOfWeekDateTime, DateTime inputDateTime)
        {
            var startWeek = GetWeekOfYear(startOfWeekDateTime);
            var endWeek = GetWeekOfYear(inputDateTime);
            if (startWeek <= endWeek) return endWeek - startWeek;
            //What if we cross new year? (dont care about data compared having more than a year in difference
            var lastWeekOfYear = GetWeekOfYear(new DateTime(startOfWeekDateTime.Year, 12, 31));
            return ((lastWeekOfYear - startWeek) + endWeek);
        }

        private WeeklyStatistic CreateWeekStatisticsForWeek(DateTime startOfHourDateTime, int numberOfHours, int startNumberOfKiloWattsUsed, double kiloWattHoursPerHour)
        {
            var startDateTime = startOfHourDateTime.AddWeeks(numberOfHours);
            var endDateTime = startOfHourDateTime.AddWeeks(numberOfHours).GetDateTimeForTheLastDayOfTheGivenWeek();
            var startNumberOfKilowattsUsed = startNumberOfKiloWattsUsed + (numberOfHours * kiloWattHoursPerHour);
            var endNumberOfKilowattsUsed = startNumberOfKilowattsUsed + ((kiloWattHoursPerHour) * (numberOfHours + 1));
            return new WeeklyStatistic(startDateTime, endDateTime, startNumberOfKilowattsUsed, endNumberOfKilowattsUsed, kiloWattHoursPerHour);


        }

        private int GetLatestKiloWattHour()
        {
            if (_endNumberOfKiloWattsUsed.HasValue) return _endNumberOfKiloWattsUsed.Value;
            return _startNumberOfKiloWattsUsed;
        }

        private DateTime GetLastestDate()
        {
            if (_endOfWeekDateTime.HasValue) return _endOfWeekDateTime.Value;
            return _startOfWeekDateTime;
        }

        private void SetEndOfWeekIfEmpty(int numberOfKiloWattHoursUsed)
        {
            if (!_endOfWeekDateTime.HasValue)
            {
                this._endOfWeekDateTime = _startOfWeekDateTime.GetDateTimeForTheLastDayOfTheGivenWeek();
                this._endNumberOfKiloWattsUsed = numberOfKiloWattHoursUsed;
            }
        }

        private HourlyStatistic CreateHourStatisticsForPeriodePart(DateTime inputDateTime, int hoursToAdd, int numberOfKilowattHoursUsed)
        {
            return null;// new HourlyStatistic() {};
        }

        private WeeklyStatistic CreateWeekStatisticsFromThis(WeeklyStatistic weeklyStatistic)
        {
            return new WeeklyStatistic(weeklyStatistic._startOfWeekDateTime, weeklyStatistic._startNumberOfKiloWattsUsed, weeklyStatistic._endOfWeekDateTime.Value, weeklyStatistic._endNumberOfKiloWattsUsed.Value);
        }

        private int GetWeekOfYear(DateTime inputDate)
        {
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            Calendar cal = dfi.Calendar;
            return cal.GetWeekOfYear(inputDate, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        private void ResetThisWeek(DateTime inputDateTime, int numberOfKiloWattHoursUsed)
        {
            _startOfWeekDateTime = inputDateTime;
            _startNumberOfKiloWattsUsed = numberOfKiloWattHoursUsed;
            _endOfWeekDateTime = null;
            _endNumberOfKiloWattsUsed = null;
        }
    }
}