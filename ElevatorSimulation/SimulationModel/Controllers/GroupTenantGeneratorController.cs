using System;
using System.Collections.Generic;

using ElevatorSimulation.SimulationModel.Transactions;
using ElevatorSimulation.SimulationModel.Entities;

namespace ElevatorSimulation.SimulationModel.Controllers
{
    /// <summary>
    /// Controller of the complex of tenant generators
    /// </summary>
    class GroupTenantGeneratorController
    {
        public List<TenantGenerator> Generators
        {
            get { return m_generators; }
        }

        public Tenant GenerateTenant(int floor)
        {
            return m_generators[floor - 1].Generate();
        }

        private List<TenantGenerator> m_generators = new List<TenantGenerator>();
    }
}
