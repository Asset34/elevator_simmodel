using ElevatorSimulation.SimulationModel.Entities;

namespace ElevatorSimulation.SimulationModel.Events
{
    /// <summary>
    /// Event of elevator with step = 1 floor
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
                "Elevator {0} moved to the {1} floor",
                m_p.ID,
                m_p.CurrentFloor
                );
        }
    }
}
