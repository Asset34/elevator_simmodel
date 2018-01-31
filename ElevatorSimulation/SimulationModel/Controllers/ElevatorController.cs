using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ElevatorSimulation.SimulationModel.Transactions;
using ElevatorSimulation.SimulationModel.Entities;
using ElevatorSimulation.SimulationModel.Dispatchers.ElevatorGroupDispatchers;

namespace ElevatorSimulation.SimulationModel.Controllers
{
    /// <summary>
    /// Controller of the complex of elevators
    /// </summary>
    class ElevatorController : Resettable
    {
        public ElevatorController(ElevatorGroupDispatcher dispatcher)
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
        private readonly ElevatorGroupDispatcher m_dispatcher;
    }
}
