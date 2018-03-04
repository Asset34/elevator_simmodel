namespace ElevatorSimulation.Model.SimulationModel.Statistics
{
    /// <summary>
    /// Additonal data class for statistics
    /// </summary>
    class SData
    {
        public int Time { get; set; }
        public double Value { get; set; }

        public SData(int time, double value)
        {
            Time = time;
            Value = value;
        }
    }
}
