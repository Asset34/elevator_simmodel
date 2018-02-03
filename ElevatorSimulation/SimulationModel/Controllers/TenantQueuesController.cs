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
        public Dictionary<int, TenantQueue> Queues
        {
            get { return m_queues; }
        }

        public void Enqueue(int floor, Tenant tenant)
        {
            m_queues[floor].Enqueue(tenant);
        }
        public Tenant Dequeue(int floor, CallType callType)
        {
            return m_queues[floor].Dequeue(callType);
        }
        public bool GetHallcallStatus(int floor, CallType callType)
        {
            return m_queues[floor].GetHallcallStatus(callType);
        }
        public override void Reset()
        {
            foreach (TenantQueue TenantQueue in m_queues.Values)
            {
                TenantQueue.Reset();
            }
        }

        private Dictionary<int, TenantQueue> m_queues = new Dictionary<int, TenantQueue>();
    }
}
