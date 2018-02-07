using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public static ElevatorSimulationModel Instance
        {
            get
            {
                if (m_instance == null)
                {
                    throw new NullReferenceException("Trying instance of uncreated simulation model");
                }

                return m_instance;
            }
        }

        /// <summary>
        /// Current model time
        /// </summary>
        public int Time { get; private set; }

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
            Dictionary<int, Distribution> generationDistributions
                = BuildGenerationDistributions(parameters.NumFloors, parameters.TenantGeneratorParameters);
            Dictionary<int, Distribution> movementDistributions
                = BuildMovementDistributions(parameters.NumElevators, parameters.ElevatorParameters);

            // Build event controller
            m_eventController = new EventController(
                generatorsController,
                queuesController,
                elevatorsController,
                generationDistributions,
                movementDistributions
                );

            m_instance = this;
        }
        /// <summary>
        /// Run the model
        /// </summary>
        public List<string> Run(int dTime)
        {
            List<string> log = new List<string>();
            Event ev;
            while (Time <= dTime)
            {
                // Get nearest event
                ev = m_eventController.GetNextEvent();

                // Set new model time
                Time = ev.Time;

                // Execute event
                ev.Execute();

                // Add to log
                log.Add(ev.ToString());
            }

            return log;
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
                controller.Generators.Add(i + 1, new TenantGenerator(i + 1, floorsDistribution));
            }

            return controller;
        }
        private TenantQueuesController BuildQueuesController(int numFloors)
        {
            TenantQueuesController controller = new TenantQueuesController();
            for (int i = 0; i < numFloors; i++)
            {
                controller.Queues.Add(i + 1, new TenantQueue(i + 1));
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
                controller.Elevators.Add(i + 1, elevator);
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
        
        private List<Event> m_events = new List<Event>();

        private readonly EventController m_eventController;

        private static ElevatorSimulationModel m_instance;
    }
}
