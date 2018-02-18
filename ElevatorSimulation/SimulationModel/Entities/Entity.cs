namespace ElevatorSimulation.SimulationModel.Entities
{
    /// <summary>
    /// Base class for model entities which defines events
    /// to notify all associated statistical classes
    /// </summary>
    abstract class Entity
    {
        public delegate void EventHandler();
        public event EventHandler Changed;

        public virtual void OnChanged()
        {
            if (Changed != null)
            {
                Changed();
            }
        }
    }
}
