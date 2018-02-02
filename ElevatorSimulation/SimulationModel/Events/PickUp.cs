using System;
using System.Text;

using ElevatorSimulation.SimulationModel.Transactions;
using ElevatorSimulation.SimulationModel.Controllers;

namespace ElevatorSimulation.SimulationModel.Events
{
    /// <summary>
    /// Event of picking up the tenants by one
    /// of the elevators
    /// </summary>
    /// <remarks>
    /// Arguments:
    ///    - ElevatorID
    /// Results:
    ///    - Floor
    ///    - TenantIDs
    /// </remarks>
    class PickUp : Event
    {
        /// <summary>
        /// ID of the elevator which pickig up the tenants
        /// </summary>
        public int ElevatorID { get; set; }

        /// <summary>
        /// The floor with picked up tenants
        /// </summary>
        public int Floor { get; set; }
        /// <summary>
        /// IDs of picked up tenants
        /// </summary>
        public int[] TenantIDs { get; set; }

        public PickUp(int time, EventController handler)
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
            text.Append(String.Format("Elevator {0} picked up tenants", ElevatorID));

            foreach (int id in TenantIDs)
            {
                text.Append(String.Format("{0}, ", id));
            }

            text.Append(String.Format("on the {0} floor", Floor));

            return text.ToString();
        }
    }
}
