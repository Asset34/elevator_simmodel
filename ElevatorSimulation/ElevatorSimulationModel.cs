using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ElevatorSimulation.Events;
using ElevatorSimulation.Transactions;

namespace ElevatorSimulation
{
    /// <summary>
    /// "Singleton" simulation model of the elevators system
    /// in multi-storey building. This model performs queue
    /// networks and consists of:
    /// - 
    /// - 
    /// - 
    /// </summary>
    class ElevatorSimulationModel
    {
        public static ElevatorSimulationModel Instance
        {
            get
            {
                if (m_instance == null)
                {
                    throw new NullReferenceException("An attempt to get instance of uncreated simulation model");
                }

                return m_instance;
            }
        }

        /// <summary>
        /// Current model time
        /// </summary>
        public int Time { get; }
        /// <summary>
        /// Current events set ordered by occurence time
        /// </summary>
        public SortedSet<Event> Events
        {
            get { return m_events; }
        }

        /// <summary>
        /// Run the model
        /// </summary>
        public void Run()
        {
            // TODO
        }
        /// <summary>
        /// Reset the model
        /// </summary>
        public void Reset()
        {
            // TODO
        }

        private ElevatorSimulationModel(SimulationParameters parameters)
        {
            BuildModel(parameters);
        }
        private void BuildModel(SimulationParameters parameters)
        {

        }
        private void initializeEvents()
        {
            // TODO
        }
        private Event GetNearestEvent()
        {
            Event temp = m_events.First();
            m_events.Remove(temp);
            return temp;
        }

        /* Event handlers */
        private void NewTenantEvent_Handle(Tenant tenant)
        {
            // TODO
        }

        private SortedSet<Event> m_events = new SortedSet<Event>(new EventComparer());
     
        private static ElevatorSimulationModel m_instance;
    }
}
