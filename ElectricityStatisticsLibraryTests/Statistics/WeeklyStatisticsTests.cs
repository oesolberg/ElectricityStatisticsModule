using System;
using ElectricityStatisticsLibrary.Statistics;
using NUnit.Framework;
using Shouldly;

namespace ElectricityStatisticsLibraryTests.Statistics
{
    public class WeeklyStatisticsTests
    {
        [Test]
        public void AddDateTimeAndKiloWattHoursUsed_Initialization_ShoudReturnNull()
        {
            //Arrange

            //Act
            var weeklyStats = new WeeklyStatistic(new DateTime(2000, 10, 1), 1000);
            //Assert
            weeklyStats.ShouldNotBe(null);
        }

        [Test]
        public void AddDateTimeAndKiloWattHoursUsed_AddTimeAndElectricityInSameWeek_ShoulReturnNull()
        {
            //Arrange
            var weeklyStats=new WeeklyStatistic(new DateTime(2016,7,25),1000 );
            //Act
            var result = weeklyStats.AddDateTimeAndKiloWattHoursUsed(new DateTime(2016, 7, 27, 0, 30, 1), 1001);
            //Assert
            result.ShouldBe(null);
        }

        [Test]
        public void AddDateTimeAndKiloWattHoursUsed_AddTimeAndElectricityInFollowingWeek_ShoulReturnOneWeeklyStatistic()
        {
            //Arrange
            var weeklyStats = new WeeklyStatistic(new DateTime(2000, 1, 3), 1000);
            //Act
            var result = weeklyStats.AddDateTimeAndKiloWattHoursUsed(new DateTime(2000, 1, 10, 1, 0, 1), 1002);
            //Assert
            result.ShouldNotBe(null);
            result.Count.ShouldBe(1);
            result[0].GetDateTimeForWeek().ShouldBe(new DateTime(2000,1,3,0,0,0));
            result[0].GetNumberOfKiloWattHoursUsed().ShouldBe(2);
        }

        [Test]
        public void AddDateTimeAndKiloWattHoursUsed_AddTimeAndElectricityInTwoWeeksAfterStart_ShoulReturnTwoWeeklyStatistics()
        {
            //Arrange
            var weeklyStats = new WeeklyStatistic(new DateTime(2016, 7, 25), 1000);
            //Act
            var result = weeklyStats.AddDateTimeAndKiloWattHoursUsed(new DateTime(2016, 8, 13, 2, 30, 1), 1004);
            //Assert
            result.ShouldNotBe(null);
            result.Count.ShouldBe(2);
            result[0].GetDateTimeForWeek().ShouldBe(new DateTime(2016, 7, 25));
            result[0].GetNumberOfKiloWattHoursUsed().ShouldBe(2);
            result[1].GetDateTimeForWeek().ShouldBe(new DateTime(2016, 8, 1));
            result[1].GetNumberOfKiloWattHoursUsed().ShouldBe(2);
        }

        [Test]
        public void AddDateTimeAndKiloWattHoursUsed_AddTimeAndElectricityTwoWeeksAfterStartSpanningNewYear_ShoulReturnTwoWeeklyStatistics()
        {
            //Arrange
            var weeklyStats = new WeeklyStatistic(new DateTime(2015, 12, 30,2,1,2), 1000);
            //Act
            var result = weeklyStats.AddDateTimeAndKiloWattHoursUsed(new DateTime(2016, 1, 13, 1, 30, 1), 1004);
            //Assert
            result.ShouldNotBe(null);
            result.Count.ShouldBe(2);
            result[0].GetDateTimeForWeek().ShouldBe(new DateTime(2015, 12, 28));
            result[0].GetNumberOfKiloWattHoursUsed().ShouldBe(2);
            result[1].GetDateTimeForWeek().ShouldBe(new DateTime(2016, 1, 4));
            result[1].GetNumberOfKiloWattHoursUsed().ShouldBe(2);
        }
    }
}