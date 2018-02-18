using System;
using System.Collections.Generic;

using ElevatorSimulation.SimulationModel.Entities;
using ElevatorSimulation.SimulationModel.Distributions;
using ElevatorSimulation.SimulationModel.Schedulers;
using ElevatorSimulation.SimulationModel.Statistics;

namespace ElevatorSimulation.SimulationModel
{
    /// <summary>
    /// Builder for elevator simulation model
    /// </summary>
    class ElevatorSimModelBuilder
    {
        /* Entities */
        public List<TenantGenerator> Generators { get; private set; }
        public List<TenantQueue> Queues { get; private set; }
        public List<Elevator> Elevators { get; private set; }

        /* Distributuions */
        public Dictionary<int, Distribution> GeneratorsDistr { get; private set; }
        public Dictionary<int, Distribution> ElevatorsDistr { get; private set; }

        /* Schedulers */
        public ElevatorsScheduler Scheduler { get; private set; }

        /* Statisctical data */
        public List<Statistic> Data { get; private set; }

        public ElevatorSimModelBuilder()
        {
            Generators = new List<TenantGenerator>();
            Queues = new List<TenantQueue>();
            Elevators = new List<Elevator>();

            GeneratorsDistr = new Dictionary<int, Distribution>();
            ElevatorsDistr = new Dictionary<int, Distribution>();

            Data = new List<Statistic>();
        }

        /* Building of parts */
        public ElevatorSimModelBuilder Generator(
            TenantGenerator generator,
            Distribution generationDistr
            )
        {
            Generators.Add(generator);
            GeneratorsDistr.Add(generator.Floor, generationDistr);

            return this;
        }
        public ElevatorSimModelBuilder Queue(
            TenantQueue queue
            )
        {
            Queues.Add(queue);
            
            return this;
        }
        public ElevatorSimModelBuilder Elevator(
            Elevator elevator,
            Distribution movementDistr
            )
        {           
            Elevators.Add(elevator);
            ElevatorsDistr.Add(elevator.ID, movementDistr);           

            return this;
        }

        public ElevatorSimModelBuilder ElevatorsScheduler(ElevatorsScheduler scheduler)
        {
            Scheduler = scheduler;

            return this;
        }
    }
}
