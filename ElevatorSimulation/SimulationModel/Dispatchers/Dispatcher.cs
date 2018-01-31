namespace ElevatorSimulation.SimulationModel.Dispatchers
{
    /// <summary>
    /// Base generic class for dispatchers
    /// </summary>
    /// <typeparam name="T"></typeparam>
    abstract class Dispatcher<T> : Resettable
    {
        /// <summary>
        /// Add element to dispatch system
        /// </summary>
        /// <param name="element"></param>
        public abstract void Register(T element);
        /// <summary>
        /// Remove element from dispatch system
        /// </summary>
        /// <param name="element"></param>
        public abstract void Unregister(T element);
        public abstract void Reset();
    }
}
