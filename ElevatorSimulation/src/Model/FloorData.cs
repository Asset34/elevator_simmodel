using System;

namespace ElevatorSimulation.Model
{
    class FloorData
    {
        /// <summary>
        /// Identification number of the floor
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Period of generation oftenants
        /// </summary>
        public int Period { get; set; }
        /// <summary>
        /// Spread of the period
        /// </summary>
        public int Spread { get; set; }

        public FloorData()
        {
        }
        public FloorData(FloorData data)
        {
            ID = data.ID;
            Period = data.Period;
            Spread = data.Spread;
        }
    }
}
