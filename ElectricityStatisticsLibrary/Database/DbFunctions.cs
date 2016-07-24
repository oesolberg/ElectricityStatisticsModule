using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using Dapper;
using DapperExtensions;
using ElectricityStatisticsLibrary.Statistics;

namespace ElectricityStatisticsLibrary.Database
{
    public class DbFunctions
    {
        private readonly string _connectionString;

        public DbFunctions()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public DateTime GetStartTimeForElectricityData()
        {
            using (var cn = new System.Data.SqlClient.SqlConnection(_connectionString))
            {
                cn.Open();

                var result = cn.Query<DateTime>("SELECT TOP 1 [FileCreatedDateTime] FROM [ElectricityData] where [HasAcceptedElectricityValue]=1 order by [FileCreatedDateTime] ").FirstOrDefault();
                cn.Close();
                return result;
            }
        }

        public DateTime GetEndTimeForElectricityData()
        {
            using (var cn = new System.Data.SqlClient.SqlConnection(_connectionString))
            {
                cn.Open();

                var result = cn.Query<DateTime>("SELECT TOP 1 [FileCreatedDateTime] FROM [ElectricityData] where [HasAcceptedElectricityValue]=1 order by [FileCreatedDateTime] desc").FirstOrDefault();
                cn.Close();
                return result;
            }
        }

        public List<ElectricityData> GetDataForPeriode(DateTime startDateTime, DateTime endDateTime)
        {
            var queryForDataOverGivenPeriode = "Select [Id],[ElectricityValue] ,[ElectricityValueSetByUser],[FileCreatedDateTime],[CreatedDateTime] ,[HasAcceptedElectricityValue] FROM  [dbo].[ElectricityData] " +
                            "WHERE [HasAcceptedElectricityValue]=1 AND [FileCreatedDateTime]>='" + startDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' AND [FileCreatedDateTime]<='" + endDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff") +
                            "' ORDER BY [FileCreatedDateTime]";
            using (var cn = new System.Data.SqlClient.SqlConnection(_connectionString))
            {
                cn.Open();

                var result = cn.Query<ElectricityData>(queryForDataOverGivenPeriode);
                cn.Close();
                return result.ToList();


            }
        }

        public void SaveHourlyStatistic(HourlyStatistic hourlyStatistic)
        {
            using (var cn = new System.Data.SqlClient.SqlConnection(_connectionString))
            {
                cn.Open();
                var result = cn.Insert(hourlyStatistic);
                cn.Close();
            }
        }
    }

    public class ElectricityData
    {
        public int Id { get; set; }
        public int ElectricityValue { get; set; }
        public int? ElectricityValueSetByUser { get; set; }
        public DateTime FileCreatedDateTime { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public bool HasAcceptedElectricityValue { get; set; }
    }
}