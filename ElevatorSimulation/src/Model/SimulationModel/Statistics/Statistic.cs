using System;
using System.Text;
using System.Collections.Generic;

namespace ElevatorSimulation.Model.SimulationModel.Statistics
{
    /// <summary>
    /// Class which store name and values of statistical quantity and 
    /// provide operations to handle
    /// </summary>
    abstract class Statistic
    {
        public string Name { get; set; }
        public List<SData> Data { get; }

        public Statistic(string name, ElevatorSimModel model)
        {
            Name = name;
            m_model = model;

            Data = new List<SData>();
        }

        /// <summary>
        /// Get new value from linked entity
        /// at the current time
        /// </summary>
        public abstract void Update();

        protected ElevatorSimModel m_model;
    }
}