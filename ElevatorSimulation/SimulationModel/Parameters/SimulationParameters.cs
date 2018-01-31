using System;

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
        public int NFloors { get; set; }      
        /// <summary>
        /// Number of elevators
        /// </summary>
        public int NElevators { get; set; }
        /// <summary>
        /// Complex of floor parameters
        /// </summary>
        public FloorParameters[] FloorParameters { get; set; }
        /// <summary>
        /// Complex of elevator parameters
        /// </summary>
        public ElevatorParameters[] ElevatorParameters { get; set; }

        public SimulationParameters
            (
            int nFloors, 
            int nElevators,
            FloorParameters[] floorParameters,
            ElevatorParameters[] elevatorParameters
            )
        {
            NFloors = nFloors;
            NElevators = nElevators;
            FloorParameters = floorParameters;
            ElevatorParameters = elevatorParameters;
        }
    }
}
