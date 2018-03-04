using ElevatorSimulation.Model.SimulationModel.Entities;

namespace ElevatorSimulation.Model.SimulationModel.Events
{
    /// <summary>
    /// Event of elevator movement
    /// </summary>
    class ElevatorMoveEvent : EventOf1<Elevator>
    {
        public override int Priority
        {
            get { return 1; }
        }

        public ElevatorMoveEvent(int time, ElevatorSimModel model, Elevator elevator)
            :base(time, model, elevator)
        {
        }
        public override void Execute()
        {
            m_p.Move();

            if (m_p.State == State.Wait)
            {
                m_model.CreateEvent_Dropoff(m_p);
                m_model.CreateEvent_Pickup(m_p);

            }
            else if (m_p.State == State.Move)
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
