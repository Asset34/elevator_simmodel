using ElevatorSimulation.SimulationModel.Entities;

namespace ElevatorSimulation.SimulationModel.Statistics
{
    /// <summary>
    /// Statistical quantity which performs tenant queue size
    /// </summary>
    class QueueSize : Statistic
    {
        public QueueSize(string name, ElevatorSimModel model)
            : base(name, model)
        {
        }

        public override void Link(int n)
        {
            m_queue = m_model.QueuesController.Get(n);
            m_queue.Changed += Update;
        }
        public override void Update()
        {
            Data.Add(new DataValue(m_model.Time, m_queue.Count));
        }

        protected TenantQueue m_queue;
    }
}