using ElevatorSimulation.SimulationModel.Entities;

namespace ElevatorSimulation.SimulationModel.Events
{
    /// <summary>
    /// Event of elevator movement with step = 1 floor
    /// </summary>
    class ElevatorMoveEvent : EventOf1<Elevator>
    {
        public ElevatorMoveEvent(int time, EventProvider provider, Elevator elevator)
            :base(time, provider, elevator)
        {
        }
        public override void Execute()
        {
            m_p.Move();

            // Create event of new move
            m_provider.ElevatorMoveControl(m_p);
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
