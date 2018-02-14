using System;
using System.Text;
using System.Collections.Generic;

using ElevatorSimulation.SimulationModel.Entities;

namespace ElevatorSimulation.SimulationModel.Events
{
    /// <summary>
    /// Event of picking up the tenants by one
    /// of the elevators
    /// </summary>
    class PickupEvent : EventOf2<TenantQueue, Elevator>
    {
        public override int Priority
        {
            get { return 5; }
        }

        /// <summary>
        /// Picked up tenants
        /// </summary>
        public List<Tenant> Tenants { get; private set; }

        public PickupEvent(int time, ElevatorSM model, TenantQueue queue, Elevator elevator)
            :base(time, model, queue, elevator)
        {
        }
        public override void Execute()
        {
            // Get list of picking up tenants
            Tenants = new List<Tenant>();
            while (m_p1.IsHallcall(m_p2.Direction) &&
                   m_p2.FreeCount > 0)
            {
                Tenants.Add(m_p1.Dequeue(m_p2.Direction));
            }

            // Add tenants to the elevator
            m_p2.Pickup(Tenants);
            
            // Add unduplicated carcalls from tenants
            foreach (Tenant tenant in Tenants)
            {
                // Create new carcall
                m_model.CreateEvent_NewCarcall(tenant, m_p2);
            }

            // Repeat call if not all tenants was picked up
            if (m_p1.IsHallcall(m_p2.Direction))
            {
                m_model.CreateEvent_NewHallcall(m_p1.Peek(m_p2.Direction));
            }
        }
        public override string ToString()
        {
            StringBuilder text = new StringBuilder();
            text.Append(String.Format("Elevator[id:{0}] - pick up[tenants:", m_p2.ID));

            foreach (Tenant tenant in Tenants)
            {
                text.Append(String.Format("{0},", tenant.ID));
            }

            text.Append(String.Format("; floor:{0}]", m_p1.Floor));

            return text.ToString();
        }
    }
}
