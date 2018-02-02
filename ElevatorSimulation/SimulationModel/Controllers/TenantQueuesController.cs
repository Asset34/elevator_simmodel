using System;
using System.Collections.Generic;

using ElevatorSimulation.SimulationModel.Transactions;
using ElevatorSimulation.SimulationModel.Entities;

namespace ElevatorSimulation.SimulationModel.Controllers
{
    /// <summary>
    /// Controller of the complex of tenant queues
    /// </summary>
    class TenantQueuesController : Controller
    {
        public List<TenantQueue> Queues
        {
            get { return m_queues; }
        }

        public void Enqueue(int floor, Tenant tenant)
        {
            m_queues[floor - 1].Enqueue(tenant);
        }
        public Tenant Dequeue(int floor, CallType callType)
        {
            return m_queues[floor - 1].Dequeue(callType);
        }
        public override void Reset()
        {
            foreach (TenantQueue TenantQueue in m_queues)
            {
                TenantQueue.Reset();
            }
        }

        private List<TenantQueue> m_queues = new List<TenantQueue>();
    }
}
