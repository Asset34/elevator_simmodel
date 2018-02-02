using System;
using System.Text;

using ElevatorSimulation.SimulationModel.Transactions;
using ElevatorSimulation.SimulationModel.Controllers;

namespace ElevatorSimulation.SimulationModel.Events
{
    /// <summary>
    /// Event of dropping off the tenants by oe
    /// of the elevators
    /// </summary>
    /// <remarks>
    /// Arguments:
    ///    - ElevatorID
    /// Results:
    ///    - Floor
    ///    - TenantIDs
    /// </remarks>
    class DropOff : Event
    {
        /// <summary>
        /// ID of the elevator which dropping off the tenants
        /// </summary>
        public int ElevatorID { get; set; }

        /// <summary>
        /// The floor with dropped off tenants
        /// </summary>
        public int Floor { get; set; }
        /// <summary>
        /// IDs of dropped off tenants
        /// </summary>
        public int[] TenantIDs { get; set; }

        public DropOff(int time, EventController handler)
            :base(time, handler)
        {
        }
        public override void Execute()
        {
            m_handler.Handle(this);
        }
        public override string ToString()
        {
            StringBuilder text = new StringBuilder();
            text.Append(String.Format("Elevator {0} dropped off tenants", ElevatorID));

            foreach (int id in TenantIDs)
            {
                text.Append(String.Format("{0}, ", id));
            }

            text.Append(String.Format("on the {0} floor", Floor));

            return text.ToString();
        }
    }
}
