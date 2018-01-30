using System;
using System.Collections.Generic;

using ElevatorSimulation.Transactions;
using ElevatorSimulation.Entities;

namespace ElevatorSimulation.Controllers
{
    /// <summary>
    /// Controller of the complex of tenant floor queues
    /// </summary>
    class FloorQueueController : Resettable
    {
        public List<FloorQueue> Queues
        {
            get { return m_queues; }
        }

        public void Enqueue(int floor, Tenant tenant)
        {
            m_queues[floor - 1].Enqueue(tenant);
        }
        public Tenant Dequeue(int floor, RequestType type)
        {
            return m_queues[floor - 1].Dequeue(type);
        }
        public void Reset()
        {
            foreach (FloorQueue floorQueue in m_queues)
            {
                floorQueue.Reset();
            }
        }

        private List<FloorQueue> m_queues = new List<FloorQueue>();
    }
}
