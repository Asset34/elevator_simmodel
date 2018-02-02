using ElevatorSimulation.SimulationModel.Transactions;
using ElevatorSimulation.SimulationModel.Entities;

namespace ElevatorSimulation.SimulationModel.Schedulers.CallSchedulers
{
    /// <summary>
    /// Base class for schedulers of floor calls of single elevator
    /// </summary>
    abstract class CallScheduler : Scheduler
    {
        public abstract void AddHallCall(Tenant tenant);
        public abstract void AddCarCall(Tenant tenant);
        public abstract void RemoveCall(int call);

        /// <summary>
        /// Get next floor request for handle by elevator
        /// </summary>
        /// <returns></returns>
        public abstract int GetCall(int floor);

        public abstract void SetElevator(Elevator elevator);

        protected Elevator m_elevator;
    }
}
