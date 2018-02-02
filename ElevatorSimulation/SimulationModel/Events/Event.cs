using ElevatorSimulation.SimulationModel.Controllers;

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
        public int Time { get; set; }

        public Event(int time, EventController handler)
        {
            Time = time;
            m_handler = handler;
        }
        public abstract void Execute();

        protected EventController m_handler;
    }
}
