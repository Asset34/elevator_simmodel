namespace ElevatorSimulation.Model.SimulationModel.Events
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
        /// Priority of the event
        /// </summary>
        public int Priority { get; protected set; }

        public Event(int time, ElevatorSimModel model)
        {
            Time = time;
            m_model = model;
        }
        public abstract void Execute();

        protected ElevatorSimModel m_model;
    }
}
