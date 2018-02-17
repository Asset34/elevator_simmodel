namespace ElevatorSimulation.SimulationModel.Events
{
    /// <summary>
    /// Base class for events with one participant
    /// </summary>
    /// <typeparam name="T"> Participant </typeparam>
    abstract class EventOf1<T> : Event
    {
        public EventOf1(int time, ElevatorSimModel model, T p)
            : base(time, model)
        {
            m_p = p;
        }

        protected readonly T m_p;
    }
}
