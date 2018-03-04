using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulation.Model.SimulationModel.Statistics
{
    /// <summary>
    /// Class which contain handle operations of statistical data
    /// </summary>
    static class StatisticHandler
    {
        /// <summary>
        /// Get average value
        /// </summary>
        /// <param name="statistic"></param>
        /// <returns></returns>
        public static double Average(Statistic statistic)
        {
            double sum = 0;
            int count = 0;

            int d;
            for (int i = 0; i < statistic.Data.Count - 1; i++)
            {
                d = statistic.Data[i + 1].Time - statistic.Data[i].Time;

                count += d;
                sum += statistic.Data[i].Value * d;
            }

            count++;
            sum += statistic.Data[statistic.Data.Count - 1].Value;

            return sum / count;
        }
    }
}
