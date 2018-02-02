namespace ElevatorSimulation.SimulationModel.Controllers
{
    /// <summary>
    /// Base class for controllers
    /// </summary>
    abstract class Controller : Resettable
    {
        public abstract void Reset();
    }
}
