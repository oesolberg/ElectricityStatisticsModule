using System;
using ElectricityStatisticsLibrary.Statistics;
using NUnit.Framework;
using Shouldly;

namespace ElectricityStatisticsLibraryTests.Statistics
{
    public class DailyStatisticsTests
    {
        [Test]
        public void AddDateTimeAndKiloWattHoursUsed_Initialization_ShoudReturnNull()
        {
            //Arrange

            //Act
            var dailyStats = new DailyStatistic(new DateTime(2000, 10, 1), 1000);
            //Assert
            dailyStats.ShouldNotBe(null);
        }

        [Test]
        public void AddDateTimeAndKiloWattHoursUsed_AddTimeAndElectricityInSameDay_ShoulReturnNull()
        {
            //Arrange
            var dailyStats=new DailyStatistic(new DateTime(2000,1,1),1000 );
            //Act
            var result = dailyStats.AddDateTimeAndKiloWattHoursUsed(new DateTime(2000, 1, 1, 0, 30, 1), 1001);
            //Assert
            result.ShouldBe(null);
        }

        [Test]
        public void AddDateTimeAndKiloWattHoursUsed_AddTimeAndElectricityInFollowingDay_ShoulReturnOneDailyStatistic()
        {
            //Arrange
            var dailyStats = new DailyStatistic(new DateTime(2000, 1, 1), 1000);
            //Act
            var result = dailyStats.AddDateTimeAndKiloWattHoursUsed(new DateTime(2000, 1, 2, 0, 0, 1), 1002);
            //Assert
            result.ShouldNotBe(null);
            result.Count.ShouldBe(1);
            result[0].GetDateTimeForDay().ShouldBe(new DateTime(2000,1,1,0,0,0));
            result[0].GetNumberOfKiloWattHoursUsed().ShouldBe(2);
        }

        [Test]
        public void AddDateTimeAndKiloWattHoursUsed_AddTimeAndElectricityInTwoDaysAfterStart_ShoulReturnTwoDailyStatistics()
        {
            //Arrange
            var dailyStats = new DailyStatistic(new DateTime(2000, 1, 1), 1000);
            //Act
            var result = dailyStats.AddDateTimeAndKiloWattHoursUsed(new DateTime(2000, 1, 3, 2, 30, 1), 1004);
            //Assert
            result.ShouldNotBe(null);
            result.Count.ShouldBe(2);
            result[0].GetDateTimeForDay().ShouldBe(new DateTime(2000, 1, 1, 0, 0, 0));
            result[0].GetNumberOfKiloWattHoursUsed().ShouldBe(2);
            result[1].GetDateTimeForDay().ShouldBe(new DateTime(2000, 1, 2, 0, 0, 0));
            result[1].GetNumberOfKiloWattHoursUsed().ShouldBe(2);
        }

        [Test]
        public void AddDateTimeAndKiloWattHoursUsed_AddTimeAndElectricityTwoDaysAfterStartSpanningNewYear_ShoulReturnTwoDailyStatistics()
        {
            //Arrange
            var dailyStats = new DailyStatistic(new DateTime(2001, 12, 31,23,1,2), 1000);
            //Act
            var result = dailyStats.AddDateTimeAndKiloWattHoursUsed(new DateTime(2002, 1, 2, 23, 30, 1), 1004);
            //Assert
            result.ShouldNotBe(null);
            result.Count.ShouldBe(2);
            result[0].GetDateTimeForDay().ShouldBe(new DateTime(2001, 12, 31, 0, 0, 0));
            result[0].GetNumberOfKiloWattHoursUsed().ShouldBe(2);
            result[1].GetDateTimeForDay().ShouldBe(new DateTime(2002, 1, 1, 0, 0, 0));
            result[1].GetNumberOfKiloWattHoursUsed().ShouldBe(2);
        }
    }
}