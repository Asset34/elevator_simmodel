using System;
using System.Collections.Generic;
using System.Linq;

using ElevatorSimulation.SimulationModel.Transactions;
using ElevatorSimulation.SimulationModel.Entities;
using ElevatorSimulation.SimulationModel.Schedulers;

namespace ElevatorSimulation.SimulationModel.Controllers
{
    /// <summary>
    /// Controller of the complex of elevators
    /// </summary>
    class ElevatorsController
    {
        public ElevatorsController(ElevatorsScheduler scheduler)
        {
            m_scheduler = scheduler;
        }

        public void Add(Elevator elevator)
        {
            m_elevators.Add(elevator.ID, elevator);

            // Add to scheduler
            m_scheduler.Add(elevator);
        }
        public void Remove(Elevator elevator)
        {
            m_elevators.Remove(elevator.ID);

            // Remove from scheduler
            m_scheduler.Remove(elevator);
        }
        public Elevator Get(int id)
        {
            return m_elevators[id];
        }
        /// <summary>
        /// Get ID of the elevator which will handle
        /// the hallcall from tenant
        /// </summary>
        /// <param name="tenant"></param>
        /// <returns> ID of elevator </returns>
        public Elevator ScheduleElevator(Tenant tenant)
        {
            return m_scheduler.Schedule(tenant);
        }     

        public int[] GetIDs()
        {
            return m_elevators.Keys.ToArray();
        }

        public void Reset()
        {
            foreach (Elevator elevator in m_elevators.Values)
            {
                elevator.Reset();
            }
        }

        private Dictionary<int, Elevator> m_elevators = new Dictionary<int, Elevator>();
        private readonly ElevatorsScheduler m_scheduler;
    }
}
