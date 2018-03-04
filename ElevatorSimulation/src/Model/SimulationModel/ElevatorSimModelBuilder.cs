using System;
using System.Collections.Generic;

using ElevatorSimulation.Model;
using ElevatorSimulation.Model.SimulationModel.Entities;
using ElevatorSimulation.Model.SimulationModel.Distributions;
using ElevatorSimulation.Model.SimulationModel.Schedulers;
using ElevatorSimulation.Model.SimulationModel.Statistics;

namespace ElevatorSimulation.Model.SimulationModel
{
    /// <summary>
    /// Builder for elevator simulation model
    /// </summary>
    class ElevatorSimModelBuilder
    {
        public int NumFloors { get; private set; }

        /* Entities */
        public List<TenantGenerator> Generators { get; private set; }
        public List<TenantQueue> Queues { get; private set; }
        public List<Elevator> Elevators { get; private set; }

        /* Distributuions */
        public Dictionary<int, Distribution> GeneratorsDistr { get; private set; }
        public Dictionary<int, Distribution> ElevatorsDistr { get; private set; }

        public ElevatorSimModelBuilder()
        {
            Generators = new List<TenantGenerator>();
            Queues = new List<TenantQueue>();
            Elevators = new List<Elevator>();

            GeneratorsDistr = new Dictionary<int, Distribution>();
            ElevatorsDistr = new Dictionary<int, Distribution>();
        }

        public void BuildGenerators(List<FloorData> floors)
        {
            Distribution floorDistr = new UniformDistribution(1, floors.Count);
            int min, max;
            foreach (FloorData data in floors)
            {
                // Add generator
                Generators.Add(new TenantGenerator(data.ID, floorDistr));

                // Add distribution
                min = data.Period - data.Spread;
                max = data.Period + data.Spread;
                GeneratorsDistr.Add(data.ID, new UniformDistribution(min, max));     
            }

            NumFloors = floors.Count;
        }
        public void BuildQueues(List<FloorData> floors)
        {
            foreach (FloorData data in floors)
            {
                Queues.Add(new TenantQueue(data.ID));
            }

            NumFloors = floors.Count;
        }
        public void BuildElevators(List<ElevatorData> elevators)
        {
            int min, max;
            foreach (ElevatorData data in elevators)
            {
                // Add elevator
                Elevators.Add(new Elevator(data.ID, data.Capacity, data.StartFloor));

                // Add distribution
                min = data.Period - data.Spread;
                max = data.Period + data.Spread;
                ElevatorsDistr.Add(data.ID, new UniformDistribution(min, max));
            }
        }
    }
}
