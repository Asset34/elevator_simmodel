using System;
using System.Collections.Generic;

using ElevatorSimulation.SimulationModel.Entities;

namespace ElevatorSimulation.SimulationModel.Dispatchers.ElevatorGroupDispatchers
{
    /// <summary>
    /// Base class for dispatchers of group of elevators
    /// </summary>
    abstract class ElevatorGroupDispatcher : Dispatcher<Elevator>
    {
        /// <summary>
        /// Get elevator which will handle the request
        /// </summary>
        /// <param name="floor"></param>
        /// <returns></returns>
        public abstract Elevator GetElevator(Request request);
    }
}
