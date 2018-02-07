using ElevatorSimulation.SimulationModel.Transactions;

using ElevatorSimulation.SimulationModel.Entities;

namespace ElevatorSimulation.SimulationModel.Events
{
    /// <summary>
    /// Event of adding the new hall call to the
    /// one of the elevators.
    /// </summary>
    class NewHallcallEvent : EventOf2<Tenant, Elevator>
    {
        public NewHallcallEvent(int time, EventProvider provider, Tenant tenant, Elevator elevator)
            :base(time, provider, tenant, elevator)
        {
        }
        public override void Execute()
        {
            m_provider.ElevatorMoveControl(m_p2);

            m_p2.AddHallcall(m_p1);
        }
        public override string ToString()
        {
            return string.Format(
                "Hall call {0}({1}) was added to elevator {2}",
                m_p1.FloorFrom,
                m_p1.CallType,
                m_p2.ID
                );
        }
    }
}
