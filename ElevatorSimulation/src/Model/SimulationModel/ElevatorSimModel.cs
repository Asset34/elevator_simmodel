using System;
using System.Collections.Generic;

using ElevatorSimulation.Model.SimulationModel.Events;
using ElevatorSimulation.Model.SimulationModel.Controllers;
using ElevatorSimulation.Model.SimulationModel.Distributions;
using ElevatorSimulation.Model.SimulationModel.Entities;
using ElevatorSimulation.Model.SimulationModel.Schedulers;
using ElevatorSimulation.Model.SimulationModel.Transactions;

namespace ElevatorSimulation.Model.SimulationModel
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
            ElevatorsController = new ElevatorsController(builder.NumFloors);
            foreach (Elevator elevator in builder.Elevators)
            {
                ElevatorsController.Add(elevator);
            }

            // Set distributions
            m_generatorsDistr = builder.GeneratorsDistr;
            m_elevatorsDistr = builder.ElevatorsDistr;

            // Set events scheduler
            m_scheduler = new EventsScheduler();

            m_isFinished = true;
        }

        /// <summary>
        /// Run the model
        /// </summary>
        public void Run(int duration)
        {
            Preprocess();

            Event ev = null;
            DateTime time = new DateTime();
            int endTime = Time + duration;

            while (Time <= endTime)
            {
                // Execute event
                if (ev != null)
                {
                    m_scheduler.Schedule();
                    ev.Execute();
                    Log(String.Format("{0}   {1}", time.AddMinutes(ev.Time).TimeOfDay, ev.ToString()));
                }

                // Get nearest event
                ev = m_scheduler.Peek();

                // Set new model time
                Time = ev.Time;
            }

            Postprocess();
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
        public void CreateEvent_ElevatorStartMove(Elevator elevator)
        {
            m_scheduler.Add(new ElevatorStartMoveEvent(Time, this, elevator));
        }
        public void CreateEvent_ElevatorStopMove(Elevator elevator)
        {
            m_scheduler.Add(new ElevatorStopMoveEvent(Time, this, elevator));
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

        public void Reset()
        {
            // Reset time
            Time = 0;

            // Reset model elements
            m_scheduler.Reset();
            GeneratorsController.Reset();
            QueuesController.Reset();
            ElevatorsController.Reset();
            
            // Finish the simulation if it was started
            if (!m_isFinished)
            {
                Log("\n *** Simulation finished *** \n");
                m_isFinished = true;
            }
        }

        private void Preprocess()
        {
            if (m_isFinished)
            {
                Log("\n *** Simulation started *** \n");

                // Initialize starting events
                TenantGenerator generator;
                TenantQueue queue;
                foreach (int key in GeneratorsController.Floors)
                {
                    generator = GeneratorsController.Get(key);
                    queue = QueuesController.Get(key);

                    CreateEvent_NewTenant(generator, queue);
                }

                // Force update statistics
                GeneratorsController.UpdateStatistics();
                QueuesController.UpdateStatistics();
                ElevatorsController.UpdateStatistics();

                m_isFinished = false;
            }
            else
            {
                Log("\n *** Simulation continued *** \n");
            }
        }
        private void Postprocess()
        {
            Log("\n *** Simulation stopped *** \n");

            // Force update statistics
            GeneratorsController.UpdateStatistics();
            QueuesController.UpdateStatistics();
            ElevatorsController.UpdateStatistics();
        }

        /* Distributions */
        private Dictionary<int, Distribution> m_generatorsDistr;
        private Dictionary<int, Distribution> m_elevatorsDistr;

        private bool m_isFinished;

        private EventsScheduler m_scheduler;
    }
}
