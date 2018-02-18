using System;
using System.Collections.Generic;

using ElevatorSimulation.SimulationModel.Events;
using ElevatorSimulation.SimulationModel.Controllers;
using ElevatorSimulation.SimulationModel.Distributions;
using ElevatorSimulation.SimulationModel.Entities;
using ElevatorSimulation.SimulationModel.Schedulers;
using ElevatorSimulation.SimulationModel.Transactions;

namespace ElevatorSimulation.SimulationModel
{
    /// <summary>
    /// Simulation model of system of elevators
    /// </summary>
    class ElevatorSimModel
    {
        public delegate void LogEventHandler(string msg);
        public event LogEventHandler Log;

        /// <summary>
        /// Current model time
        /// </summary>
        public int Time { get; private set; }

        /* Controllers of subsystems */
        public TenantGeneratorsController GeneratorsController { get; }
        public TenantQueuesController QueuesController { get; }
        public ElevatorsController ElevatorsController { get; }

        public ElevatorSimModel(ElevatorSimModelBuilder builder)
        {
            // Build subsystem of transaction sources
            GeneratorsController = new TenantGeneratorsController();
            foreach (TenantGenerator generator in builder.Generators)
            {
                GeneratorsController.Add(generator);
            }

            // Build subsystem of transaction queues
            QueuesController = new TenantQueuesController();
            foreach (TenantQueue queue in builder.Queues)
            {
                QueuesController.Add(queue);
            }

            // Build subsystem of service devices
            ElevatorsController = new ElevatorsController(builder.Scheduler);
            foreach (Elevator elevator in builder.Elevators)
            {
                ElevatorsController.Add(elevator);
            }

            // Set distributions
            m_generatorsDistr = builder.GeneratorsDistr;
            m_elevatorsDistr = builder.ElevatorsDistr;

            // Set events scheduler
            m_scheduler = new EventsScheduler();
        }

        /// <summary>
        /// Run the model
        /// </summary>
        public void Run(int duration)
        {
            Log("*** Simulation started ***");

            Initialize();

            Event ev = null;
            DateTime time = new DateTime();
            while (Time <= duration)
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
            Time = duration;

            Finalize();

            Log("*** Simulation finished ***");
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
            Elevator elevator = ElevatorsController.ScheduleElevator(tenant);

            m_scheduler.Add(new NewHallcallEvent(Time, this, tenant, elevator));
        }
        public void CreateEvent_NewCarcall(Tenant tenant, Elevator elevator)
        {
            m_scheduler.Add(new NewCarcall(Time, this, tenant, elevator));
        }
        public void CreateEvent_Pickup(Elevator elevator)
        {
            // Get associated queue
            TenantQueue queue = QueuesController.Get(elevator.CurrentFloor);

            if (queue.IsHallcall(elevator.Direction) && 
                elevator.FreePlace > 0)
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

        private void Initialize()
        {
            // Set starting time
            Time = 0;

            UpdateStatistics();

            // initialize events
            TenantGenerator generator;
            TenantQueue queue;
            foreach (int key in GeneratorsController.Floors)
            {
                generator = GeneratorsController.Get(key);
                queue = QueuesController.Get(key);

                CreateEvent_NewTenant(generator, queue);
            }
        }
        private void Finalize()
        {
            UpdateStatistics();
        }

        /// <summary>
        /// Force update all statistics linked to all entities in all subsystems
        /// </summary>
        private void UpdateStatistics()
        {
            GeneratorsController.UpdateStatistics();
            QueuesController.UpdateStatistics();
            ElevatorsController.UpdateStatistics();
        }

        /* Distributions */
        private Dictionary<int, Distribution> m_generatorsDistr;
        private Dictionary<int, Distribution> m_elevatorsDistr;

        private EventsScheduler m_scheduler;
    }
}
