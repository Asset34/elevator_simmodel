using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulationModel.Model.Distributions
{
    class UniformDistribution : Distribution
    {
        public UniformDistribution(int leftBorder, int rightBorder)
        {
            this.leftBorder = leftBorder;
            this.rightBorder = rightBorder;

            CheckBorders();
        }
        public override int GetValue()
        {
            return leftBorder + randGenerator.Next(rightBorder - leftBorder);
        }

        private int leftBorder;
        private int rightBorder;
        private Random randGenerator = new Random();

        private void CheckBorders()
        {
            if (leftBorder > rightBorder)
            {
                throw new ArgumentException();
            }
        }
    }
}
