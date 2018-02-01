using System;
using System.Collections.Generic;

using ElevatorSimulation.SimulationModel.Transactions;
using ElevatorSimulation.SimulationModel.Entities;

namespace ElevatorSimulation.SimulationModel.Dispatchers.RequestDispatchers
{
    /// <summary>
    /// Base class for dispatchers of floor requests of single elevator
    /// </summary>
    abstract class CallDispatcher : Resettable
    {
        public abstract void RegisterHallCall(Tenant tenant);
        public abstract void RegisterCarCall(Tenant tenant);
        public abstract void UnregisterCall(int call);
        /// <summary>
        /// Get next floor request for handle by elevator
        /// </summary>
        /// <returns></returns>
        public abstract Tuple<int, bool> GetCall(int floor);
        public abstract void SetElevator(Elevator elevator);
        public abstract void Reset();
    }
}
