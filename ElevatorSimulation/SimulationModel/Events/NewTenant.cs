using ElevatorSimulation.SimulationModel.Controllers;
using ElevatorSimulation.SimulationModel.Transactions;

namespace ElevatorSimulation.SimulationModel.Events
{
    /// <summary>
    /// Event of the generating and adding new tetant to the queue
    /// </summary>
    /// <remarks>
    /// Arguments:
    ///    - Floor
    ///  Results:
    ///    - Tenant
    /// </remarks>
    class NewTenant : Event
    {
        /// <summary>
        /// The starting floor of generating tenant
        /// </summary>
        public int Floor { get; set; }

        /// <summary>
        /// ID of generated tenant
        /// </summary>
        public int TenantID { get; set; }

        public NewTenant(int time, EventController mediator)
            :base(time, mediator)
        {
        }
        public override void Execute()
        {
            m_handler.Handle(this);
        }
        public override string ToString()
        {
            return string.Format(
                "Tenant {0} was generated on the {1} floor",
                TenantID,
                Floor
                );
        }
    }
}
