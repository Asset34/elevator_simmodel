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
    class TenantQueue : Entity
    {
        /// <summary>
        /// Floor number
        /// </summary>
        public int Floor { get; }

        public TenantQueue(int floor)
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
        public Tenant Peek(CallType callType)
        {
            return m_queues[callType].Peek();
        }

        /// <summary>
        /// Getter of current state of hallcall from this
        /// floor and direction
        /// </summary>
        /// <param name="callType"></param>
        /// <returns>
        /// true  - hallcall is enabled
        /// false - hallcall is disabled
        /// </returns>
        public bool GetHallcallStatus(CallType callType)
        {
            return m_queues[callType].Count > 0;
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
