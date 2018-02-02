using ElevatorSimulation.SimulationModel.Controllers;

namespace ElevatorSimulation.SimulationModel.Events
{
    /// <summary>
    /// Event of elevator with step = 1 floor
    /// </summary>
    /// <remarks>
    /// Arguments:
    ///    - ElevatorID
    /// Results:
    ///    - Floor
    /// </remarks>
    class ElevatorMovement : Event
    {
        /// <summary>
        /// ID of the elevator which pickig up the tenants
        /// </summary>
        public int ElevatorID { get; set; }
        
        /// <summary>
        /// The new floor number of the elevator
        /// </summary>
        public int Floor { get; set; }

        public ElevatorMovement(int time, EventController handler)
            :base(time, handler)
        {
        }
        public override void Execute()
        {
            m_handler.Handle(this);
        }
        public override string ToString()
        {
            return string.Format(
                "Elevator {0} moved to the {1} floor",
                ElevatorID,
                Floor);
        }
    }
}
