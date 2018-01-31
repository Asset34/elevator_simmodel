namespace ElevatorSimulation.SimulationModel.Parameters
{
    /// <summary>
    /// Data class of parameters for floor of
    /// elevator simulation model
    /// </summary>
    class FloorParameters
    {
        /// <summary>
        /// Period of tenants appearance
        /// </summary>
        public int Period { get; set; }
        /// <summary>
        /// Variance of period
        /// </summary>
        public int DPeriod { get; set; }

        public FloorParameters(int period, int dPeriod)
        {
            Period = period;
            DPeriod = dPeriod;
        }
    }
}
