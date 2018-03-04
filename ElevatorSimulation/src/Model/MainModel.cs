using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ElevatorSimulation.Model.SimulationModel;
using ElevatorSimulation.Model.SimulationModel.Distributions;
using ElevatorSimulation.Model.SimulationModel.Entities;
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

            return m_model;
        }
        public void ResetModel()
        {
            if (m_model == null)
            {
                throw new Exception("The model is empty");
            }

            m_model.Reset();
        }
        public void RunModel(int duration)
        {
            if (m_model == null)
            {
                throw new Exception("The model is empty");
            }

            m_model.Run(duration);
        }

        private ElevatorSimModel m_model = null;
    }
}
