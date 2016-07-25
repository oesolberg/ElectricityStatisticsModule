using System;
using ElectricityStatisticsLibrary.Statistics;
using NUnit.Framework;
using Shouldly;

namespace ElectricityStatisticsLibraryTests.Statistics
{
    public class MonthlyStatisticsTests
    {
        [Test]
        public void AddDateTimeAndKiloWattHoursUsed_Initialization_ShoudReturnNull()
        {
            //Arrange

            //Act
            var monthlyStats = new MonthlyStatistic(new DateTime(2000, 10, 1), 1000);
            //Assert
            monthlyStats.ShouldNotBe(null);
        }

        [Test]
        public void AddDateTimeAndKiloWattHoursUsed_AddTimeAndElectricityInSameMonth_ShoulReturnNull()
        {
            //Arrange
            var monthlyStats=new MonthlyStatistic(new DateTime(2000,1,1),1000 );
            //Act
            var result = monthlyStats.AddDateTimeAndKiloWattHoursUsed(new DateTime(2000, 1, 23, 0, 30, 1), 1001);
            //Assert
            result.ShouldBe(null);
        }

        [Test]
        public void AddDateTimeAndKiloWattHoursUsed_AddTimeAndElectricityInFollowingMonth_ShoulReturnOneHourlyStatistic()
        {
            //Arrange
            var monthlyStats = new MonthlyStatistic(new DateTime(2000, 1, 1), 1000);
            //Act
            var result = monthlyStats.AddDateTimeAndKiloWattHoursUsed(new DateTime(2000, 2, 1, 1, 0, 1), 1002);
            //Assert
            result.ShouldNotBe(null);
            result.Count.ShouldBe(1);
            result[0].GetDateTimeForMonth().ShouldBe(new DateTime(2000,1,1));
            result[0].GetNumberOfKiloWattHoursUsed().ShouldBe(2);
        }

        [Test]
        public void AddDateTimeAndKiloWattHoursUsed_AddTimeAndElectricityInTwoMonthsAfterStart_ShoulReturnTwoonthlyStatistics()
        {
            //Arrange
            var monthlyStats = new MonthlyStatistic(new DateTime(2000, 1, 1), 1000);
            //Act
            var result = monthlyStats.AddDateTimeAndKiloWattHoursUsed(new DateTime(2000, 3, 1, 0, 30, 1), 1004);
            //Assert
            result.ShouldNotBe(null);
            result.Count.ShouldBe(2);
            result[0].GetDateTimeForMonth().ShouldBe(new DateTime(2000, 1, 1));
            result[0].GetNumberOfKiloWattHoursUsed().ShouldBe(2);
            result[1].GetDateTimeForMonth().ShouldBe(new DateTime(2000, 2, 1));
            result[1].GetNumberOfKiloWattHoursUsed().ShouldBe(2);
        }

        [Test]
        public void AddDateTimeAndKiloWattHoursUsed_AddTimeAndElectricityInTwoMonthsAfterStartSpanningNewYear_ShoulReturnTwoMonthlyStatistics()
        {
            //Arrange
            var monthlyStats = new MonthlyStatistic(new DateTime(2001, 12, 31,23,1,2), 1000);
            //Act
            var result = monthlyStats.AddDateTimeAndKiloWattHoursUsed(new DateTime(2002, 2, 1, 1, 30, 1), 1004);
            //Assert
            result.ShouldNotBe(null);
            result.Count.ShouldBe(2);
            result[0].GetDateTimeForMonth().ShouldBe(new DateTime(2001, 12, 1, 0, 0, 0));
            result[0].GetNumberOfKiloWattHoursUsed().ShouldBe(2);
            result[1].GetDateTimeForMonth().ShouldBe(new DateTime(2002, 1, 1, 0, 0, 0));
            result[1].GetNumberOfKiloWattHoursUsed().ShouldBe(2);
        }
    }
}