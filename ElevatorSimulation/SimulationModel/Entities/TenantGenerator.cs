using ElevatorSimulation.SimulationModel.Distributions;

namespace ElevatorSimulation.SimulationModel.Entities
{
    /// <summary>
    /// Generator of tenants on the specific floor.
    /// Performs transaction source
    /// </summary>
    class TenantGenerator
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
        public Tenant Generate()
        {
            m_tenantCounter++;

            return new Tenant
            {
                ID = m_tenantCounter,
                FloorFrom = Floor,
                FloorTo = m_floorDistr.GetValue()
            };
        }

        public void Reset()
        {
            m_tenantCounter = 0;
        }

        private static int m_tenantCounter = 0;
        private readonly Distribution m_floorDistr;
    }
}
