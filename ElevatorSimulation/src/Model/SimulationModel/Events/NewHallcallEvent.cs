using ElevatorSimulation.Model.SimulationModel.Entities;
using ElevatorSimulation.Model.SimulationModel.Transactions;

namespace ElevatorSimulation.Model.SimulationModel.Events
{
    /// <summary>
    /// Event of adding the new hall call to the elevator
    /// </summary>
    class NewHallcallEvent : EventOf2<Tenant, Elevator>
    {
        public NewHallcallEvent(int time, ElevatorSimModel model, Tenant tenant, Elevator elevator)
            :base(time, model, tenant, elevator)
        {
            Priority = 4;
        }
        public override void Execute()
        {
            if (m_p2.State == State.Idle)
            {
                m_model.CreateEvent_ElevatorStartMove(m_p2);
            }

            m_p2.AddHallcall(m_p1);
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
