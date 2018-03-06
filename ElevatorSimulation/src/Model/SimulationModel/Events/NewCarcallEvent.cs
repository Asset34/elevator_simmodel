using ElevatorSimulation.Model.SimulationModel.Entities;
using ElevatorSimulation.Model.SimulationModel.Transactions;

namespace ElevatorSimulation.Model.SimulationModel.Events
{
    /// <summary>
    /// Event of adding the new car call to the elevatorы
    /// </summary>
    class NewCarcall : EventOf2<Tenant, Elevator>
    {
        public NewCarcall(int time, ElevatorSimModel model, Tenant tenant, Elevator elevator)
            :base(time, model, tenant, elevator)
        {
            Priority = 5;
        }
        public override void Execute()
        {
            if (m_p2.State == State.Idle)
            {
                m_model.CreateEvent_ElevatorStartMove(m_p2);
            }

            m_p2.AddCarcall(m_p1);
        }
        public override string ToString()
        {
            return string.Format(
                "Elevator[id:{0}] - add carcall[floor:{1}]",
                m_p2.ID,
                m_p1.FloorTo               
                );
        }
    }
}
