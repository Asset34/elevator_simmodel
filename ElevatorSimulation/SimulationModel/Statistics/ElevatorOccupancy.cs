﻿using System;
using System.Linq;

using ElevatorSimulation.SimulationModel.Entities;

namespace ElevatorSimulation.SimulationModel.Statistics
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

        public override void Link(int n)
        {
            m_elevator = m_model.ElevatorsController.Get(n);
            m_elevator.Changed += Update;
        }
        public override void Update()
        {
            if (Data.Count == 0 || Data.Last().Time != m_model.Time)
            {
                Data.Add(new DataValue(m_model.Time, m_elevator.Occupancy));
            }
            else
            {
                Data.Last().Value = m_elevator.Occupancy;
            }
        }

        protected Elevator m_elevator;
    }
}
