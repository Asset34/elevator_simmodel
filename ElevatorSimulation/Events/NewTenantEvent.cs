using System;

using ElevatorSimulation.Transactions;

namespace ElevatorSimulation.Events
{
    /// <summary>
    /// Event of entering new tenant the floor queue
    /// </summary>
    class NewTenantEvent : Event
    {
        public NewTenantEvent(Request tenant)
        {
            m_tenant = tenant;
        }
        public override void Execute()
        {
            // TODO
            throw new NotImplementedException();
        }

        private readonly Request m_tenant;
    }
}
