using System;
using System.Collections.Generic;
using System.Linq;

using ElevatorSimulation.SimulationModel.Entities;

namespace ElevatorSimulation.SimulationModel.Controllers
{
    /// <summary>
    /// Controller of the complex of tenant generators
    /// </summary>
    class TenantGeneratorsController
    {
        /// <summary>
        /// Numbers of floors for which there is generator
        /// </summary>
        public int[] Floors
        {
            get { return m_generators.Keys.ToArray(); }
        }

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

        public void Reset()
        {
            foreach (TenantGenerator generator in m_generators.Values)
            {
                generator.Reset();
            }
        }
        
        private SortedDictionary<int, TenantGenerator> m_generators
            = new SortedDictionary<int, TenantGenerator>();
    }
}
