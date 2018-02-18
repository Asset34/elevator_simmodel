using System;

namespace ElevatorSimulation.SimulationModel.Distributions
{
    /// <summary>
    /// Base class for distributions
    /// </summary>
    abstract class Distribution
    {
        public abstract int GetValue();

        protected Random m_rand = new Random();
    }
}
