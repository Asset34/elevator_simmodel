namespace ElevatorSimulation.SimulationModel.Events
{
    /// <summary>
    /// Base class for events between two participants
    /// </summary>
    /// <typeparam name="T1"> 1st participant </typeparam>
    /// <typeparam name="T2"> 2nd participant </typeparam>
    abstract class EventOf2<T1, T2> : Event
    {
        public EventOf2(int time, ElevatorSM model, T1 p1, T2 p2)
            :base(time, model)
        {
            m_p1 = p1;
            m_p2 = p2;
        }
        
        protected T1 m_p1;
        protected T2 m_p2;
    }
}
