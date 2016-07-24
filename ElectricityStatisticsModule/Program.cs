using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityStatisticsModule
{
    class Program
    {
        static void Main(string[] args)
        {
            var test = new ElectricityStatisticsLibrary.Database.DbFunctions();
            var stat=new ElectricityStatisticsLibrary.StatisticsFacade();
            stat.RunStatistics();
            Console.ReadLine();
        }
    }
}
