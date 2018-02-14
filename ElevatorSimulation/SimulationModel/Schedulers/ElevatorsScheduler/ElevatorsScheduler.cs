using ElevatorSimulation.SimulationModel.Entities;

namespace ElevatorSimulation.SimulationModel.Schedulers
{
    /// <summary>
    /// Base class for schedulers of elevators
    /// </summary>
    abstract class ElevatorsScheduler
    {
        public abstract bool IsEmpty { get; }

        public abstract void Add(Elevator element);
        public abstract void Remove(Elevator element);
        public abstract Elevator Schedule(Tenant tenant);

        public abstract void Reset();
    }
}
