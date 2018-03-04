using System;

namespace ElevatorSimulation.Model.SimulationModel.Distributions
{
    /// <summary>
    /// Base class for distributions
    /// </summary>
    abstract class Distribution
    {
        public abstract int GetValue();
        
        protected static Random m_rand = new Random();
    }
}
