using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ElevatorSimulation.SimulationModel.Parameters;
using ElevatorSimulation.SimulationModel.Events;
using ElevatorSimulation.SimulationModel.Controllers;
using ElevatorSimulation.SimulationModel.Transactions;
using ElevatorSimulation.SimulationModel.Distributions;
using ElevatorSimulation.SimulationModel.Entities;
using ElevatorSimulation.SimulationModel.Schedulers.CallSchedulers;
using ElevatorSimulation.SimulationModel.Schedulers.GlobalSchedulers;

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
        /// <summary>
        /// Current model time
        /// </summary>
        public int Time { get; }
        /// <summary>
        /// Current events set ordered by occurence time
        /// </summary>
        public List<Event> Events
        {
            get { return m_events; }
        }

        public ElevatorSimulationModel(SimulationParameters parameters)
        {
            // Build group controllers
            TenantGeneratorsController generatorsController = BuildGeneratorsController(parameters.NumFloors);
            TenantQueuesController queuesController = BuildQueuesController(parameters.NumFloors);
            ElevatorsController elevatorsController = BuildElevatorsController(
                parameters.NumFloors,
                parameters.NumElevators,
                parameters.ElevatorParameters);

            // Build distributions
            Distribution[] generationDistributions
                = BuildGenerationDistributions(parameters.NumFloors, parameters.TenantGeneratorParameters);
            Distribution[] movementDistributions
                = BuildMovementDistributions(parameters.NumElevators, parameters.ElevatorParameters);

            // Build event controller
            m_eventController = new EventController(
                generatorsController,
                queuesController,
                elevatorsController,
                generationDistributions,
                movementDistributions
                );
        }
        /// <summary>
        /// Run the model
        /// </summary>
        public void Run()
        {
            // TODO
        }
        /// <summary>
        /// Reset the model
        /// </summary>
        public void Reset()
        {
            // Reset events
            m_events.Clear();

        }

        private TenantGeneratorsController BuildGeneratorsController(int numFloors)
        {
            TenantGeneratorsController controller = new TenantGeneratorsController();
            Distribution floorsDistribution = new UniformDistribution(1, numFloors);
            for (int i = 0; i < numFloors; i++)
            {
                controller.Generators.Add(new TenantGenerator(i + 1, floorsDistribution));
            }

            return controller;
        }
        private TenantQueuesController BuildQueuesController(int numFloors)
        {
            TenantQueuesController controller = new TenantQueuesController();
            for (int i = 0; i < numFloors; i++)
            {
                controller.Queues.Add(new TenantQueue(i + 1));
            }

            return controller;
        }
        private ElevatorsController BuildElevatorsController(int numFloors, int numElevators, ElevatorParameters[] parameters)
        {
            // Build global scheduler
            GlobalScheduler scheduler = new NearestCarScheduler(numFloors);

            // Build controller
            ElevatorsController controller = new ElevatorsController(scheduler);
            Elevator elevator;
            for (int i = 0; i < numElevators; i++)
            {
                elevator = BuildElevator(i + 1, parameters[i]);
                controller.Elevators.Add(elevator);
            }

            return controller;
        }
        private Elevator BuildElevator(int id, ElevatorParameters parameters)
        {
            // Build call scheduler
            CallScheduler scheduler = new CollectiveCallScheduler();

            // Build elevator
            return new Elevator(
                id,
                parameters.Capacity,
                parameters.StartFloor,
                scheduler);
        }
        private Distribution[] BuildGenerationDistributions(int numFloors, TenantGeneratorParameters[] parameters)
        {
            Distribution[] distributions = new Distribution[numFloors];
            int min, max;
            for (int i = 0; i < numFloors; i++)
            {
                min = parameters[i].Period - parameters[i].DPeriod;
                max = parameters[i].Period + parameters[i].DPeriod;

                distributions[i] = new UniformDistribution(min, max);
            }

            return distributions;
        }
        private Distribution[] BuildMovementDistributions(int numElevators, ElevatorParameters[] parameters)
        {
            Distribution[] distributions = new Distribution[numElevators];
            int min, max;
            for (int i = 0; i < numElevators; i++)
            {
                min = parameters[i].Period - parameters[i].DPeriod;
                max = parameters[i].Period + parameters[i].DPeriod;

                distributions[i] = new UniformDistribution(min, max);
            }

            return distributions;
        }


        private List<Event> m_events = new List<Event>();

        private readonly EventController m_eventController;
    }
}
