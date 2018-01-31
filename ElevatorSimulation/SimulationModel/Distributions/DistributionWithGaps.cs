using System;
using System.Linq;

namespace ElevatorSimulation.SimulationModel.Distributions
{
    /// <summary>
    /// "Decorator" for distributions which excludes specific values(gaps)
    /// </summary>
    class DistributionWithGaps : Distribution
    {
        public DistributionWithGaps(Distribution distribution, params int[] gaps)
        {
            m_distribution = distribution;
            m_gaps = gaps;
        }
        public override int GetValue()
        {
            int value;

            do
            {
                value = m_distribution.GetValue();
            }
            while (m_gaps.Contains(value));

            return value;
        }

        private readonly Distribution m_distribution;
        private readonly int[] m_gaps;
    }
}
