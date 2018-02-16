using System;
using System.Collections.Generic;

namespace ElevatorSimulation.SimulationModel.Entities
{
    /// <summary>
    /// Queue of tenants on the specific floor.
    /// Performs queue(FCFS) of transactions and
    /// contains two queues for each of two types 
    /// of transactions
    /// </summary>
    class TenantQueue : Entity
    {
        /// <summary>
        /// Floor number
        /// </summary>
        public int Floor { get; }

        public int Count
        {
            get
            {
                return m_queues[Direction.Up].Count + m_queues[Direction.Down].Count;
            }
        }

        public TenantQueue(int floor)
        {
            Floor = floor;
        }

        public void Enqueue(Tenant tenant)
        {
            m_queues[tenant.Direction].Enqueue(tenant);

            OnChanged();
        }
        public Tenant Dequeue(Direction Direction)
        {
            OnChanged();

            return m_queues[Direction].Dequeue();
        }
        public Tenant Peek(Direction Direction)
        {
            return m_queues[Direction].Peek();
        }

        /// <summary>
        /// Getter of current state of hallcall of
        /// this direction
        /// </summary>
        /// <param name="direction"></param>
        public bool IsHallcall(Direction direction)
        {
            return m_queues[direction].Count > 0;
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
