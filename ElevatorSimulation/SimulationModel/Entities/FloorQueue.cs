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
            if (tenant.CallType == CallType.Up)
            {
                m_upQueue.Enqueue(tenant);
            }
            else
            {
                m_downQueue.Enqueue(tenant);
            }
        }
        public Tenant DequeueUp()
        {
            return m_upQueue.Dequeue();
        }
        public Tenant DequeueDown()
        {
            return m_downQueue.Dequeue();
        }
        public void Reset()
        {
            m_upQueue.Clear();
            m_downQueue.Clear();
        }

        private Queue<Tenant> m_upQueue = new Queue<Tenant>();
        private Queue<Tenant> m_downQueue = new Queue<Tenant>();
    }
}
