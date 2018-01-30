using System;
using System.Collections.Generic;

using ElevatorSimulation.Entities;

namespace ElevatorSimulation.Dispatchers.CollectiveDispatchers
{
    /// <summary>
    /// Base class for dispatchers of group of elevators
    /// </summary>
    abstract class CollectiveDispatcher : Dispatcher<Elevator>
    {
        /// <summary>
        /// Get elevator which will handle the request
        /// </summary>
        /// <param name="floor"></param>
        /// <returns></returns>
        public abstract Elevator GetElevator(Request request);
    }
}
