using System;
using System.Linq;

using ElevatorSimulation.Model.SimulationModel.Entities;

namespace ElevatorSimulation.Model.SimulationModel.Statistics
{
    /// <summary>
    /// Statistical quantity which performs elevator occupancy
    /// </summary>
    class ElevatorOccupancy : Statistic
    {
        public ElevatorOccupancy(string name, ElevatorSimModel model)
            : base(name, model)
        {
        }

        public void Link(Elevator elevator)
        {
            m_elevator = elevator;
            m_elevator.Changed += Update;
        }
        public override void Update()
        {
            if (Data.Count == 0 || Data.Last().Time != m_model.Time)
            {
                Data.Add(new SData(m_model.Time, m_elevator.Occupancy));
            }
            else
            {
                Data.Last().Value = m_elevator.Occupancy;
            }
        }

        private Elevator m_elevator;
    }
}
