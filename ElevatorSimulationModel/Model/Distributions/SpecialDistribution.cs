using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulationModel.Model.Distributions
{
    class SpecialDistribution : Distribution
    {
        public SpecialDistribution(Distribution distribution, int[] specialPoints)
        {
            this.distribution = distribution;
            this.specialPoints = specialPoints;
        }
        public override int GetValue()
        {
            int value;

            do
            {
                value = distribution.GetValue();
            }
            while (specialPoints.Contains(value));

            return value;
        }

        private readonly Distribution distribution;
        private int[] specialPoints;
    }
}
