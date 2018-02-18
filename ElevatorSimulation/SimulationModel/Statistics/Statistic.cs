using System;
using System.Text;
using System.Collections.Generic;

namespace ElevatorSimulation.SimulationModel.Statistics
{
    /// <summary>
    /// Class which store name and values of statistical quantity and 
    /// provide operations to handle
    /// </summary>
    abstract class Statistic
    {
        public string Name { get; set; }
        /// <summary>
        /// Set of values
        /// </summary>
        public SortedList<int, DataValue> Data { get; }

        public Statistic(string name, ElevatorSimModel model)
        {
            Name = name;
            m_model = model;

            Data = new SortedList<int, DataValue>();
        }

        /// <summary>
        /// Link statistical data with one of the entities
        /// </summary>
        /// <param name="n"> Identification number of the linked entity </param>
        public abstract void Link(int n);
        /// <summary>
        /// Get new value from linked entity
        /// at the current time
        /// </summary>
        public abstract void Update();

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();

            str.Append(string.Format("{0}:", Name));

            foreach (DataValue data in Data.Values)
            {
                str.Append(string.Format(" {0}", data));
            }

            return str.ToString();
        }

        protected void Check()
        {
            if (Data.ContainsKey(m_model.Time))
            {
                Data.Remove(m_model.Time);
            }
        }

        protected ElevatorSimModel m_model;
    }
}
