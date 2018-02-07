using ElevatorSimulation.SimulationModel.Transactions;
using ElevatorSimulation.SimulationModel.Distributions;

namespace ElevatorSimulation.SimulationModel.Entities
{
    /// <summary>
    /// Generator of tenants on the specific floor
    /// </summary>
    /// <remarks>
    /// Transaction source
    /// </remarks>
    class TenantGenerator : Entity
    {
        /// <summary>
        /// Floor number
        /// </summary>
        public int Floor { get; }

        public TenantGenerator(int floor, Distribution destinationFloorDistr)
        {
            Floor = floor;
            m_destinationFloorDistr = new DistributionWithGaps(destinationFloorDistr, floor);
        }
        public Tenant Generate()
        {
            m_tenantCounter++;

            return new Tenant
            {
                ID = m_tenantCounter,
                FloorFrom = Floor,
                FloorTo = m_destinationFloorDistr.GetValue()
            };
        }

        public void Reset()
        {
            m_tenantCounter = 0;
        }

        private static int m_tenantCounter = 0;
        private readonly Distribution m_destinationFloorDistr;
    }
}
