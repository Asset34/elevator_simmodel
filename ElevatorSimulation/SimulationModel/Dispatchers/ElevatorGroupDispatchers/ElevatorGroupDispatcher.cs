using System;
using ElevatorSimulation.SimulationModel.Entities;
using ElevatorSimulation.SimulationModel.Transactions;

namespace ElevatorSimulation.SimulationModel.Dispatchers.ElevatorGroupDispatchers
{
    /// <summary>
    /// Base class for dispatchers of group of elevators
    /// </summary>
    abstract class ElevatorGroupDispatcher : Resettable
    {
        public abstract void Register(Elevator elevator);
        public abstract void Unregister(Elevator elevator);
        /// <summary>
        /// Get elevator which will handle the request
        /// </summary>
        /// <param name="tenant"></param>
        /// <returns></returns>
        public abstract Elevator GetElevator(Tenant tenant);
        public abstract void Reset();
    }
}
