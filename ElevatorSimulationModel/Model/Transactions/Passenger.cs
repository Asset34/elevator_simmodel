namespace ElevatorSimulationModel.Model.Transactions
{
    class Passenger
    {
        public int Id        { get; }
        public int FloorFrom { get; set; }
        public int FloorTo   { get; set; }

        public Passenger()
        {
            counter++;
            Id = counter;
        }
        public override string ToString()
        {
            return string.Format("Passenger: id:{0}, from:{1}, to:{2}", Id, FloorFrom, FloorTo);
        }

        private static int counter = 0;
    }
}
