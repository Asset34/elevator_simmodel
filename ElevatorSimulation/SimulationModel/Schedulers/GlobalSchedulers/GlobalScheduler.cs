using ElevatorSimulation.SimulationModel.Entities;
using ElevatorSimulation.SimulationModel.Transactions;

namespace ElevatorSimulation.SimulationModel.Schedulers.GlobalSchedulers
{
    /// <summary>
    /// Base class for schedulers of group of elevators
    /// </summary>
    abstract class GlobalScheduler : Scheduler
    {
        public abstract void Add(Elevator elevator);
        public abstract void Remove(Elevator elevator);

        /// <summary>
        /// Get elevator which will handle the request
        /// </summary>
        /// <param name="tenant"></param>
        /// <returns>ID of elevator</returns>
        public abstract int GetElevator(Tenant tenant);
    }
}
