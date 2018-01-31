using System;
using System.Collections.Generic;

using ElevatorSimulation.SimulationModel.Entities;

namespace ElevatorSimulation.SimulationModel.Dispatchers.RequestDispatchers
{
    /// <summary>
    /// Base class for dispatchers of floor requests of single elevator
    /// </summary>
    abstract class RequestDispatcher : Dispatcher<Request>
    {
        /// <summary>
        /// Get next floor request for handle by elevator
        /// </summary>
        /// <returns></returns>
        public abstract int GetRequest();
    }
}
