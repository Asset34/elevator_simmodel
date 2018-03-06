using ElevatorSimulation.Model.SimulationModel.Entities;

namespace ElevatorSimulation.Model.SimulationModel.Events
{
    class ElevatorStartMoveEvent : EventOf1<Elevator>
    {
        public int Floor { get; private set; }

        public ElevatorStartMoveEvent(int time, ElevatorSimModel model, Elevator p)
            : base(time, model, p)
        {
            Priority = 1;
        }
        public override void Execute()
        {
            Floor = m_p.CurrentFloor;

            m_p.Next();

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
                "Elevator[id:{0}] - start move[floor:{1}]",
                m_p.ID,
                Floor
                );
        }
    }
}
