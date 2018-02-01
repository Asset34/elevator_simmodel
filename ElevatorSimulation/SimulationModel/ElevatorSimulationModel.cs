using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ElevatorSimulation.SimulationModel.Parameters;
using ElevatorSimulation.SimulationModel.Events;
using ElevatorSimulation.SimulationModel.Controllers;
using ElevatorSimulation.SimulationModel.Transactions;
using ElevatorSimulation.SimulationModel.Distributions;
using ElevatorSimulation.SimulationModel.Entities;

namespace ElevatorSimulation
{
    /// <summary>
    /// Simulation model of the elevators system
    /// in multi-storey building. This model performs queue
    /// networks and consists of:
    /// - 
    /// - 
    /// - 
    /// </summary>
    class ElevatorSimulationModel : Resettable
    {
        /// <summary>
        /// Current model time
        /// </summary>
        public int Time { get; }
        /// <summary>
        /// Current events set ordered by occurence time
        /// </summary>
        public List<Event> Events
        {
            get { return m_events; }
        }

        public ElevatorSimulationModel(SimulationParameters parameters)
        {
                               
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
            // Reset events
            m_events.Clear();

            // Reset controllers
            m_floorQueueController.Reset();
            m_elevatorController.Reset();
        }

        private List<Event> m_events = new List<Event>();
        /* Distributions */
        private readonly Distribution[] m_generationDistributions;
        private readonly Distribution[] m_movementDistributions;
        /* Controllers */
        private readonly GroupTenantGeneratorController m_tenantGeneratorController;
        private readonly GroupFloorQueueController m_floorQueueController;
        private readonly GroupElevatorController m_elevatorController;
    }
}
