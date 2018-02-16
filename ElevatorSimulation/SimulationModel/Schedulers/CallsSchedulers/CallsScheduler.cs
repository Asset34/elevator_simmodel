using ElevatorSimulation.SimulationModel.Entities;

namespace ElevatorSimulation.SimulationModel.Schedulers
{
    /// <summary>
    /// Base class for schedulers of calls which
    /// defines which next call will be handled
    /// </summary>
    abstract class CallsScheduler
    {
        /// <summary>
        /// Flag, wch defines the next scheduled call 
        /// represents the border floor
        /// </summary>
        public bool IsBorder { get; protected set; }

        public abstract bool IsEmpty { get; }

        public void SetElevator(Elevator elevator)
        {
            m_elevator = elevator;
        }
        
        public abstract void AddHallcall(Tenant tenant);
        public abstract void AddCarcall(Tenant tenant);
        public abstract void RemoveCall(int call);

        public abstract int Schedule();

        public abstract void Reset();

        protected Elevator m_elevator;
    }
}
