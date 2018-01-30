using ElevatorSimulation.Transactions;
using ElevatorSimulation.Distributions;

namespace ElevatorSimulation.Entities
{
    /// <summary>
    /// Generator of tenants on the specific floor
    /// </summary>
    /// <remarks>
    /// Transaction source
    /// </remarks>
    class TenantGenerator
    {
        /// <summary>
        /// Floor number
        /// </summary>
        public int Floor { get; }

        public TenantGenerator(Distribution destinationFloorDistr, int floor)
        {
            m_destinationFloorDistr = destinationFloorDistr;
            Floor = floor;
        }
        public Tenant Generate()
        {
            m_tenantCounter++;

            return new Tenant(m_tenantCounter, Floor, m_destinationFloorDistr.GetValue());
        }

        private static int m_tenantCounter = 0;
        private readonly Distribution m_destinationFloorDistr;
    }
}
