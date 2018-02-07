﻿using System;
using System.Collections.Generic;

using ElevatorSimulation.SimulationModel.Events;
using ElevatorSimulation.SimulationModel.Distributions;
using ElevatorSimulation.SimulationModel.Transactions;
using ElevatorSimulation.SimulationModel.Entities;
using ElevatorSimulation.SimulationModel.Schedulers;

namespace ElevatorSimulation.SimulationModel
{
    /// <summary>
    /// 
    /// </summary>
    class EventProvider
    {
        public EventProvider(
            ElevatorSimulationModel model,
            Dictionary<int, Distribution> generatorsDistr,
            Dictionary<int, Distribution> elevatorsDistr
            )
        {
            m_model = model;

            m_generatorsDistr = generatorsDistr;
            m_elevatorsDistr = elevatorsDistr;
        }

        /// <summary>
        /// Set starting events
        /// </summary>
        public void Initialize()
        {
            Dictionary<int, TenantGenerator> generators = m_model.GeneratorsController.Generators;
            Dictionary<int, TenantQueue> queues = m_model.QueuesController.Queues;
            
            foreach (int key in generators.Keys)
            {
                CreateEvent_NewTenant(generators[key], queues[key]);
            }
        }
        /// <summary>
        /// Get the nearest event
        /// </summary>
        /// <returns></returns>
        public Event GetEvent()
        {
            return m_scheduler.Schedule();
        }

        /* Event create */
        public void CreateEvent_NewTenant(TenantGenerator generator, TenantQueue queue)
        {
            // Compute event occurence time
            int time = m_model.Time + m_generatorsDistr[queue.Floor].GetValue();
            
            // Create event
            Event newEv = new NewTenantEvent(time, this, generator, queue);

            m_scheduler.Add(newEv);
        }
        public void CreateEvent_ElevatorMove(Elevator elevator)
        {
            // Compute event occurence time
            int time = m_model.Time + m_elevatorsDistr[elevator.ID].GetValue();

            // Create event
            Event newEv = new ElevatorMoveEvent(time, this, elevator);

            m_scheduler.Add(newEv);
        }
        public void CreateEvent_NewHallcall(Tenant tenant)
        {
            // Compute event occurence time
            int time = m_model.Time;

            // Get elevator from scheduler
            Elevator elevator = m_model.ElevatorsController.ScheduleElevator(tenant);

            // Create event
            Event newEv = new NewHallcallEvent(time, this, tenant, elevator);

            m_scheduler.Add(newEv);
        }
        public void CreateEvent_NewCarcall(Tenant tenant, Elevator elevator)
        {
            // Compute event occurence time
            int time = m_model.Time;

            // Create event
            Event newEv = new NewCarcall(time, this, tenant, elevator);

            m_scheduler.Add(newEv);
        }
        public void CreateEvent_Pickup(Elevator elevator)
        {
            // Compute event occurencet ime
            int time = m_model.Time;

            // Get associated queue
            TenantQueue queue = m_model.QueuesController.Queues[elevator.CurrentFloor];

            // Create event
            Event newEv = new PickupEvent(time, this, queue, elevator);

            m_scheduler.Add(newEv);
        }
        public void CreateEvent_Dropoff(Elevator elevator)
        {
            // Compute event occurencet ime
            int time = m_model.Time;

            // Create event
            Event newEv = new DropoffEvent(time, this, elevator);

            m_scheduler.Add(newEv);
        }

        /* Additional */
        public void ElevatorMoveControl(Elevator elevator)
        {
            if (elevator.IsReached)
            {
                // Remove handled call
                elevator.RemoveCall(elevator.CurrentFloor);

                // Set next call
                if (!elevator.IsIdle)
                {
                    elevator.SetCall();
                }

                // Try to pick up
                PickupControl(elevator);
                
                CreateEvent_Dropoff(elevator);
            }
            // Move next
            else if (!elevator.IsIdle)
            {
                CreateEvent_ElevatorMove(elevator);
            }
        }
        public void PickupControl(Elevator elevator)
        {
            // Get associated queue
            TenantQueue queue = m_model.QueuesController.Queues[elevator.CurrentFloor];

            if (elevator.FreeCount > 0 &&
                queue.GetHallcallStatus(elevator.CurrentCallType))
            {
                CreateEvent_Pickup(elevator);
            }
        }

        public void Reset()
        {
            m_scheduler.Reset();
        }

        /* Distributions */
        private readonly Dictionary<int, Distribution> m_generatorsDistr;
        private readonly Dictionary<int, Distribution> m_elevatorsDistr;

        private readonly ElevatorSimulationModel m_model;
        private readonly EventsScheduler m_scheduler = new EventsScheduler();
    }
}