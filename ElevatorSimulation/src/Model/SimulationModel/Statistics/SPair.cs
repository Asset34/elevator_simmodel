namespace ElevatorSimulation.Model.SimulationModel.Statistics
{
    /// <summary>
    /// Additonal data class for handled statistics
    /// </summary>
    class SPair
    {
        public string Name { get; set; }
        public double Value { get; set; }

        public SPair(string name, double value)
        {
            Name = name;
            Value = value;
        }
    }
}
