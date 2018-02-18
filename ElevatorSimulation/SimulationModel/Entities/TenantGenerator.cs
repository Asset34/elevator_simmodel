using ElevatorSimulation.SimulationModel.Distributions;
using ElevatorSimulation.SimulationModel.Transactions;

namespace ElevatorSimulation.SimulationModel.Entities
{
    /// <summary>
    /// Generator of tenants on the specific floor which
    /// performs 'Transaction source' entity of the queueing theory
    /// </summary>
    class TenantGenerator : Entity
    {
        /// <summary>
        /// Floor number
        /// </summary>
        public int Floor { get; }

        public TenantGenerator(int floor, Distribution floorDistr)
        {
            Floor = floor;
            m_floorDistr = new DistributionWithGaps(floorDistr, floor);
        }

        /// <summary>
        /// Generate new tenant for specific floor
        /// </summary>
        /// <returns></returns>
        public Tenant Generate()
        {
            m_tenantCounter++;

            return new Tenant(m_tenantCounter, Floor, m_floorDistr.GetValue());
        }

        private readonly Distribution m_floorDistr;

        private static int m_tenantCounter = 0;
    }
}
