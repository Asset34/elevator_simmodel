using System;
using System.Collections.Generic;
using System.Linq;

using ElevatorSimulation.SimulationModel.Events;

namespace ElevatorSimulation.SimulationModel.Schedulers
{
    /// <summary>
    /// Scheduler which distributes and orders events
    /// by the model time
    /// </summary>
    partial class EventsScheduler
    {
        public bool IsEmpty
        {
            get { return m_events.Count == 0; }
        }

        public void Add(Event ev)
        {
            if (!m_events.ContainsKey(ev.Time))
            {
                m_events.Add(ev.Time, new PriorityEventQueue());
            }

            m_events[ev.Time].Enqueue(ev);
        }
        public Event Remove(int time)
        {
            Event ev = m_events[time].Dequeue();

            if (m_events[time].Count == 0)
            {
                m_events.Remove(time);
            }

            return ev;
        }
        public Event Schedule()
        {
            // Check
            if (IsEmpty)
            {
                throw new InvalidOperationException("No events to schedule");
            }

            return Remove(m_events.Keys.Min());
        }

        public void Reset()
        {
            m_events.Clear();
        }

        private Dictionary<int, PriorityEventQueue> m_events = new Dictionary<int, PriorityEventQueue>();
    }
}
