using System;
using System.Collections.Generic;
using System.Globalization;

namespace ElectricityStatisticsLibrary.Statistics
{
    public class HourlyStatistic
    {
        private DateTime _startOfHourDateTime;
        private DateTime? _endOfHourDateTime;
        private readonly double _numberOfKiloWattHoursUsed = 0;
        private int _startNumberOfKiloWattsUsed;
        private int? _endNumberOfKiloWattsUsed;

        public HourlyStatistic(DateTime startOfHourDateTime, int numberOfKiloWattHoursUsed)
        {
            _startOfHourDateTime = startOfHourDateTime;
            _startNumberOfKiloWattsUsed = numberOfKiloWattHoursUsed;
        }

        public HourlyStatistic(DateTime startOfHourDateTime, int startNumberOfKiloWattHoursUsed, DateTime endOfHourDateTime, int endNumberOfKiloWattHoursUsed)
        {
            _startOfHourDateTime = startOfHourDateTime;
            _startNumberOfKiloWattsUsed = startNumberOfKiloWattHoursUsed;
            _endOfHourDateTime = endOfHourDateTime;
            _endNumberOfKiloWattsUsed = endNumberOfKiloWattHoursUsed;
            _numberOfKiloWattHoursUsed = (_endNumberOfKiloWattsUsed.Value - _startNumberOfKiloWattsUsed);
        }

        public HourlyStatistic(DateTime startDateTime, DateTime endDateTime, double startNumberOfKilowattsUsed, double endNumberOfKilowattsUsed, double kiloWattHoursPerHour)
        {
            _startOfHourDateTime = startDateTime;
            _endOfHourDateTime = endDateTime;
            _startNumberOfKiloWattsUsed = (int)startNumberOfKilowattsUsed;
            _endNumberOfKiloWattsUsed = (int)endNumberOfKilowattsUsed;
            _numberOfKiloWattHoursUsed = kiloWattHoursPerHour;
        }

        public DateTime DateTimeStartOfHour => _startOfHourDateTime;

        public DateTime? DateTimeEndOfHour => _endOfHourDateTime;

        public int HourNumber => _startOfHourDateTime.Hour;

        public double KWhNumberUsed => _numberOfKiloWattHoursUsed;

        public int StartNumberOfKiloWattsUsed => _startNumberOfKiloWattsUsed;

        public int? EndNumberOfKiloWattsUsed => _endNumberOfKiloWattsUsed;

        public DateTime CreatedDateTime => DateTime.Now;

        public double GetNumberOfKiloWattHoursUsed()
        {
            return _numberOfKiloWattHoursUsed;
        }

        public DateTime GetDateTimeForHour()
        {
            return new DateTime(_startOfHourDateTime.Year, _startOfHourDateTime.Month, _startOfHourDateTime.Day, _startOfHourDateTime.Hour, 0, 0);
        }



        public List<HourlyStatistic> AddDateTimeAndKiloWattHoursUsed(DateTime inputDateTime, int numberOfKiloWattHoursUsed)
        {
            if (_startOfHourDateTime.Date == inputDateTime.Date && _startOfHourDateTime.Hour == inputDateTime.Hour)
            {
                AddEndHourWithinSameHourAsStart(inputDateTime, numberOfKiloWattHoursUsed);
            }
            else
            {
                return AddEndHourAtDifferentTimeThanStartOfHour(inputDateTime, numberOfKiloWattHoursUsed);
            }
            return null;

        }

        private void AddEndHourWithinSameHourAsStart(DateTime inputDateTime, int numberOfKiloWattHoursUsed)
        {
            _endOfHourDateTime = inputDateTime;
            _endNumberOfKiloWattsUsed = numberOfKiloWattHoursUsed;
        }

        private List<HourlyStatistic> AddEndHourAtDifferentTimeThanStartOfHour(DateTime inputDateTime, int numberOfKiloWattHoursUsed)
        {
            var listToReturn = new List<HourlyStatistic>();
            //Best case: new datetime is the following hour
            if (_startOfHourDateTime.AddHours(1).Date == inputDateTime.Date && _startOfHourDateTime.AddHours(1).Hour == inputDateTime.Hour)
            {
                //Create stats for the previous hour and set the new hour
                SetEndOfHourIfEmpty(numberOfKiloWattHoursUsed);
                var hourlyStatisticToReturn = CreateHourStatisticsFromThis(this);
                listToReturn.Add(hourlyStatisticToReturn);
                ResetThisHour(inputDateTime, numberOfKiloWattHoursUsed);
            }
            else
            {
                //Worst case: new datetime is later than the following hour
                var numberOfHoursBetweenCurrentHourAndInputDate = (int)(inputDateTime - _startOfHourDateTime).TotalHours;

                var kiloWattHoursPerHour = ((numberOfKiloWattHoursUsed - _startNumberOfKiloWattsUsed) / numberOfHoursBetweenCurrentHourAndInputDate);

                for (int i = 0; i < numberOfHoursBetweenCurrentHourAndInputDate; i++)
                {
                    listToReturn.Add(CreateHourStatisticsForHour(_startOfHourDateTime, i, _startNumberOfKiloWattsUsed, kiloWattHoursPerHour));
                }


                ResetThisHour(inputDateTime, numberOfKiloWattHoursUsed);

            }
            return listToReturn;
        }

        private HourlyStatistic CreateHourStatisticsForHour(DateTime startOfHourDateTime, int numberOfHours, int startNumberOfKiloWattsUsed, double kiloWattHoursPerHour)
        {
            var startDateTime = startOfHourDateTime.AddHours(numberOfHours);
            var endDateTime = startOfHourDateTime.AddHours(numberOfHours).GetDateTimeToTheLastOfTheGivenHour();
            var startNumberOfKilowattsUsed = startNumberOfKiloWattsUsed + (numberOfHours * kiloWattHoursPerHour);
            var endNumberOfKilowattsUsed = startNumberOfKilowattsUsed + ((kiloWattHoursPerHour) * (numberOfHours + 1));
            return new HourlyStatistic(startDateTime, endDateTime, startNumberOfKilowattsUsed, endNumberOfKilowattsUsed, kiloWattHoursPerHour);


        }

        private void SetEndOfHourIfEmpty(int numberOfKiloWattHoursUsed)
        {
            if (!_endOfHourDateTime.HasValue)
            {
                this._endOfHourDateTime = _startOfHourDateTime.GetDateTimeToTheLastOfTheGivenHour();
                this._endNumberOfKiloWattsUsed = numberOfKiloWattHoursUsed;
            }
        }


        private HourlyStatistic CreateHourStatisticsFromThis(HourlyStatistic hourlyStatistic)
        {
            return new HourlyStatistic(hourlyStatistic._startOfHourDateTime, hourlyStatistic._startNumberOfKiloWattsUsed, hourlyStatistic._endOfHourDateTime.Value, hourlyStatistic._endNumberOfKiloWattsUsed.Value);
        }

        private void ResetThisHour(DateTime inputDateTime, int numberOfKiloWattHoursUsed)
        {
            _startOfHourDateTime = inputDateTime;
            _startNumberOfKiloWattsUsed = numberOfKiloWattHoursUsed;
            _endOfHourDateTime = null;
            _endNumberOfKiloWattsUsed = null;
        }
    }
}