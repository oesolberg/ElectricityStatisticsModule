using System;
using NUnit.Framework;
using Shouldly;

namespace ElectricityStatisticsLibrary.Statistics
{
    public class HourlyStatisticsTests
    {
        [Test]
        public void AddDateTimeAndKiloWattHoursUsed_Initialization_ShoudReturnNull()
        {
            //Arrange

            //Act
            var hourlyStats = new HourlyStatistic(new DateTime(2000, 10, 1), 1000);
            //Assert
            hourlyStats.ShouldNotBe(null);
        }

        [Test]
        public void AddDateTimeAndKiloWattHoursUsed_AddTimeAndElectricityInSameHour_ShoulReturnNull()
        {
            //Arrange
            var hourlyStats=new HourlyStatistic(new DateTime(2000,1,1),1000 );
            //Act
            var result = hourlyStats.AddDateTimeAndKiloWattHoursUsed(new DateTime(2000, 1, 1, 0, 30, 1), 1001);
            //Assert
            result.ShouldBe(null);
        }

        [Test]
        public void AddDateTimeAndKiloWattHoursUsed_AddTimeAndElectricityInFollowingHour_ShoulReturnOneHourlyStatistic()
        {
            //Arrange
            var hourlyStats = new HourlyStatistic(new DateTime(2000, 1, 1), 1000);
            //Act
            var result = hourlyStats.AddDateTimeAndKiloWattHoursUsed(new DateTime(2000, 1, 1, 1, 0, 1), 1002);
            //Assert
            result.ShouldNotBe(null);
            result.Count.ShouldBe(1);
            result[0].GetDateTimeForHour().ShouldBe(new DateTime(2000,1,1,0,0,0));
            result[0].GetNumberOfKiloWattHoursUsed().ShouldBe(2);
        }

        [Test]
        public void AddDateTimeAndKiloWattHoursUsed_AddTimeAndElectricityInTwoHoursAfterStart_ShoulReturnTwoHourlyStatistics()
        {
            //Arrange
            var hourlyStats = new HourlyStatistic(new DateTime(2000, 1, 1), 1000);
            //Act
            var result = hourlyStats.AddDateTimeAndKiloWattHoursUsed(new DateTime(2000, 1, 1, 2, 30, 1), 1004);
            //Assert
            result.ShouldNotBe(null);
            result.Count.ShouldBe(2);
            result[0].GetDateTimeForHour().ShouldBe(new DateTime(2000, 1, 1, 0, 0, 0));
            result[0].GetNumberOfKiloWattHoursUsed().ShouldBe(2);
            result[1].GetDateTimeForHour().ShouldBe(new DateTime(2000, 1, 1, 1, 0, 0));
            result[1].GetNumberOfKiloWattHoursUsed().ShouldBe(2);
        }

        [Test]
        public void AddDateTimeAndKiloWattHoursUsed_AddTimeAndElectricityInTwoHoursAfterStartSpanningNewYear_ShoulReturnTwoHourlyStatistics()
        {
            //Arrange
            var hourlyStats = new HourlyStatistic(new DateTime(2001, 12, 31,23,1,2), 1000);
            //Act
            var result = hourlyStats.AddDateTimeAndKiloWattHoursUsed(new DateTime(2002, 1, 1, 1, 30, 1), 1004);
            //Assert
            result.ShouldNotBe(null);
            result.Count.ShouldBe(2);
            result[0].GetDateTimeForHour().ShouldBe(new DateTime(2001, 12, 31, 23, 0, 0));
            result[0].GetNumberOfKiloWattHoursUsed().ShouldBe(2);
            result[1].GetDateTimeForHour().ShouldBe(new DateTime(2002, 1, 1, 0, 0, 0));
            result[1].GetNumberOfKiloWattHoursUsed().ShouldBe(2);
        }
    }
}