using ElevatorSimulation.Model.SimulationModel.Entities;

namespace ElevatorSimulation.Model.SimulationModel.Events
{
    /// <summary>
    /// Event of elevator movement
    /// </summary>
    class ElevatorMoveEvent : EventOf1<Elevator>
    {
        public ElevatorMoveEvent(int time, ElevatorSimModel model, Elevator elevator)
            :base(time, model, elevator)
        {
            Priority = 2;
        }
        public override void Execute()
        {
            m_p.Move();

            if (m_p.State == State.Wait || m_p.State == State.Idle)
            {
                m_model.CreateEvent_ElevatorStopMove(m_p);
            }
            else
            {
                m_model.CreateEvent_ElevatorMove(m_p);
            }
        }
        public override string ToString()
        {
            return string.Format(
                "Elevator[id:{0}] - move[floor:{1}]",
                m_p.ID,
                m_p.CurrentFloor
                );
        }
    }
}
