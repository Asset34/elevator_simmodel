using System;
using System.Collections.Generic;
using System.Linq;

using ElevatorSimulation.SimulationModel.Transactions;
using ElevatorSimulation.SimulationModel.Dispatchers.RequestDispatchers;

namespace ElevatorSimulation.SimulationModel.Entities
{
    enum ElevatorState
    {
        Wait,
        Stop,
        MoveUp,
        MoveDown
    }

    /// <summary>
    /// Elevator serving tenants
    /// </summary>
    /// <remarks>
    /// Service device with accumulator
    /// </remarks>
    class Elevator : Resettable
    {
        public int Id { get; }
        /// <summary>
        /// Maximum number of tenants
        /// </summary>
        public int Capacity { get; set; }
        public int DefaultFloor { get; set; }
        public int CurrentFloor { get; private set; }
        public int DestinationFloor { get; private set; }
        public ElevatorState State { get; private set; }
        /// <summary>
        /// Number of tenants that can be picked up
        /// </summary>
        public int FreeSpace
        {
            get { return Capacity - m_tenants.Count; }
        }

        public Elevator(int id, int capacity, int startFloor, RequestDispatcher requestDispatcher)
        {
            Id = id;
            DefaultFloor = startFloor;
            CurrentFloor = startFloor;
            Capacity = capacity;
            m_dispatcher = requestDispatcher;

            State = ElevatorState.Wait;
        }
        public void Move()
        {
            switch (State)
            {
                case ElevatorState.MoveDown:
                    CurrentFloor--;
                    break;
                case ElevatorState.MoveUp:
                    CurrentFloor++;
                    break;
            }
        }
        public void AddRequest(Request request)
        {
            m_dispatcher.Register(request);
            SetRequest();         
        }
        public void SetRequest()
        {
            // Get new handling request
            DestinationFloor = m_dispatcher.GetRequest();

            // Set new state
            if (DestinationFloor > CurrentFloor)
            {
                State = ElevatorState.MoveUp;
            }
            else
            {
                State = ElevatorState.MoveDown;
            }
        }
        public void PickUp(Tenant[] tenants)
        {
            foreach (Tenant tenant in tenants)
            {
                PickUp(tenant);
            }
        }
        public void PickUp(Tenant tenant)
        {
            // Checks
            if (State != ElevatorState.Stop)
            {
                throw new InvalidOperationException("Picking up during the movement");
            }
            else if (m_tenants.Count == Capacity)
            {
                throw new InvalidOperationException("The elevator is full");
            }

            // Add tenant
            m_tenants.Add(tenant, tenant.FloorTo);

            // Add request from tenant
            AddRequest(tenant.GetInternalRequest());
        }
        public Tenant[] DropOff()
        {
            // Checks
            if (State == ElevatorState.Stop)
            {
                throw new InvalidOperationException("Dropping off during the movement");
            }
            else if (m_tenants.Count == 0)
            {
                throw new InvalidOperationException("");
            }

            // Get list of dropping off tenants
            List<Tenant> tenants = new List<Tenant>();
            foreach (var item in m_tenants.Where(kvp => kvp.Value == CurrentFloor).ToList())
            {
                tenants.Add(item.Key);
                m_tenants.Remove(item.Key);
            }

            return tenants.ToArray();
        }
        public void Reset()
        {
            CurrentFloor = DefaultFloor;
            State = ElevatorState.Wait;

            m_dispatcher.Reset();
            m_tenants.Clear();
        }

        private readonly RequestDispatcher m_dispatcher;
        private Dictionary<Tenant, int> m_tenants = new Dictionary<Tenant, int>();
    }
}
