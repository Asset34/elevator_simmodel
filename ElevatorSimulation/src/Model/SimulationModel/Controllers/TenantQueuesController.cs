using System;
using System.Collections.Generic;
using System.Linq;

using ElevatorSimulation.Model.SimulationModel.Entities;

namespace ElevatorSimulation.Model.SimulationModel.Controllers
{
    /// <summary>
    /// Controller of the complex of tenant queues which
    /// performs subsystem 'Transaction queues' of the
    /// queueing theory
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
            foreach (TenantQueue queue in m_queues.Values)
            {
                queue.Reset();
            }
        }

        private Dictionary<int, TenantQueue> m_queues = new Dictionary<int, TenantQueue>();
    }
}
