using System;
using System.Collections.Generic;
using System.Linq;

using ElevatorSimulation.SimulationModel.Entities;

namespace ElevatorSimulation.SimulationModel.Controllers
{
    /// <summary>
    /// Controller of the complex of tenant queues
    /// </summary>
    class TenantQueuesController
    {
        /// <summary>
        /// Numbers of floors for which there is queue
        /// </summary>
        public int[] Floors
        {
            get { return m_queues.Keys.ToArray(); }
        }

        public void Add(TenantQueue queue)
        {
            m_queues.Add(queue.Floor, queue);
        }
        public void Remove(TenantQueue queue)
        {
            m_queues.Remove(queue.Floor);
        }
        public TenantQueue Get(int floor)
        {
            return m_queues[floor];
        }

        public void Reset()
        {
            foreach (TenantQueue TenantQueue in m_queues.Values)
            {
                TenantQueue.Reset();
            }
        }

        private SortedDictionary<int, TenantQueue> m_queues
            = new SortedDictionary<int, TenantQueue>();
    }
}
