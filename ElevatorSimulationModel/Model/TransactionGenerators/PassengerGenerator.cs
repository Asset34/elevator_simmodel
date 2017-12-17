using ElevatorSimulationModel.Model.Distributions;
using ElevatorSimulationModel.Model.Transactions;

namespace ElevatorSimulationModel.Model.TransactionGenerators
{
    class PassengerGenerator
    {
        public PassengerGenerator(int floor, Distribution floorDistribution)
        {
            this.floor = floor;
            this.floorDistribution = new SpecialDistribution(floorDistribution, new int[] {floor});
        }
        public Passenger Generate()
        {
            return new Passenger()
            {
                FloorFrom = floor,
                FloorTo = floorDistribution.GetValue()
            };
        }

        private readonly int floor;
        private readonly Distribution floorDistribution;
    }
}
