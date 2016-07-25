using System;
using ElectricityStatisticsLibrary.Statistics;
using NUnit.Framework;
using Shouldly;

namespace ElectricityStatisticsLibraryTests.Statistics
{
    public class DateTimeHelperTests
    {
        [Test]
        public void GetDateTimeToTheLastOfTheGivenWeek_InputDayIsSunday_ShouldReturnSameDateCloseToMidnight()
        {
            //Act
            var result=new DateTime(2016,7,24).GetDateTimeForTheLastDayOfTheGivenWeek();
            //Assert
            result.ShouldBe(new DateTime(2016,7,24,23,59,59,997));
        }

        [Test]
        public void GetDateTimeToTheLastOfTheGivenWeek_InputDayIsMonday_ShouldReturnSameDateCloseToMidnight()
        {
            //Act
            var result = new DateTime(2016, 7, 25).GetDateTimeForTheLastDayOfTheGivenWeek();
            //Assert
            result.ShouldBe(new DateTime(2016, 7, 31, 23, 59, 59, 997));
        }

        [Test]
        public void GetDateTimeToTheLastOfTheGivenWeek_InputDayIsTuesday_ShouldReturnSameDateCloseToMidnight()
        {
            //Act
            var result = new DateTime(2016, 7, 26).GetDateTimeForTheLastDayOfTheGivenWeek();
            //Assert
            result.ShouldBe(new DateTime(2016, 7, 31, 23, 59, 59, 997));
        }

        [Test]
        public void GetDateTimeToTheLastOfTheGivenWeek_InputDayIsWednesday_ShouldReturnSameDateCloseToMidnight()
        {
            //Act
            var result = new DateTime(2016, 7, 27).GetDateTimeForTheLastDayOfTheGivenWeek();
            //Assert
            result.ShouldBe(new DateTime(2016, 7, 31, 23, 59, 59, 997));
        }

        [Test]
        public void GetDateTimeToTheLastOfTheGivenWeek_InputDayIsThursday_ShouldReturnSameDateCloseToMidnight()
        {
            //Act
            var result = new DateTime(2016, 7, 28).GetDateTimeForTheLastDayOfTheGivenWeek();
            //Assert
            result.ShouldBe(new DateTime(2016, 7, 31, 23, 59, 59, 997));
        }

        [Test]
        public void GetDateTimeToTheLastOfTheGivenWeek_InputDayIsFriday_ShouldReturnSameDateCloseToMidnight()
        {
            //Act
            var result = new DateTime(2016, 7, 29).GetDateTimeForTheLastDayOfTheGivenWeek();
            //Assert
            result.ShouldBe(new DateTime(2016, 7, 31, 23, 59, 59, 997));
        }

        [Test]
        public void GetDateTimeToTheLastOfTheGivenWeek_InputDayIsSaturday_ShouldReturnSameDateCloseToMidnight()
        {
            //Act
            var result = new DateTime(2016, 7, 30).GetDateTimeForTheLastDayOfTheGivenWeek();
            //Assert
            result.ShouldBe(new DateTime(2016, 7, 31, 23, 59, 59, 997));
        }


        [Test]
        public void GetDateTimeForTheFirstDayOfTheGivenWeek_InputDayIsSunday_ShouldMondayBeforeSunday()
        {
            //Act
            var result = new DateTime(2016, 7, 3).GetDateTimeForTheFirstDayOfTheGivenWeek();
            //Assert
            result.ShouldBe(new DateTime(2016, 6, 27));
        }


        [Test]
        public void GetDateTimeForTheFirstDayOfTheGivenWeek_InputDayIsMonday_ShouldSameDate()
        {
            //Act
            var result = new DateTime(2016, 6, 27,1,2,3).GetDateTimeForTheFirstDayOfTheGivenWeek();
            //Assert
            result.ShouldBe(new DateTime(2016, 6, 27));
        }


        [Test]
        public void GetDateTimeForTheFirstDayOfTheGivenWeek_InputDayIsTuesday_ShouldMondayBeforeTuesday()
        {
            //Act
            var result = new DateTime(2016, 6, 28).GetDateTimeForTheFirstDayOfTheGivenWeek();
            //Assert
            result.ShouldBe(new DateTime(2016, 6, 27));
        }


        [Test]
        public void GetDateTimeForTheFirstDayOfTheGivenWeek_InputDayIsWednesday_ShouldMondayBeforeWednesday()
        {
            //Act
            var result = new DateTime(2016, 6, 29).GetDateTimeForTheFirstDayOfTheGivenWeek();
            //Assert
            result.ShouldBe(new DateTime(2016, 6, 27));
        }


        [Test]
        public void GetDateTimeForTheFirstDayOfTheGivenWeek_InputDayIsThursday_ShouldMondayBeforeThursday()
        {
            //Act
            var result = new DateTime(2016, 6, 30).GetDateTimeForTheFirstDayOfTheGivenWeek();
            //Assert
            result.ShouldBe(new DateTime(2016, 6, 27));
        }


        [Test]
        public void GetDateTimeForTheFirstDayOfTheGivenWeek_InputDayIsFriday_ShouldMondayBeforeFriday()
        {
            //Act
            var result = new DateTime(2016, 7, 1).GetDateTimeForTheFirstDayOfTheGivenWeek();
            //Assert
            result.ShouldBe(new DateTime(2016, 6, 27));
        }


        [Test]
        public void GetDateTimeForTheFirstDayOfTheGivenWeek_InputDayIsSaturday_ShouldMondayBeforeSaturday()
        {
            //Act
            var result = new DateTime(2016, 7, 2).GetDateTimeForTheFirstDayOfTheGivenWeek();
            //Assert
            result.ShouldBe(new DateTime(2016, 6, 27));
        }
    }
}