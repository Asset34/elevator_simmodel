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
    ///    - Tenant
    /// Results:
    ///    - ElevatorID
    /// </remarks>
    class NewHallcall : Event
    {
        /// <summary>
        /// Tenant generating the hallcall
        /// </summary>
        public Tenant Tenant { get; set; }

        /// <summary>
        /// ID of the elevator which accepted the call
        /// </summary>
        public int ElevatorID { get; set; }

        public NewHallcall(int time, EventController handler)
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
                Tenant.FloorFrom,
                Tenant.CallType,
                ElevatorID
                );
        }
    }
}
