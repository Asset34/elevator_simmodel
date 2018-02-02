﻿using System;
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
    class GroupElevatorController : Resettable
    {
        public List<Elevator> Elevators
        {
            get { return m_elevators; }
        }

        public GroupElevatorController(GlobalScheduler scheduler)
        {
            m_scheduler = scheduler;
        }
        public void Reset()
        {
            foreach (Elevator elevator in m_elevators)
            {
                elevator.Reset();
            }
        }

        private List<Elevator> m_elevators = new List<Elevator>();
        private readonly GlobalScheduler m_scheduler;
    }
}
