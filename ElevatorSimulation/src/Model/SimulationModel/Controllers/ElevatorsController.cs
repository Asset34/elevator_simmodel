﻿using System;
using System.Collections.Generic;
using System.Linq;

using ElevatorSimulation.Model.SimulationModel.Entities;
using ElevatorSimulation.Model.SimulationModel.Schedulers;
using ElevatorSimulation.Model.SimulationModel.Transactions;
    
namespace ElevatorSimulation.Model.SimulationModel.Controllers
{
    /// <summary>
    /// Controller of the complex of elevators which
    /// performs subsystem 'Service devices' of the
    /// queueing theory
    /// </summary>
    class ElevatorsController
    {
        /// <summary>
        /// Identification numbers of all elevators
        /// of the simulation model
        /// </summary>
        public int[] IDs
        {
            get { return m_elevators.Keys.ToArray(); }
        }

        public ElevatorsController(int numFloors)
        {
            m_scheduler = new ElevatorsScheduler(numFloors);
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

        public void UpdateStatistics()
        {
            foreach (Elevator elevator in m_elevators.Values)
            {
                elevator.OnChanged();
            }
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
