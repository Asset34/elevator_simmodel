using ElevatorSimulation.SimulationModel.Transactions;
using ElevatorSimulation.SimulationModel.Distributions;
using System;

namespace ElevatorSimulation.SimulationModel.Entities
{
    /// <summary>
    /// Generator of tenants on the specific floor
    /// </summary>
    /// <remarks>
    /// Transaction source
    /// </remarks>
    class TenantGenerator : Entitie
    {
        /// <summary>
        /// Floor number
        /// </summary>
        public int Floor { get; }

        public TenantGenerator(int floor, Distribution destinationFloorDistr)
        {
            Floor = floor;
            m_destinationFloorDistr = destinationFloorDistr;
            m_destinationFloorDistr
                = new DistributionWithGaps(destinationFloorDistr, floor);
        }

        public Tenant Generate()
        {
            m_tenantCounter++;

            return new Tenant
            {
                Id = m_tenantCounter,
                FloorFrom = Floor,
                FloorTo = m_destinationFloorDistr.GetValue()
            };
        }

        public override void Reset()
        {
            m_tenantCounter = 0;
        }

        private static int m_tenantCounter = 0;
        private readonly Distribution m_destinationFloorDistr;
    }
}
