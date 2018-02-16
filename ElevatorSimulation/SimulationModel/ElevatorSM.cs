using System;
using System.Collections.Generic;

using ElevatorSimulation.SimulationModel.Parameters;
using ElevatorSimulation.SimulationModel.Events;
using ElevatorSimulation.SimulationModel.Controllers;
using ElevatorSimulation.SimulationModel.Distributions;
using ElevatorSimulation.SimulationModel.Entities;
using ElevatorSimulation.SimulationModel.Schedulers;

namespace ElevatorSimulation.SimulationModel
{
    /// <summary>
    /// Simulation model of the elevators system
    /// in multi-storey building. This model performs queue
    /// networks and consists of:
    /// - 
    /// - 
    /// - 
    /// </summary>
    partial class ElevatorSM
    {
        public delegate void LogEventHandler(string msg);
        public event LogEventHandler Log;

        /// <summary>
        /// Current model time
        /// </summary>
        public int Time { get; private set; }

        /// <summary>
        /// Run the model
        /// </summary>
        public void Run(int Duration)
        {
            Log("*** Simulation started ***");

            Initialize();

            Event ev = null;
            DateTime time = new DateTime();
            while (Time <= Duration)
            {
                // Execute event
                if (ev != null)
                {
                    ev.Execute();
                    Log(String.Format("{0}   {1}", time.AddMinutes(ev.Time).TimeOfDay, ev.ToString()));
                }

                // Get nearest event
                ev = m_scheduler.Schedule();

                // Set new model time
                Time = ev.Time;
            }

            Log("*** Simulation finished ***");
        }
        /// <summary>
        /// Reset the model
        /// </summary>
        public void Reset()
        {
            Time = 0;

            m_generatorsController.Reset();
            m_queuesController.Reset();
            m_elevatorsController.Reset();

            m_scheduler.Reset();
        }

        /// <summary>
        /// Set starting events
        /// </summary>
        private void Initialize()
        {
            TenantGenerator generator;
            TenantQueue queue;
            foreach (int key in m_generatorsController.Floors)
            {
                generator = m_generatorsController.Get(key);
                queue = m_queuesController.Get(key);

                CreateEvent_NewTenant(generator, queue);
            }
        }

        /* Event create */
        public void CreateEvent_NewTenant(TenantGenerator generator, TenantQueue queue)
        {
            // Compute event occurence time
            int time = Time + m_generatorsDistr[queue.Floor].GetValue();

            m_scheduler.Add(new NewTenantEvent(time, this, generator, queue));
        }
        public void CreateEvent_ElevatorMove(Elevator elevator)
        {
            // Compute event occurence time
            int time = Time + m_elevatorsDistr[elevator.ID].GetValue();

            m_scheduler.Add(new ElevatorMoveEvent(time, this, elevator));
        }
        public void CreateEvent_NewHallcall(Tenant tenant)
        {
            // Get elevator from scheduler
            Elevator elevator = m_elevatorsController.ScheduleElevator(tenant);

            m_scheduler.Add(new NewHallcallEvent(Time, this, tenant, elevator));
        }
        public void CreateEvent_NewCarcall(Tenant tenant, Elevator elevator)
        {
            m_scheduler.Add(new NewCarcall(Time, this, tenant, elevator));
        }
        public void CreateEvent_Pickup(Elevator elevator)
        {
            // Get associated queue
            TenantQueue queue = m_queuesController.Get(elevator.CurrentFloor);

            if (queue.IsHallcall(elevator.Direction))
            {
                m_scheduler.Add(new PickupEvent(Time, this, queue, elevator));
            }       
        }
        public void CreateEvent_Dropoff(Elevator elevator)
        {
            if (elevator.IsDropOff)
            {
                m_scheduler.Add(new DropoffEvent(Time, this, elevator));
            } 
        }

        /* Controllers of subsystems */
        private TenantGeneratorsController m_generatorsController;
        private TenantQueuesController m_queuesController;
        private ElevatorsController m_elevatorsController;

        /* Distributions */
        private Dictionary<int, Distribution> m_generatorsDistr;
        private Dictionary<int, Distribution> m_elevatorsDistr;

        private EventsScheduler m_scheduler;
    }
}
