using System;
using System.Collections.Generic;

using ElevatorSimulation.SimulationModel.Transactions;
using ElevatorSimulation.SimulationModel.Entities;

namespace ElevatorSimulation.SimulationModel.Controllers
{
    /// <summary>
    /// Controller of the complex of tenant queues
    /// </summary>
    class TenantQueuesController
    {
        public Dictionary<int, TenantQueue> Queues
        {
            get { return m_queues; }
        }

        public void Reset()
        {
            foreach (TenantQueue TenantQueue in m_queues.Values)
            {
                TenantQueue.Reset();
            }
        }

        private Dictionary<int, TenantQueue> m_queues = new Dictionary<int, TenantQueue>();
    }
}
