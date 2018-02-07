using System;
using System.Collections.Generic;
using System.Linq;

using ElevatorSimulation.SimulationModel.Events;

namespace ElevatorSimulation.SimulationModel.Schedulers
{
    /// <summary>
    /// Schedulers of simulatuon model's events
    /// </summary>
    class EventsScheduler
    {
        public void Add(Event ev)
        {
            if (!m_events.ContainsKey(ev.Time))
            {
                m_events.Add(ev.Time, new Queue<Event>());
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
            return Remove(m_events.Min().Key);
        }

        public void Reset()
        {
            m_events.Clear();
        }

        private Dictionary<int, Queue<Event>> m_events = new Dictionary<int, Queue<Event>>();
    }
}
