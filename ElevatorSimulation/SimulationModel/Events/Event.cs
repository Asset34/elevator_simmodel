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

        public abstract void Execute();
    }
}
