namespace ElevatorSimulation.SimulationModel.Statistics
{
    /// <summary>
    /// Data class which stores value in specific moment of time
    /// </summary>
    class DataValue
    {
        public int Time { get; set; }
        public double Value { get; set; }

        public DataValue(int time, double value)
        {
            Time = time;
            Value = value;
        }

        public override string ToString()
        {
            return string.Format("[{0}; {1}]", Time, Value);
        }
    }
}
