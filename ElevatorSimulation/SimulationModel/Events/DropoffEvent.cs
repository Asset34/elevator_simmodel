﻿using System;
using System.Text;
using System.Collections.Generic;

using ElevatorSimulation.SimulationModel.Entities;

namespace ElevatorSimulation.SimulationModel.Events
{
    /// <summary>
    /// Event of dropping off the tenants by oe
    /// of the elevators
    /// </summary>
    class DropoffEvent : EventOf1<Elevator>
    {
        public override int Priority
        {
            get { return 6; }
        }

        /// <summary>
        /// Dropped off tenants
        /// </summary>
        public List<Tenant> Tenants { get; set; }
        
        public DropoffEvent(int time, ElevatorSM model, Elevator elevator)
            :base(time, model, elevator)
        {
        }
        public override void Execute()
        {
            Tenants = m_p.Dropoff();
        }
        public override string ToString()
        {
            StringBuilder text = new StringBuilder();
            text.Append(String.Format("Elevator[id:{0}] - drop off[tenants:", m_p.ID));

            foreach (Tenant tenant in Tenants)
            {
                text.Append(String.Format("{0},", tenant.ID));
            }

            text.Append(String.Format("; floor:{0}]", m_p.CurrentFloor));

            return text.ToString();
        }
    }
}
