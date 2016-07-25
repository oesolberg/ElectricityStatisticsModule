using System;
using System.Collections.Generic;
using System.Linq;
using ElectricityStatisticsLibrary.Database;
using ElectricityStatisticsLibrary.Statistics;

namespace ElectricityStatisticsLibrary
{
    public class StatisticsFacade
    {
        private DbFunctions _dbHandling;

        private HourlyStatistic _hourlyStatistic;
        private DailyStatistic _dailyStatistic;
        private WeeklyStatistic _weeklyStatistic;
        private MonthlyStatistic _monthlyStatistic;

        public StatisticsFacade()
        {
            _dbHandling=new Database.DbFunctions();
        }
        public void RunStatistics()
        {

            //Find start and end
            var foundStartDate = _dbHandling.GetStartTimeForElectricityData();
            var foundEndDate = _dbHandling.GetEndTimeForElectricityData();

            var createdStartDate = foundStartDate;
            //Start creating hours, days, months by going month by month
            while (createdStartDate < foundEndDate)
            {
                //Get end of date
                var endOfThisMonth = GetEndOfMonth(createdStartDate);
                //Get data for the periode
                var foundData=_dbHandling.GetDataForPeriode(createdStartDate, endOfThisMonth);
                //Do some shit with the data
                CreateStatisticsForData(foundData);
                //Create new startDate
                createdStartDate = GiveStartOfNextMonth(createdStartDate);
            }
        }

        private void CreateStatisticsForData(List<ElectricityData> foundData)
        {
            foreach (var electricityData in foundData)
            {
                CreateDataForHour(electricityData);
                CreateDataForDay(electricityData);
                CreateDataForWeek(electricityData);
                CreateDataForMonth(electricityData);
            }
            Console.WriteLine(foundData[0].FileCreatedDateTime.ToString("yyyy-MM-dd HH:mm:ss,fff"));
            Console.WriteLine(foundData.Last().FileCreatedDateTime.ToString("yyyy-MM-dd HH:mm:ss,fff"));
        }

        private void CreateDataForHour(ElectricityData foundData)
        {
            if (_hourlyStatistic == null)
            {
                _hourlyStatistic =new HourlyStatistic(foundData.FileCreatedDateTime,foundData.ElectricityValue);
                return;
            }
            var statisticsToSave = _hourlyStatistic.AddDateTimeAndKiloWattHoursUsed(foundData.FileCreatedDateTime, foundData.ElectricityValue);
            if (statisticsToSave == null || !statisticsToSave.Any()) return;

            SaveHourlyStatistics(statisticsToSave);

            //Få inn data. Sjekke om man har data fra før. Hvis ikke så er dette første data/start på timen
            //Hvis man har data fra før, tidspunktet er etter første tidspunkt, men innenfor samme time så settes dette som slutt
            //Hvis man har data fra før, tidspunktet er etter første tidspunkt, men en annen time så må man sjekke om hvor mange timer etter det man har dette er. 
            //Sjekke om endringen i forbruk er mer enn 1.
            //  Hvis det er neste time og antall minutter fra forrige er under 10 minutter så kan "forbruket" mellom disse punktene neglisjeres og man lagrer ned den nye timen og starter en ny en
            return;
        }

        private void SaveHourlyStatistics(List<HourlyStatistic> statisticsToSave)
        {
            
            foreach (var hourlyStatistic in statisticsToSave)
            {
                _dbHandling.SaveHourlyStatistic(hourlyStatistic);

            }
        }

        private void CreateDataForDay(ElectricityData foundData)
        {
            if (_dailyStatistic == null)
            {
                _dailyStatistic = new DailyStatistic(foundData.FileCreatedDateTime, foundData.ElectricityValue);
                return;
            }
            var statisticsToSave = _dailyStatistic.AddDateTimeAndKiloWattHoursUsed(foundData.FileCreatedDateTime, foundData.ElectricityValue);
            if (statisticsToSave == null || !statisticsToSave.Any()) return;

            SaveDailyStatistics(statisticsToSave);

            return;
        }

        private void SaveDailyStatistics(List<DailyStatistic> statisticsToSave)
        {
            foreach (var dailyStatistic in statisticsToSave)
            {
                _dbHandling.SaveDailyStatistic(dailyStatistic);

            }
        }

        private void CreateDataForWeek(ElectricityData foundData)
        {
            if (_weeklyStatistic == null)
            {
                _weeklyStatistic = new WeeklyStatistic(foundData.FileCreatedDateTime, foundData.ElectricityValue);
                return;
            }
            var statisticsToSave = _weeklyStatistic.AddDateTimeAndKiloWattHoursUsed(foundData.FileCreatedDateTime, foundData.ElectricityValue);
            if (statisticsToSave == null || !statisticsToSave.Any()) return;

            SaveWeeklyStatistics(statisticsToSave);

            return;
        }


        private void SaveWeeklyStatistics(List<WeeklyStatistic> statisticsToSave)
        {
            foreach (var weeklyStatistic in statisticsToSave)
            {
                _dbHandling.SaveWeeklyStatistic(weeklyStatistic);

            }
        }

        private void CreateDataForMonth(ElectricityData foundData)
        {
            if (_monthlyStatistic == null)
            {
                _monthlyStatistic = new MonthlyStatistic(foundData.FileCreatedDateTime, foundData.ElectricityValue);
                return;
            }
            var statisticsToSave = _monthlyStatistic.AddDateTimeAndKiloWattHoursUsed(foundData.FileCreatedDateTime, foundData.ElectricityValue);
            if (statisticsToSave == null || !statisticsToSave.Any()) return;

            SaveMonthlyStatistics(statisticsToSave);

            return;
        }

        private void SaveMonthlyStatistics(List<MonthlyStatistic> statisticsToSave)
        {
            foreach (var monthlyStatistic in statisticsToSave)
            {
                _dbHandling.SaveMonthlyStatistic(monthlyStatistic);

            }
        }
        private DateTime GetEndOfMonth(DateTime dateTime)
        {
            var daysInMonth = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
            return new DateTime(dateTime.Year,dateTime.Month,daysInMonth,23,59,59,997);
        }

        private DateTime GiveStartOfNextMonth(DateTime createdStartDate)
        {
            var basisDate = createdStartDate.AddMonths(1);
            return new DateTime(basisDate.Year,basisDate.Month,1);
        }
    }
}
