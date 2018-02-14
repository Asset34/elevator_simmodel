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

        /// <summary>
        /// 
        /// </summary>
        public abstract int Priority { get; }

        public Event(int time, ElevatorSM model)
        {
            Time = time;
            m_model = model;
        }
        public abstract void Execute();

        protected ElevatorSM m_model;
    }
}
