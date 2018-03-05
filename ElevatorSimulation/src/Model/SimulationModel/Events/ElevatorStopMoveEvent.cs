using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ElevatorSimulation.Model.SimulationModel.Entities;

namespace ElevatorSimulation.Model.SimulationModel.Events
{
    class ElevatorStopMoveEvent : EventOf1<Elevator>
    {
        public override int Priority
        {
            get { return 8; }
        }

        public ElevatorStopMoveEvent(int time, ElevatorSimModel model, Elevator p)
            : base(time, model, p)
        {
        }
        public override void Execute()
        {
            m_model.CreateEvent_Dropoff(m_p);
            m_model.CreateEvent_Pickup(m_p);

            if (m_p.State == State.Wait)
            {
                m_model.CreateEvent_ElevatorStartMove(m_p);
            }   
        }

        public override string ToString()
        {
            return string.Format(
                "Elevator[id:{0}] - stop move[floor:{1}]",
                m_p.ID,
                m_p.CurrentFloor
                );
        }
    }
}
