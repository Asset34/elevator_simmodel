using System;
using System.Linq;

using ElevatorSimulation.Model.SimulationModel.Entities;

namespace ElevatorSimulation.Model.SimulationModel.Statistics
{
    /// <summary>
    /// Statistical quantity which performs tenant queue size
    /// </summary>
    class QueueSize : Statistic
    {
        public QueueSize(string name, ElevatorSimModel model)
            : base(name, model)
        {
        }

        public void Link(TenantQueue queue)
        {
            m_queue = queue;
            m_queue.Changed += Update;
        }
        public override void Update()
        {
            if (Data.Count == 0 || Data.Last().Time != m_model.Time)
            {
                Data.Add(new SData(m_model.Time, m_queue.Count));
            }
            else
            {
                Data.Last().Value = m_queue.Count;
            }
        }

        private TenantQueue m_queue;      
    }
}
