using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ElevatorSimulation.SimulationModel.Events;
using ElevatorSimulation.SimulationModel.Distributions;

namespace ElevatorSimulation.SimulationModel.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    class EventController : Controller
    {
        public EventController(
            TenantGeneratorsController generatorsController,
            TenantQueuesController queuesController,
            ElevatorsController elevatorsController,
            Distribution[] generationDistributions,
            Distribution[] movementDistributions)
        {
            m_generatorsController = generatorsController;
            m_queuesController = queuesController;
            m_elevatorsController = elevatorsController;

            m_generationDistributions = generationDistributions;
            m_movementDistributions = movementDistributions;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Event GetNextEvent()
        {
            int min = m_events.Min(x => x.Time);
            return m_events.Where(x => x.Time == min).First();
        }

        public override void Reset()
        {
            m_generatorsController.Reset();
            m_queuesController.Reset();
            m_elevatorsController.Reset();

            m_events.Clear();
        }



        /* Events creation */

        /* Events handle */

        private List<Event> m_events = new List<Event>();

        /* Controllers */
        private readonly TenantGeneratorsController m_generatorsController;
        private readonly TenantQueuesController m_queuesController;
        private readonly ElevatorsController m_elevatorsController;

        /* Distributions */
        private readonly Distribution[] m_generationDistributions;
        private readonly Distribution[] m_movementDistributions;
    }
}
