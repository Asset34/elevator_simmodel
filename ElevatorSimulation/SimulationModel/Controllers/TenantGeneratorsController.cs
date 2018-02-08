using System;
using System.Collections.Generic;
using System.Linq;

using ElevatorSimulation.SimulationModel.Transactions;
using ElevatorSimulation.SimulationModel.Entities;

namespace ElevatorSimulation.SimulationModel.Controllers
{
    /// <summary>
    /// Controller of the complex of tenant generators
    /// </summary>
    class TenantGeneratorsController
    {
        public void Add(TenantGenerator generator)
        {
            m_generators.Add(generator.Floor, generator);
        }
        public void Remove(TenantGenerator generator)
        {
            m_generators.Remove(generator.Floor);
        }
        public TenantGenerator Get(int floor)
        {
            return m_generators[floor];
        }

        public int[] GetFloors()
        {
            return m_generators.Keys.ToArray();
        }

        public void Reset()
        {
            foreach (TenantGenerator generator in m_generators.Values)
            {
                generator.Reset();
            }
        }
        
        private Dictionary<int, TenantGenerator> m_generators = new Dictionary<int, TenantGenerator>();
    }
}
