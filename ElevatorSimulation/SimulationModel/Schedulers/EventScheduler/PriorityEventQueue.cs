using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ElevatorSimulation.SimulationModel.Events;

namespace ElevatorSimulation.SimulationModel.Schedulers
{
    partial class EventsScheduler
    {
        private class PriorityEventQueue
        {
            public int Count
            {
                get { return m_events.Count; }
            }

            public void Enqueue(Event ev)
            {
                m_events.Add(ev);
            }
            public Event Dequeue()
            {
                Event ev = Peek();
                m_events.Remove(ev);

                return ev;
            }
            public Event Peek()
            {
                return m_events.OrderByDescending(ev => ev.Priority).First();
            }
            public void Clear()
            {
                m_events.Clear();
            }

            private List<Event> m_events = new List<Event>();
        }
    }
}
