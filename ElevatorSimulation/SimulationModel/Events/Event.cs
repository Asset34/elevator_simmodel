namespace ElevatorSimulation.SimulationModel.Events
{
    /// <summary>
    /// Base class for events
    /// </summary>
    abstract class Event
    {
        /// <summary>
        /// Occurrence time
        /// </summary>
        public int Time { get; }

        public Event(int time, EventProvider provider)
        {
            Time = time;
            m_provider = provider;
        }
        public abstract void Execute();

        protected EventProvider m_provider;
    }
}
