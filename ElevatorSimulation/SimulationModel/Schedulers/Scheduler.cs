namespace ElevatorSimulation.SimulationModel.Schedulers
{
    /// <summary>
    /// Base class for Schedulers
    /// </summary>
    abstract class Scheduler : Resettable
    {
        /// <summary>
        /// Flag which defines the ability to schedule
        /// </summary>
        public abstract bool IsEmpty { get; }

        public abstract void Reset();
    }
}
