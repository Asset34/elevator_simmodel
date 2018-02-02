using System;
using System.Collections.Generic;

using ElevatorSimulation.SimulationModel.Transactions;

namespace ElevatorSimulation.SimulationModel.Entities
{
    /// <summary>
    /// Queue of tenants on the specific floor
    /// </summary>
    /// <remarks>
    /// Queue(FCFS) of transactions
    /// </remarks>
    class FloorQueue : Resettable
    {
        /// <summary>
        /// Floor number
        /// </summary>
        public int Floor { get; }

        public FloorQueue(int floor)
        {
            Floor = floor;
        }
        public void Enqueue(Tenant tenant)
        {
            m_queues[tenant.CallType].Enqueue(tenant);
        }
        public Tenant Dequeue(CallType callType)
        {
            return m_queues[callType].Dequeue();
        }
        public void Reset()
        {
            m_queues[CallType.Up].Clear();
            m_queues[CallType.Down].Clear();
        }

        private readonly Dictionary<CallType, Queue<Tenant>> m_queues
            = new Dictionary<CallType, Queue<Tenant>>()
            {
                { CallType.Up  , new Queue<Tenant>()},
                { CallType.Down, new Queue<Tenant>()}
            };
    }
}
