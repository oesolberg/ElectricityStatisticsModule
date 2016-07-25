using System;
using System.Collections.Generic;

namespace ElectricityStatisticsLibrary.Statistics
{
    public class DailyStatistic
    {
        private DateTime _startOfDayDateTime;
        private DateTime? _endOfDayDateTime;
        private readonly double _numberOfKiloWattHoursUsed = 0;
        private int _startNumberOfKiloWattsUsed;
        private int? _endNumberOfKiloWattsUsed;
        
        public DailyStatistic(DateTime startOfDayDateTime, int numberOfKiloWattHoursUsed)
        {
            _startOfDayDateTime = startOfDayDateTime;
            _startNumberOfKiloWattsUsed = numberOfKiloWattHoursUsed;
        }

        public DailyStatistic(DateTime startOfDayDateTime, int startNumberOfKiloWattHoursUsed, DateTime endOfDayDateTime, int endNumberOfKiloWattHoursUsed)
        {
            _startOfDayDateTime = startOfDayDateTime;
            _startNumberOfKiloWattsUsed = startNumberOfKiloWattHoursUsed;
            _endOfDayDateTime = endOfDayDateTime;
            _endNumberOfKiloWattsUsed = endNumberOfKiloWattHoursUsed;
            _numberOfKiloWattHoursUsed = (_endNumberOfKiloWattsUsed.Value - _startNumberOfKiloWattsUsed);
        }

        public DailyStatistic(DateTime startDateTime, DateTime endDateTime, double startNumberOfKilowattsUsed, double endNumberOfKilowattsUsed, double kiloWattHoursPerHour)
        {
            _startOfDayDateTime = startDateTime;
            _endOfDayDateTime = endDateTime;
            _startNumberOfKiloWattsUsed = (int)startNumberOfKilowattsUsed;
            _endNumberOfKiloWattsUsed = (int)endNumberOfKilowattsUsed;
            _numberOfKiloWattHoursUsed = kiloWattHoursPerHour;
        }


        

        public DateTime DateTimeStartOfDay => _startOfDayDateTime;

        public DateTime? DateTimeEndOfDay => _endOfDayDateTime;

        public int DayNumber => _startOfDayDateTime.Day;

        public double KWhNumberUsed => _numberOfKiloWattHoursUsed;

        public int StartNumberOfKiloWattsUsed => _startNumberOfKiloWattsUsed;

        public int? EndNumberOfKiloWattsUsed => _endNumberOfKiloWattsUsed;

        public DateTime CreatedDateTime => DateTime.Now;

        public double GetNumberOfKiloWattHoursUsed()
        {
            return _numberOfKiloWattHoursUsed;
        }

        public DateTime GetDateTimeForDay()
        {
            return new DateTime(_startOfDayDateTime.Year, _startOfDayDateTime.Month, _startOfDayDateTime.Day, 0, 0, 0);
        }
        public List<DailyStatistic> AddDateTimeAndKiloWattHoursUsed(DateTime inputDateTime, int numberOfKiloWattHoursUsed)
        {
            if (_startOfDayDateTime.Date == inputDateTime.Date )
            {
                AddEndOfDayWithinSameDayAsStart(inputDateTime, numberOfKiloWattHoursUsed);
            }
            else
            {
                return AddEndDayAtDifferentDayThanStartOfDay(inputDateTime, numberOfKiloWattHoursUsed);
            }
            return null;
            
        }

        private void AddEndOfDayWithinSameDayAsStart(DateTime inputDateTime, int numberOfKiloWattHoursUsed)
        {
            _endOfDayDateTime = inputDateTime;
            _endNumberOfKiloWattsUsed = numberOfKiloWattHoursUsed;
        }

        private List<DailyStatistic> AddEndDayAtDifferentDayThanStartOfDay(DateTime inputDateTime, int numberOfKiloWattHoursUsed)
        {
            var listToReturn = new List<DailyStatistic>();
            //Best case: new datetime is the following day
            if (_startOfDayDateTime.AddDays(1).Date == inputDateTime.Date )
            {
                //Create stats for the previous hour and set the new hour
                SetEndOfDayIfEmpty(numberOfKiloWattHoursUsed);
                var dailyStatisticToReturn = CreateDayStatisticsFromThis(this);
                listToReturn.Add(dailyStatisticToReturn);
                ResetThisDay(inputDateTime, numberOfKiloWattHoursUsed);
            }
            else
            {
                //Worst case: new datetime is later than the following hour
                var numberOfDaysBetweenCurrentDayAndInputDate =(int) (inputDateTime - _startOfDayDateTime).TotalDays;

                var kiloWattHoursPerDay = ((numberOfKiloWattHoursUsed - _startNumberOfKiloWattsUsed)/numberOfDaysBetweenCurrentDayAndInputDate);

                for (int i = 0; i < numberOfDaysBetweenCurrentDayAndInputDate ; i++)
                {
                    listToReturn.Add(CreateDayStatisticsForDay(_startOfDayDateTime, i, _startNumberOfKiloWattsUsed, kiloWattHoursPerDay));
                }


                ResetThisDay(inputDateTime, numberOfKiloWattHoursUsed);

            }
            return listToReturn;
        }

        private DailyStatistic CreateDayStatisticsForDay(DateTime startOfHourDateTime, int numberOfHours, int startNumberOfKiloWattsUsed, double kiloWattHoursPerHour)
        {
            var startDateTime = startOfHourDateTime.AddDays(numberOfHours);
            var endDateTime = startOfHourDateTime.AddDays(numberOfHours).GetDateTimeToTheLastOfTheGivenDay();
            var startNumberOfKilowattsUsed = startNumberOfKiloWattsUsed + (numberOfHours * kiloWattHoursPerHour);
            var endNumberOfKilowattsUsed = startNumberOfKilowattsUsed + ((kiloWattHoursPerHour) * (numberOfHours + 1));
            return new DailyStatistic(startDateTime, endDateTime, startNumberOfKilowattsUsed, endNumberOfKilowattsUsed, kiloWattHoursPerHour);


        }

        
        private void SetEndOfDayIfEmpty(int numberOfKiloWattHoursUsed)
        {
            if (!_endOfDayDateTime.HasValue)
            {
                this._endOfDayDateTime = _startOfDayDateTime.GetDateTimeToTheLastOfTheGivenHour();
                this._endNumberOfKiloWattsUsed = numberOfKiloWattHoursUsed;
            }
        }

        private DailyStatistic CreateDayStatisticsFromThis(DailyStatistic dailyStatistic)
        {
            return new DailyStatistic(dailyStatistic._startOfDayDateTime, dailyStatistic._startNumberOfKiloWattsUsed, dailyStatistic._endOfDayDateTime.Value, dailyStatistic._endNumberOfKiloWattsUsed.Value);
        }

        private void ResetThisDay(DateTime inputDateTime, int numberOfKiloWattHoursUsed)
        {
            _startOfDayDateTime = inputDateTime;
            _startNumberOfKiloWattsUsed = numberOfKiloWattHoursUsed;
            _endOfDayDateTime = null;
            _endNumberOfKiloWattsUsed = null;
        }
    }
}