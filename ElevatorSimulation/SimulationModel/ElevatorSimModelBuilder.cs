using System;
using System.Collections.Generic;

using ElevatorSimulation.SimulationModel.Entities;
using ElevatorSimulation.SimulationModel.Distributions;
using ElevatorSimulation.SimulationModel.Schedulers;
using ElevatorSimulation.SimulationModel.Statistic.Quantities;

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

        /* Statisctical quantities */
        public List<StatisticQuantity> Quantities { get; private set; }

        public ElevatorSimModelBuilder()
        {
            Generators = new List<TenantGenerator>();
            Queues = new List<TenantQueue>();
            Elevators = new List<Elevator>();

            GeneratorsDistr = new Dictionary<int, Distribution>();
            ElevatorsDistr = new Dictionary<int, Distribution>();
        }

        /* Building of parts */
        public ElevatorSimModelBuilder Generator(
            TenantGenerator generator,
            Distribution generationDistr,
            BindedStatisticQuantity<TenantGenerator> quantity = null
            )
        {
            if (quantity != null)
            {
                quantity.Bind(generator);
                Quantities.Add(quantity);
            }

            Generators.Add(generator);
            GeneratorsDistr.Add(generator.Floor, generationDistr);

            return this;
        }
        public ElevatorSimModelBuilder Queue(
            TenantQueue queue,
            BindedStatisticQuantity<TenantQueue> quantity = null
            )
        {
            if (quantity != null)
            {
                quantity.Bind(queue);
                Quantities.Add(quantity);
            }

            Queues.Add(queue);
            
            return this;
        }
        public ElevatorSimModelBuilder Elevator(
            Elevator elevator,
            Distribution movementDistr,
            BindedStatisticQuantity<Elevator> quantity = null
            )
        {
            if (quantity != null)
            {
                quantity.Bind(elevator);
                Quantities.Add(quantity);
            }
            
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
