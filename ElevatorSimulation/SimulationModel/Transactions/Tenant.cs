namespace ElevatorSimulation.SimulationModel.Transactions
{
    enum CallType
    {
        Up,
        Down
    }

    /// <summary>
    /// Model of the tenant
    /// </summary>
    class Tenant
    {
        public int Id { get; set; }
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
        public CallType CallType
        {
            get
            {
                if (FloorTo > FloorFrom)
                {
                    return CallType.Up;
                }
                else
                {
                    return CallType.Down;
                }
            }
        }  
    }
}
