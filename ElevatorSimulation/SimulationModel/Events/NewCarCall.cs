using ElevatorSimulation.SimulationModel.Controllers;
using ElevatorSimulation.SimulationModel.Transactions;

namespace ElevatorSimulation.SimulationModel.Events
{
    /// <summary>
    /// Event of adding the new car call to the
    /// one of the elevators
    /// </summary>
    /// <remarks>
    /// Arguments:
    ///    - ElevatorID
    ///    - Tenant
    /// </remarks>
    class NewCarcall : Event
    {
        public int ElevatorID { get; set; }
        /// <summary>
        /// Tenant generating the carcall
        /// </summary>
        public Tenant Tenant { get; set; }

        public NewCarcall(int time, EventController handler)
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
                "Car call {0} was added to elevator {1}",
                Tenant.FloorTo,
                ElevatorID
                );
        }
    }
}
