using ElevatorSimulation.SimulationModel.Entities;

namespace ElevatorSimulation.SimulationModel.Events
{
    /// <summary>
    /// Event of adding the new hall call to the elevator
    /// </summary>
    class NewHallcallEvent : EventOf2<Tenant, Elevator>
    {
        public override int Priority
        {
            get { return 3; }
        }

        public NewHallcallEvent(int time, ElevatorSimModel model, Tenant tenant, Elevator elevator)
            :base(time, model, tenant, elevator)
        {
        }
        public override void Execute()
        {
            State prevState = m_p2.State;

            m_p2.AddHallcall(m_p1);

            if (prevState == State.Wait)
            {
                if (m_p2.State == State.Wait)
                {
                    m_model.CreateEvent_Dropoff(m_p2);
                    m_model.CreateEvent_Pickup(m_p2);

                }
                else if (m_p2.State == State.Move)
                {
                    m_model.CreateEvent_ElevatorMove(m_p2);
                }
            }
        }
        public override string ToString()
        {
            return string.Format(
                "Elevator[id:{0}] - add hallcall[floor:{1}; type:{2}]",
                m_p2.ID,
                m_p1.FloorFrom,
                m_p1.Direction
                );
        }
    }
}
