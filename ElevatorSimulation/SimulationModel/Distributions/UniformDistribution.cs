using System;

namespace ElevatorSimulation.SimulationModel.Distributions
{
    /// <summary>
    /// Ordinary uniform distribution between [min; max]
    /// </summary>
    class UniformDistribution : Distribution
    {
        public UniformDistribution(int min, int max)
        {
            if (min > max)
            {
                throw new ArgumentException("Invalid border values of distribution");
            }

            m_min = min;
            m_max = max;
        }
        public override int GetValue()
        {
            return RandomGenerator.Instance.Next(m_min, m_max + 1);
        }

        private int m_min;
        private int m_max;
    }
}
