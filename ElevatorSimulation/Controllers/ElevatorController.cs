using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ElevatorSimulation.Transactions;
using ElevatorSimulation.Entities;
using ElevatorSimulation.Dispatchers.CollectiveDispatchers;

namespace ElevatorSimulation.Controllers
{
    /// <summary>
    /// Controller of the complex of elevators
    /// </summary>
    class ElevatorController : Resettable
    {
        public ElevatorController(CollectiveDispatcher dispatcher)
        {
            m_dispatcher = dispatcher;
        }
        public List<Elevator> Elevators
        {
            get { return m_elevators; }
        }
        
        public void Reset()
        {
            foreach (Elevator elevator in m_elevators)
            {
                elevator.Reset();
            }
        }

        private List<Elevator> m_elevators = new List<Elevator>();
        private readonly CollectiveDispatcher m_dispatcher;
    }
}
