using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ElevatorSimulation.SimulationModel.Transactions;
using ElevatorSimulation.SimulationModel.Entities;
using ElevatorSimulation.SimulationModel.Schedulers.GlobalSchedulers;

namespace ElevatorSimulation.SimulationModel.Controllers
{
    /// <summary>
    /// Controller of the complex of elevators
    /// </summary>
    class ElevatorsController : Controller
    {
        public Dictionary<int, Elevator> Elevators
        {
            get { return m_elevators; }
        }

        public ElevatorsController(GlobalScheduler scheduler)
        {
            m_scheduler = scheduler;
        }
        /// <summary>
        /// Get ID of the elevator which will handle
        /// the hallcall from tenant
        /// </summary>
        /// <param name="tenant"></param>
        /// <returns> ID of elevator </returns>
        public int SelectElevator(Tenant tenant)
        {
            return m_scheduler.GetElevator(tenant);
        }     

        public override void Reset()
        {
            foreach (Elevator elevator in m_elevators.Values)
            {
                elevator.Reset();
            }
        }

        private Dictionary<int, Elevator> m_elevators = new Dictionary<int, Elevator>();
        private readonly GlobalScheduler m_scheduler;
    }
}
