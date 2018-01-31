namespace ElevatorSimulation.SimulationModel.Parameters
{
    /// <summary>
    /// Data class of parameters for elevator of
    /// elevator simulation model
    /// </summary>
    class ElevatorParameters
    {
        /// <summary>
        /// Period of elevator movement between the near floors
        /// </summary>
        public int Period { get; set; }
        /// <summary>
        /// Variance of period
        /// </summary>
        public int DPeriod { get; set; }
        /// <summary>
        /// Maximum number of tenants
        /// </summary>
        public int Capacity { get; set; }
        /// <summary>
        /// Starting position
        /// </summary>
        public int StartFloor { get; set; }

        public ElevatorParameters
            (
            int period,
            int dPeriod,
            int capacity,
            int startDloor
            )
        {
            Period = period;
            DPeriod = dPeriod;
            Capacity = capacity;
            StartFloor = StartFloor;
        }
    }
}
