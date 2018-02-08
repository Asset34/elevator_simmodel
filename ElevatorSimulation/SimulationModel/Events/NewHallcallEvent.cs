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
            bool flag = false;

            if (m_p2.IsIdle)
            {
                flag = true;
            }

            m_p2.AddHallcall(m_p1);

            if (flag)
            {
                m_provider.ElevatorMoveControl(m_p2);
            }
        }
        public override string ToString()
        {
            return string.Format(
                "Elevator[id:{0}] - add hallcall[floor:{1}; type:{2}]",
                m_p2.ID,
                m_p1.FloorFrom,
                m_p1.Direction
                );
        }
    }
}
