using System;
using System.Collections.Generic;

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


            //If no end hour and input hour in same hour as start hour or input hour same hour as current end hour
            //if (!_endOfHourDateTime.HasValue && _startOfHourDateTime.Hour == inputDateTime.Hour && _startOfHourDateTime.Date == inputDateTime.Date ||
            //    (_endOfHourDateTime.HasValue && _endOfHourDateTime.Value.Hour == inputDateTime.Hour && _endOfHourDateTime.Value.Month == inputDateTime.Month))
            //{
            //    _endOfHourDateTime = inputDateTime;
            //    _endNumberOfKiloWattsUsed = numberOfKiloWattHoursUsed;
            //    return null;
            //}


            //if (_endOfHourDateTime.HasValue && (_endOfHourDateTime.Value.Hour + 1 == inputDateTime.Hour) && _endOfHourDateTime.Value.Date <= inputDateTime.Date)
            //{
            //    //Produce new hour to store and update this to the new hour.
            //    var hourlyStatisticsToStore = new HourlyStatistic(this._startOfHourDateTime, this._startNumberOfKiloWattsUsed, this._endOfHourDateTime.Value, this._endNumberOfKiloWattsUsed.Value);
            //    //Update this to the new numbers
            //    ResetThisHour(inputDateTime, numberOfKiloWattHoursUsed);
            //    return new List<HourlyStatistic>() { hourlyStatisticsToStore };
            //}
            //if (!_endOfHourDateTime.HasValue && (_startOfHourDateTime < inputDateTime))
            //{
            //    var hourlyStatisticsToStore = new HourlyStatistic(this._startOfHourDateTime, this._startNumberOfKiloWattsUsed, new DateTime(this._startOfHourDateTime.Year, this._startOfHourDateTime.Month, this._startOfHourDateTime.Day, this._startOfHourDateTime.Hour, 59, 59), numberOfKiloWattHoursUsed);
            //    //Update this to the new numbers
            //    ResetThisHour(inputDateTime, numberOfKiloWattHoursUsed);
            //    return new List<HourlyStatistic>() { hourlyStatisticsToStore };

            //}
            ////if end hour but input datetime has next hour and diff between current endhour and input is less than 15 minutes
            //if (_endOfHourDateTime.Value.Hour + 1 >= inputDateTime.Hour)
            //{
            //    var listOfStatistics = new List<HourlyStatistic>();
            //    //More than 1 hour in difference. Most possibly hours in difference.
            //    var totalNumbersOfHoursInDifference = (inputDateTime - _endOfHourDateTime.Value).TotalHours;
            //    //Do we keep the current number for the hour that we are in? If we have values for more than 30 minutes use what we have
            //    if ((_endOfHourDateTime.Value - _startOfHourDateTime).TotalMinutes > 30)
            //    {
            //        listOfStatistics.Add(CreateHourStatisticsFromThis(this));
            //    }
            //    else
            //    {
            //        inputDateTime = _startOfHourDateTime;

            //    }
            //    //Used electricity
            //    var numberOfKiloWattHoursUsedForPeriode = numberOfKiloWattHoursUsed - _endNumberOfKiloWattsUsed.Value;
            //    //find the number of hours to dived the used electricty
            //    var numberOfHours = (inputDateTime - _endOfHourDateTime.Value).TotalHours;
            //    var numberOfKilowattHoursPerHour = (int)(numberOfKiloWattHoursUsedForPeriode / numberOfHours);
            //    for (int i = 0; i < numberOfHours; i++)
            //    {
            //        //listOfStatistics.Add(CreateHourStatisticsForPeriodePart(inputDateTime, i, numberOfKilowattHoursPerHour,));
            //    }
            //    return listOfStatistics;
            //}
            //return null;
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
                var numberOfHoursBetweenCurrentHourAndInputDate =(int) (inputDateTime - _startOfHourDateTime).TotalHours;

                var kiloWattHoursPerHour = ((numberOfKiloWattHoursUsed - _startNumberOfKiloWattsUsed)/numberOfHoursBetweenCurrentHourAndInputDate);

                for (int i = 0; i < numberOfHoursBetweenCurrentHourAndInputDate ; i++)
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

        private int GetLatestKiloWattHour()
        {
            if (_endNumberOfKiloWattsUsed.HasValue) return _endNumberOfKiloWattsUsed.Value;
            return _startNumberOfKiloWattsUsed;
        }

        private DateTime GetLastestDate()
        {
            if (_endOfHourDateTime.HasValue) return _endOfHourDateTime.Value;
            return _startOfHourDateTime;
        }

        private void SetEndOfHourIfEmpty(int numberOfKiloWattHoursUsed)
        {
            if (!_endOfHourDateTime.HasValue)
            {
                this._endOfHourDateTime = _startOfHourDateTime.GetDateTimeToTheLastOfTheGivenHour();
                this._endNumberOfKiloWattsUsed = numberOfKiloWattHoursUsed;
            }
        }

        private HourlyStatistic CreateHourStatisticsForPeriodePart(DateTime inputDateTime, int hoursToAdd, int numberOfKilowattHoursUsed)
        {
            return null;// new HourlyStatistic() {};
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