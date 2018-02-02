namespace ElevatorSimulation.SimulationModel.Entities
{
    /// <summary>
    /// Base class for entities of queueing theory
    /// </summary>
    abstract class Entitie : Resettable
    {
        public abstract void Reset();
    }
}
