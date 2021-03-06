﻿using ElevatorSimulation.Model.SimulationModel.Entities;
using ElevatorSimulation.Model.SimulationModel.Transactions;

namespace ElevatorSimulation.Model.SimulationModel.Events
{
    /// <summary>
    /// Event of the generating and adding new tetant to the queue
    /// </summary>
    class NewTenantEvent : EventOf2<TenantGenerator, TenantQueue>
    {
        /// <summary>
        /// Generated tenant
        /// </summary>
        public Tenant Tenant { get; private set; }

        public NewTenantEvent(int time, ElevatorSimModel model, TenantGenerator generator, TenantQueue queue)
            : base(time, model, generator, queue)
        {
            Priority = 3;
        }
        public override void Execute()
        {
            // Generate new tenant
            Tenant = m_p1.Generate();
            
            // Create new event
            if (!m_p2.IsHallcall(Tenant.Direction))
            {
                m_model.CreateEvent_NewHallcall(Tenant);
            }

            // Add tenant to the associated queue
            m_p2.Enqueue(Tenant);

            // Create next event of tenant generation
            m_model.CreateEvent_NewTenant(m_p1, m_p2);
        }
        public override string ToString()
        {
            return string.Format(
                "Generator[floor:{0}] - generate tenant[id:{1}; path:{2}->{3}]",
                Tenant.FloorFrom,
                Tenant.ID,
                Tenant.FloorFrom,
                Tenant.FloorTo
                );
        }
    }
}
