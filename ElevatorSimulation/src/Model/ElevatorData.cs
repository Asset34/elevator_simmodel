using System;

namespace ElevatorSimulation.Model
{
    class ElevatorData
    {
        /// <summary>
        /// Identification number of the elevator
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Starting floor
        /// </summary>
        public int StartFloor { get; set; }
        /// <summary>
        /// Maximum number of tenants inside the elevator
        /// </summary>
        public int Capacity { get; set; }
        /// <summary>
        /// Period of movement
        /// </summary>
        public int Period { get; set; }
        /// <summary>
        /// Spread of the period
        /// </summary>
        public int Spread { get; set; }

        public ElevatorData()
        {
        }
        public ElevatorData(ElevatorData data)
        {
            ID = data.ID;
            StartFloor = data.StartFloor;
            Capacity = data.Capacity;
            Period = data.Period;
            Spread = data.Spread;
        }
    }
}
