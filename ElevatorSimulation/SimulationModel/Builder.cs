using System;
using System.Collections.Generic;

using ElevatorSimulation.SimulationModel.Entities;
using ElevatorSimulation.SimulationModel.Distributions;
using ElevatorSimulation.SimulationModel.Schedulers;
using ElevatorSimulation.SimulationModel.Controllers;

namespace ElevatorSimulation.SimulationModel
{
    partial class ElevatorSM
    {
        public static class Builder
        {
            static public void TenantGenerator(int floor, Distribution floorDistr)
            {
                m_generators.Add(floor, new TenantGenerator(floor, floorDistr));
            }
            static public void TenantQueue(int floor)
            {
                m_queues.Add(floor, new TenantQueue(floor));
            }
            static public void Elevator(int id, int capacity, int startFloor, CallsScheduler scheduler)
            {
                m_elevators.Add(id, new Elevator(id, capacity, startFloor, scheduler));
            }
            static public void ElevatorsScheduler(ElevatorsScheduler scheduler)
            {
                m_scheduler = scheduler;
            }

            static public void TenantGenerationDistr(int floor, Distribution distr)
            {
                m_generatorsDistr.Add(floor, distr);
            }
            static public void ElevatorMovementDistr(int id, Distribution distr)
            {
                m_elevatorsDistr.Add(id, distr);
            }

            static public ElevatorSM Build()
            {
                ElevatorSM model = new ElevatorSM();
                
                // Build tenant generators controller
                model.m_generatorsController = new TenantGeneratorsController();
                foreach (TenantGenerator generator in m_generators.Values)
                {
                    model.m_generatorsController.Add(generator);
                }

                // Build tenant queues controller
                model.m_queuesController = new TenantQueuesController();
                foreach (TenantQueue queue in m_queues.Values)
                {
                    model.m_queuesController.Add(queue);
                }

                // BUild elevators cotroller
                model.m_elevatorsController = new ElevatorsController(m_scheduler);
                foreach (Elevator elevator in m_elevators.Values)
                {
                    model.m_elevatorsController.Add(elevator);
                }

                // Build events scheduler
                model.m_scheduler = new EventsScheduler();

                // Build distributions
                model.m_generatorsDistr = m_generatorsDistr;
                model.m_elevatorsDistr = m_elevatorsDistr;

                return model;
            }

            static public void Reset()
            {
                m_generators.Clear();
                m_queues.Clear();
                m_elevators.Clear();
            }

            static private Dictionary<int, TenantGenerator> m_generators
                = new Dictionary<int, TenantGenerator>();
            static private Dictionary<int, TenantQueue> m_queues
                = new Dictionary<int, TenantQueue>();
            static private Dictionary<int, Elevator> m_elevators
                = new Dictionary<int, Elevator>();

            static private ElevatorsScheduler m_scheduler;

            static private Dictionary<int, Distribution> m_generatorsDistr
                = new Dictionary<int, Distribution>();
            static private Dictionary<int, Distribution> m_elevatorsDistr
                = new Dictionary<int, Distribution>();
        }
    }  
}
