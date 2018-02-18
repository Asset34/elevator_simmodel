namespace ElevatorSimulation.SimulationModel.Transactions
{
    enum Direction
    {
        Up,
        Down
    }

    /// <summary>
    /// Model of the tenant which performs 'Transaction'
    /// entity of the queueing theory
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
        /// Destination of the movement
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
        
        public Tenant(int id, int floorFrom, int floorTo)
        {
            ID = id;
            FloorFrom = floorFrom;
            FloorTo = floorTo;
        }
    }
}
