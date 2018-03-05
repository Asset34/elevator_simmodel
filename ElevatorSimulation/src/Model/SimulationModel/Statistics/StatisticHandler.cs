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
        public static SPair Average(Statistic statistic)
        {
            double sum = 0;
            int count = 0;

            int interval;
            for (int i = 1; i < statistic.Data.Count; i++)
            {
                // Get next interval
                interval = statistic.Data[i].Time - statistic.Data[i - 1].Time;
                count += interval;

                // Compute area on this interval
                sum += statistic.Data[i - 1].Value * interval;
            }

            return new SPair(string.Format("{0} (Average)", statistic.Name), sum / count);
        }
    }
}
