using System;
using System.Collections.Generic;

using ElevatorSimulation.Events;

namespace ElevatorSimulation
{
    /// <summary>
    /// Simple event comparer class by occurence time
    /// </summary>
    class EventComparer : IComparer<Event>
    {
        public int Compare(Event x, Event y)
        {
            return x.Time.CompareTo(y.Time);
        }
    }
}
