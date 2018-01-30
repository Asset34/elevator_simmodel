using System;

namespace ElevatorSimulation
{
    /// <summary>
    /// Input parameters of the elevator simulation model
    /// </summary>
    class SimulationParameters
    {
        /// <summary>
        /// Number of floors
        /// </summary>
        public int nFloors { get; set; }
        
        /// <summary>
        /// Number of elevators
        /// </summary>
        public int nElevators { get; set; }
        
        /// <summary>
        /// Periods of people apperance
        /// </summary>
        public Tuple<double, double>[] PeriodsAppearance { get; set; }

        /// <summary>
        /// Period of elevator movement between the adjacent floors
        /// </summary>
        public Tuple<double, double> MovementPeriod { get; set; }
    }
}
