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
            m_queues[tenant.Direction].Enqueue(tenant);
        }
        public Tenant Dequeue(Direction Direction)
        {
            return m_queues[Direction].Dequeue();
        }
        public Tenant Peek(Direction Direction)
        {
            return m_queues[Direction].Peek();
        }

        /// <summary>
        /// Getter of current state of hallcall from this
        /// floor and direction
        /// </summary>
        /// <param name="Direction"></param>
        /// <returns>
        /// true  - hallcall is enabled
        /// false - hallcall is disabled
        /// </returns>
        public bool GetHallcallStatus(Direction Direction)
        {
            return m_queues[Direction].Count > 0;
        }

        public void Reset()
        {
            m_queues[Direction.Up].Clear();
            m_queues[Direction.Down].Clear();
        }

        private readonly Dictionary<Direction, Queue<Tenant>> m_queues
            = new Dictionary<Direction, Queue<Tenant>>()
            {
                { Direction.Up  , new Queue<Tenant>()},
                { Direction.Down, new Queue<Tenant>()}
            };
    }
}
