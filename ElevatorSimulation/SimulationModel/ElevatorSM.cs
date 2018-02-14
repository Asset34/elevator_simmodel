using System;
using System.Collections.Generic;

using ElevatorSimulation.SimulationModel.Parameters;
using ElevatorSimulation.SimulationModel.Events;
using ElevatorSimulation.SimulationModel.Controllers;
using ElevatorSimulation.SimulationModel.Distributions;
using ElevatorSimulation.SimulationModel.Entities;
using ElevatorSimulation.SimulationModel.Schedulers;
using ElevatorSimulation.SimulationModel.Transactions;

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

        public ElevatorSM(SimulationParameters parameters)
        {
            // Build group controllers
            m_generatorsController = BuildGeneratorsController(parameters.NumFloors);
            m_queuesController = BuildQueuesController(parameters.NumFloors);
            m_elevatorsController = BuildElevatorsController(
                parameters.NumFloors,
                parameters.NumElevators,
                parameters.ElevatorParameters
                );

            // Build distributions
            m_generatorsDistr = BuildGenerationDistributions(parameters.NumFloors, parameters.TenantGeneratorParameters);
            m_elevatorsDistr = BuildMovementDistributions(parameters.NumElevators, parameters.ElevatorParameters);
        }
        /// <summary>
        /// Run the model
        /// </summary>
        public void Run(int dTime)
        {
            Log("*** Simulation started ***");

            Initialize();

            Event ev = null;
            DateTime time = new DateTime();
            while (Time <= dTime)
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
            int[] floors1 = m_generatorsController.GetFloors();

            TenantGenerator generator;
            TenantQueue queue;
            foreach (int key in floors1)
            {
                generator = m_generatorsController.Get(key);
                queue = m_queuesController.Get(key);

                CreateEvent_NewTenant(generator, queue);
            }
        }

        /* Temprorary builders */
        // TODO: Replace all build methods to external 'Buidler'
        private TenantGeneratorsController BuildGeneratorsController(int numFloors)
        {
            TenantGeneratorsController controller = new TenantGeneratorsController();
            Distribution floorsDistribution = new UniformDistribution(1, numFloors);
            for (int i = 0; i < numFloors; i++)
            {
                controller.Add(new TenantGenerator(i + 1, floorsDistribution));
            }

            return controller;
        }
        private TenantQueuesController BuildQueuesController(int numFloors)
        {
            TenantQueuesController controller = new TenantQueuesController();
            for (int i = 0; i < numFloors; i++)
            {
                controller.Add(new TenantQueue(i + 1));
            }

            return controller;
        }
        private ElevatorsController BuildElevatorsController(
            int numFloors, 
            int numElevators, 
            ElevatorParameters[] parameters
            )
        {
            // Build global scheduler
            ElevatorsScheduler scheduler = new ElevatorsScheduler(numFloors);

            // Build controller
            ElevatorsController controller = new ElevatorsController(scheduler);
            Elevator elevator;
            for (int i = 0; i < numElevators; i++)
            {
                elevator = BuildElevator(i + 1, parameters[i]);
                controller.Add(elevator);
            }

            return controller;
        }
        private Elevator BuildElevator(int id, ElevatorParameters parameters)
        {
            // Build elevator
            return new Elevator(
                id,
                parameters.Capacity,
                parameters.StartFloor
                );
        }
        private Dictionary<int, Distribution> BuildGenerationDistributions(
            int numFloors, 
            TenantGeneratorParameters[] parameters
            )
        {
            Dictionary<int, Distribution> distributions = new Dictionary<int, Distribution>();
            int min, max;
            for (int i = 0; i < numFloors; i++)
            {
                min = parameters[i].Period - parameters[i].DPeriod;
                max = parameters[i].Period + parameters[i].DPeriod;

                distributions.Add(i + 1, new UniformDistribution(min, max));
            }

            return distributions;
        }
        private Dictionary<int, Distribution> BuildMovementDistributions(
            int numElevators, 
            ElevatorParameters[] parameters
            )
        {
            Dictionary<int, Distribution> distributions = new Dictionary<int, Distribution>();
            int min, max;
            for (int i = 0; i < numElevators; i++)
            {
                min = parameters[i].Period - parameters[i].DPeriod;
                max = parameters[i].Period + parameters[i].DPeriod;

                distributions.Add(i + 1, new UniformDistribution(min, max));
            }

            return distributions;
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
        private readonly TenantGeneratorsController m_generatorsController;
        private readonly TenantQueuesController m_queuesController;
        private readonly ElevatorsController m_elevatorsController;

        /* Distributions */
        private Dictionary<int, Distribution> m_generatorsDistr;
        private Dictionary<int, Distribution> m_elevatorsDistr;

        private readonly EventsScheduler m_scheduler = new EventsScheduler();
    }
}
