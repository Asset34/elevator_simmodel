namespace ElevatorSimulation.SimulationModel.Parameters
{
    /// <summary>
    /// Input parameters of the simulation model
    /// </summary>
    class SimulationParameters
    {
        /// <summary>
        /// Number of floors
        /// </summary>
        public int NumFloors { get; set; }      
        /// <summary>
        /// Number of elevators
        /// </summary>
        public int NumElevators { get; set; }
        /// <summary>
        /// Complex of floor parameters
        /// </summary>
        public TenantGeneratorParameters[] TenantGeneratorParameters { get; set; }
        /// <summary>
        /// Complex of elevator parameters
        /// </summary>
        public ElevatorParameters[] ElevatorParameters { get; set; }
    }
}
