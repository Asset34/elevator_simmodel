using System;
using System.Collections.Generic;

using ElevatorSimulation.SimulationModel.Transactions;
using ElevatorSimulation.SimulationModel.Entities;

namespace ElevatorSimulation.SimulationModel.Controllers
{
    /// <summary>
    /// Controller of the complex of tenant generators
    /// </summary>
    class TenantGeneratorsController : Controller
    {
        public Dictionary<int, TenantGenerator> Generators
        {
            get { return m_generators; }
        }

        public override void Reset()
        {
            foreach (TenantGenerator generator in m_generators.Values)
            {
                generator.Reset();
            }
        }
       
        private Dictionary<int, TenantGenerator> m_generators = new Dictionary<int, TenantGenerator>();
    }
}
