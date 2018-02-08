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
    class ElevatorSimulationModel : Resettable
    {
        public delegate void LogEventHandler(string text);

        public event LogEventHandler Log;

        /// <summary>
        /// Current model time
        /// </summary>
        public int Time { get; private set; }

        /* Controllers of subsystems */
        public TenantGeneratorsController GeneratorsController { get; }
        public TenantQueuesController QueuesController { get; }
        public ElevatorsController ElevatorsController { get; }

        public ElevatorSimulationModel(SimulationParameters parameters)
        {
            // Build group controllers
            GeneratorsController = BuildGeneratorsController(parameters.NumFloors);
            QueuesController = BuildQueuesController(parameters.NumFloors);
            ElevatorsController = BuildElevatorsController(
                parameters.NumFloors,
                parameters.NumElevators,
                parameters.ElevatorParameters
                );

            // Build distributions
            Dictionary<int, Distribution> generationDistributions
                = BuildGenerationDistributions(parameters.NumFloors, parameters.TenantGeneratorParameters);
            Dictionary<int, Distribution> movementDistributions
                = BuildMovementDistributions(parameters.NumElevators, parameters.ElevatorParameters);

            // Build event provider
            m_eventProvider = new EventProvider(
                this,
                generationDistributions,
                movementDistributions
                );
        }
        /// <summary>
        /// Run the model
        /// </summary>
        public void Run(int dTime)
        {
            // Initialize
            m_eventProvider.Initialize();

            Event ev;
            while (Time <= dTime)
            {
                // Get nearest event
                ev = m_eventProvider.GetEvent();

                // Set new model time
                Time = ev.Time;

                // Execute event
                ev.Execute();

                // Log
                Log(ev.ToString());
            }
        }
        /// <summary>
        /// Reset the model
        /// </summary>
        public void Reset()
        {
            GeneratorsController.Reset();
            QueuesController.Reset();
            ElevatorsController.Reset();

            m_eventProvider.Reset();
        }

        /* Temprorary builders */
        // NEED: Replace all build methods to external 'Buidler'
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

        private readonly EventProvider m_eventProvider;
    }
}
