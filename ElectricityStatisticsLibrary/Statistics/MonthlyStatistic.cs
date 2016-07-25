using System;
using System.Collections.Generic;

namespace ElectricityStatisticsLibrary.Statistics
{
    public class MonthlyStatistic
    {
        private DateTime _startOfMonthDateTime;
        private DateTime? _endOfMonthDateTime;
        private readonly double _numberOfKiloWattHoursUsed = 0;
        private int _startNumberOfKiloWattsUsed;
        private int? _endNumberOfKiloWattsUsed;

        public MonthlyStatistic(DateTime startOfMonthDateTime, int numberOfKiloWattHoursUsed)
        {
            _startOfMonthDateTime = startOfMonthDateTime;
            _startNumberOfKiloWattsUsed = numberOfKiloWattHoursUsed;
        }

        public MonthlyStatistic(DateTime startOfMonthDateTime, int startNumberOfKiloWattHoursUsed, DateTime endOfMonthDateTime, int endNumberOfKiloWattHoursUsed)
        {
            _startOfMonthDateTime = startOfMonthDateTime;
            _startNumberOfKiloWattsUsed = startNumberOfKiloWattHoursUsed;
            _endOfMonthDateTime = endOfMonthDateTime;
            _endNumberOfKiloWattsUsed = endNumberOfKiloWattHoursUsed;
            _numberOfKiloWattHoursUsed = (_endNumberOfKiloWattsUsed.Value - _startNumberOfKiloWattsUsed);
        }

        public MonthlyStatistic(DateTime startDateTime, DateTime endDateTime, double startNumberOfKilowattsUsed, double endNumberOfKilowattsUsed, double kiloWattHoursPerHour)
        {
            _startOfMonthDateTime = startDateTime;
            _endOfMonthDateTime = endDateTime;
            _startNumberOfKiloWattsUsed = (int)startNumberOfKilowattsUsed;
            _endNumberOfKiloWattsUsed = (int)endNumberOfKilowattsUsed;
            _numberOfKiloWattHoursUsed = kiloWattHoursPerHour;
        }

        public DateTime DateTimeStartOfMonth => _startOfMonthDateTime;

        public DateTime? DateTimeEndOfMonth => _endOfMonthDateTime;

        public int MonthNumber => _startOfMonthDateTime.Month;

        public double KWhNumberUsed => _numberOfKiloWattHoursUsed;

        public int StartNumberOfKiloWattsUsed => _startNumberOfKiloWattsUsed;

        public int? EndNumberOfKiloWattsUsed => _endNumberOfKiloWattsUsed;

        public DateTime CreatedDateTime => DateTime.Now;

        public double GetNumberOfKiloWattHoursUsed()
        {
            return _numberOfKiloWattHoursUsed;
        }

        public DateTime GetDateTimeForMonth()
        {
            return _startOfMonthDateTime.GetDateTimeForTheFirstDayOfTheGivenMonth();
        }

        public List<MonthlyStatistic> AddDateTimeAndKiloWattHoursUsed(DateTime inputDateTime, int numberOfKiloWattHoursUsed)
        {
            if (_startOfMonthDateTime.Date.Month == inputDateTime.Date.Month && _startOfMonthDateTime.Date.Year == inputDateTime.Date.Year)
            {
                AddDateTimeWithinSameMonthAsStart(inputDateTime, numberOfKiloWattHoursUsed);
            }
            else
            {
                return AddDateTimeAtDifferentMonthThanStartOfMonth(inputDateTime, numberOfKiloWattHoursUsed);
            }
            return null;



        }

        private void AddDateTimeWithinSameMonthAsStart(DateTime inputDateTime, int numberOfKiloWattHoursUsed)
        {
            _endOfMonthDateTime = inputDateTime;
            _endNumberOfKiloWattsUsed = numberOfKiloWattHoursUsed;
        }

