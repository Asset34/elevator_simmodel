using ElevatorSimulation.SimulationModel.Entities;
using ElevatorSimulation.SimulationModel.Transactions;

namespace ElevatorSimulation.SimulationModel.Events
{
    /// <summary>
    /// Event of the generating and adding new tetant to the queue
    /// </summary>
    class NewTenantEvent : EventOf2<TenantGenerator, TenantQueue>
    {
        /// <summary>
        /// Generated tenant
        /// </summary>
        public Tenant Tenant { get; private set; }

        public NewTenantEvent(int time, EventProvider provider, TenantGenerator generator, TenantQueue queue)
            : base(time, provider, generator, queue)
        { 
        }
        public override void Execute()
        {
            // Generate new tenant
            Tenant = m_p1.Generate();

            // Create new event
            if (!m_p2.GetHallcallStatus(Tenant.CallType))
            {
                m_provider.CreateEvent_NewHallcall(Tenant);
            }

            // Create new event of tenant generation
            m_provider.CreateEvent_NewTenant(m_p1, m_p2);
        }
        public override string ToString()
        {
            return string.Format(
                "Tenant {0} was generated on the {1} floor",
                Tenant.ID,
                Tenant.FloorFrom
                );
        }
    }
}
