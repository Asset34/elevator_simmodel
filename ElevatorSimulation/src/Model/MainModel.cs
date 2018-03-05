using System;
using System.Collections.Generic;

using ElevatorSimulation.Model.SimulationModel;
using ElevatorSimulation.Model.SimulationModel.Statistics;

namespace ElevatorSimulation.Model
{
    /// <summary>
    /// 
    /// </summary>
    class MainModel
    {
        public ElevatorSimModel BuildModel(List<FloorData> floors, List<ElevatorData> elevators)
        {
            ElevatorSimModelBuilder builder = new ElevatorSimModelBuilder();
            builder.BuildGenerators(floors);
            builder.BuildQueues(floors);
            builder.BuildElevators(elevators);

            m_model = new ElevatorSimModel(builder);

            LinkStatistics(floors.Count, elevators.Count);

            return m_model;
        }
        public void Reset()
        {
            if (m_model == null)
            {
                throw new Exception("The model is empty");
            }

            m_model.Reset();
            
            // Reset statistics
            foreach (Statistic statistic in m_statistics)
            {
                statistic.Data.Clear();
            }
        }
        public void RunModel(int duration)
        {
            if (m_model == null)
            {
                throw new Exception("The model is empty");
            }

            m_model.Run(duration);
        }
        public void LinkStatistics(int numFloors, int numElevators)
        {
            m_statistics.Clear();

            // Link statistics to the queues
            QueueSize queueSize;
            for (int i = 0; i < numFloors; i++)
            {
                queueSize = new QueueSize(string.Format("Queue size {0}", i + 1), m_model);
                queueSize.Link(m_model.QueuesController.Get(i + 1));
                m_statistics.Add(queueSize);
            }

            // Link statistics to the elevators
            ElevatorOccupancy elevatorOccupancy;
            for (int i = 0; i < numElevators; i++)
            {
                elevatorOccupancy = new ElevatorOccupancy(string.Format("Elevator occupancy ({0})", i + 1), m_model);
                elevatorOccupancy.Link(m_model.ElevatorsController.Get(i + 1));
                m_statistics.Add(elevatorOccupancy);
            }
        }
        public List<SPair> HandleStatistics()
        {
            List<SPair> pairs = new List<SPair>();

            foreach (Statistic statistic in m_statistics)
            {
                pairs.Add(StatisticHandler.Average(statistic));
            }

            return pairs;
        }

        private List<Statistic> m_statistics = new List<Statistic>();
        private ElevatorSimModel m_model = null;
    }
}
