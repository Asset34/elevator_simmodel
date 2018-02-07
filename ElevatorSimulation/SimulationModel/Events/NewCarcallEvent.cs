﻿using ElevatorSimulation.SimulationModel.Entities;
using ElevatorSimulation.SimulationModel.Transactions;

namespace ElevatorSimulation.SimulationModel.Events
{
    /// <summary>
    /// Event of adding the new car call to the
    /// one of the elevators
    /// </summary>
    class NewCarcall : EventOf2<Tenant, Elevator>
    {
        public NewCarcall(int time, EventProvider provider, Tenant tenant, Elevator elevator)
            :base(time, provider, tenant, elevator)
        {
        }
        public override void Execute()
        {
            m_provider.ElevatorMoveControl(m_p2);

            m_p2.AddCarcall(m_p1);
        }
        public override string ToString()
        {
            return string.Format(
                "Car call {0} was added to elevator {1}",
                m_p1.FloorTo,
                m_p2.ID
                );
        }
    }
}