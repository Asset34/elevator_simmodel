using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulationModel
{
    class Parameters
    {
        // Number of floors
        public int nFloors { get; set; }

        // Number of elevators
        public int nElevators { get; set; }

        // Periods of people appearance
        public Tuple<double, double>[] PeriodsAppearance { get; set; }
        
        // Probabilities of people appearance
        public Tuple<double, double>[] ProbabiliiesAppearance { get; set; }

        // Period of elevator movement
        public Tuple<double, double> Period { get; set; }
    }
}
