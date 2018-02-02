using ElevatorSimulation.SimulationModel.Controllers;
using ElevatorSimulation.SimulationModel.Transactions;

namespace ElevatorSimulation.SimulationModel.Events
{
    /// <summary>
    /// Event of adding the new hall call to the
    /// one of the elevators.
    /// </summary>
    /// <remarks>
    /// Arguments:
    ///    - Floor
    ///    - CallType
    /// Results:
    ///    - ElevatorID
    /// </remarks>
    class NewHallCall : Event
    {
        /// <summary>
        /// The floor of the call
        /// </summary>
        public int Floor { get; set; }
        /// <summary>
        /// Type of the call ("up" or "down")
        /// </summary>
        public CallType CallType { get; set; }

        /// <summary>
        /// ID of the elevator which accepted the call
        /// </summary>
        public int ElevatorID { get; set; }

        public NewHallCall(int time, EventController handler)
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
                "Hall call {0}({1}) was added to elevator {2}",
                Floor,
                CallType,
                ElevatorID
                );
        }
    }
}
