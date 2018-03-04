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
        /// Priority of the event which defines
        /// ....
        /// </summary>
        public abstract int Priority { get; }

        public Event(int time, ElevatorSimModel model)
        {
            Time = time;
            m_model = model;
        }
        public abstract void Execute();

        protected ElevatorSimModel m_model;
    }
}
