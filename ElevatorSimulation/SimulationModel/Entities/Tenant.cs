namespace ElevatorSimulation.SimulationModel.Entities
{
    enum Direction
    {
        Up,
        Down
    }

    /// <summary>
    /// Model of the tenant
    /// </summary>
    class Tenant
    {
        public int ID { get; set; }
        /// <summary>
        /// Starting floor
        /// </summary>
        public int FloorFrom { get; set; }
        /// <summary>
        /// Destination floor
        /// </summary>
        public int FloorTo { get; set; }
        /// <summary>
        /// Destination of movement
        /// </summary>
        public Direction Direction
        {
            get
            {
                if (FloorTo > FloorFrom)
                {
                    return Direction.Up;
                }
                else
                {
                    return Direction.Down;
                }
            }
        }  
    }
}
