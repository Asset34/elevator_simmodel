namespace ElevatorSimulation.SimulationModel.Statistics
{
    enum Operation
    {
        Average
    }

    /// <summary>
    /// Class for handle statistics with set of operations
    /// </summary>
    class StatisticHandler
    {
        /// <summary>
        /// Handling statistic
        /// </summary>
        public Statistic Statistic { get; set; }
        /// <summary>
        /// Last completed operation
        /// </summary>
        public Operation Operation { get; private set; }
        /// <summary>
        /// Last result
        /// </summary>
        public double Result { get; private set; }

        /// <summary>
        /// Handle setted statistic with chosen operation
        /// </summary>
        /// <param name="operation"> Handle operation </param>
        /// <returns></returns>
        public double Handle(Operation operation)
        {
            switch (operation)
            {
                case Operation.Average:
                    Average();
                    Operation = Operation.Average;
                    break;
            }

            return Result;
        }

        /* Handlings */
        private void Average()
        {
            double sum = 0;
            int count = 0;

            int d;
            for (int i = 0; i < Statistic.Data.Count - 1; i++)
            {
                d = Statistic.Data[i + 1].Time - Statistic.Data[i].Time;

                count += d;
                sum += Statistic.Data[i].Value * d;
            }

            count++;
            sum += Statistic.Data[Statistic.Data.Count - 1].Value;

            Result = sum / count;
        }

        public override string ToString()
        {
            return string.Format("{0} [{1}]: {2}", Statistic.Name, Operation, Result);
        }
    }
}