        private List<MonthlyStatistic> AddDateTimeAtDifferentMonthThanStartOfMonth(DateTime inputDateTime, int numberOfKiloWattHoursUsed)
        {
            var listToReturn = new List<MonthlyStatistic>();
            //Best case: new datetime is the following hour
            if (_startOfMonthDateTime.AddMonths(1).Month == inputDateTime.Month && _startOfMonthDateTime.AddMonths(1).Year == inputDateTime.Year)
            {
                //Create stats for the previous hour and set the new hour
                SetEndOfMonthIfEmpty(numberOfKiloWattHoursUsed);
                var hourlyStatisticToReturn = CreateHourStatisticsFromThis(this);
                listToReturn.Add(hourlyStatisticToReturn);
                ResetThisHour(inputDateTime, numberOfKiloWattHoursUsed);
            }
            else
            {
                //Worst case: new datetime is later than the following hour
                var numberOfMonthsBetweenCurrentMonthAndInputDate = GetTotalMonths(_startOfMonthDateTime, inputDateTime);

                var kiloWattHoursPerHour = ((numberOfKiloWattHoursUsed - _startNumberOfKiloWattsUsed) / numberOfMonthsBetweenCurrentMonthAndInputDate);

                for (int i = 0; i < numberOfMonthsBetweenCurrentMonthAndInputDate; i++)
                {
                    listToReturn.Add(CreateMonthStatisticsForMonth(_startOfMonthDateTime, i, _startNumberOfKiloWattsUsed, kiloWattHoursPerHour));
                }


                ResetThisHour(inputDateTime, numberOfKiloWattHoursUsed);

            }
            return listToReturn;
        }

        private int GetTotalMonths(DateTime startDateTime, DateTime inputDateTime)
        {
            if (startDateTime.Year == inputDateTime.Year && startDateTime.Month == inputDateTime.Month) return 0;
            //find number of years in difference
            var numberOfMonthsInDifference = 0;
            if (startDateTime.Year < inputDateTime.Year)
            {
                numberOfMonthsInDifference = (inputDateTime.Year - startDateTime.Year) * 12;
            }
            if (startDateTime.Month < inputDateTime.Month)
            {
                numberOfMonthsInDifference = (inputDateTime.Month - startDateTime.Month);
            }
            else if (startDateTime.Month > inputDateTime.Month)
            {
                numberOfMonthsInDifference = numberOfMonthsInDifference - (startDateTime.Month - inputDateTime.Month);
            }
            return numberOfMonthsInDifference;
        }

        private MonthlyStatistic CreateMonthStatisticsForMonth(DateTime startMonthDateTime, int numberOfMonths, int startNumberOfKiloWattsUsed, double kiloWattHoursPerHour)
        {
            var startDateTime = startMonthDateTime.GetDateTimeForTheFirstDayOfTheGivenMonth().AddMonths(numberOfMonths);
            var endDateTime = startMonthDateTime.AddMonths(numberOfMonths).GetDateTimeToTheLastOfTheGivenMonth();
            var startNumberOfKilowattsUsed = startNumberOfKiloWattsUsed + (numberOfMonths * kiloWattHoursPerHour);
            var endNumberOfKilowattsUsed = startNumberOfKilowattsUsed + ((kiloWattHoursPerHour) * (numberOfMonths + 1));
            return new MonthlyStatistic(startDateTime, endDateTime, startNumberOfKilowattsUsed, endNumberOfKilowattsUsed, kiloWattHoursPerHour);
        }


        private void SetEndOfMonthIfEmpty(int numberOfKiloWattHoursUsed)
        {
            if (!_endOfMonthDateTime.HasValue)
            {
                this._endOfMonthDateTime = _startOfMonthDateTime.GetDateTimeToTheLastOfTheGivenMonth();
                this._endNumberOfKiloWattsUsed = numberOfKiloWattHoursUsed;
            }
        }

        private HourlyStatistic CreateHourStatisticsForPeriodePart(DateTime inputDateTime, int hoursToAdd, int numberOfKilowattHoursUsed)
        {
            return null;// new HourlyStatistic() {};
        }

        private MonthlyStatistic CreateHourStatisticsFromThis(MonthlyStatistic monthlyStatistic)
        {
            return new MonthlyStatistic(monthlyStatistic._startOfMonthDateTime, monthlyStatistic._startNumberOfKiloWattsUsed, monthlyStatistic._endOfMonthDateTime.Value, monthlyStatistic._endNumberOfKiloWattsUsed.Value);
        }

        private void ResetThisHour(DateTime inputDateTime, int numberOfKiloWattHoursUsed)
        {
            _startOfMonthDateTime = inputDateTime;
            _startNumberOfKiloWattsUsed = numberOfKiloWattHoursUsed;
            _endOfMonthDateTime = null;
            _endNumberOfKiloWattsUsed = null;
        }
    }
}