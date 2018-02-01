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
        /// Maximum number of served tenants
        /// </summary>
        public int Capacity { get; set; }
        /// <summary>
        /// Number of served tenants
        /// </summary>
        public int FillCount
        {
            get { return m_tenants.Count; }
        }
        /// <summary>
        /// Number of free places
        /// </summary>
        public int FreeCount
        {
            get { return Capacity - FillCount; }
        }
        public int DefaultFloor { get; set; }
        public int CurrentFloor { get; private set; }
        public int DestinationFloor { get; private set; }
        public ElevatorState State { get; private set; }

        public Elevator
            (
            int id, 
            int capacity, 
            int startFloor, 
            CallDispatcher requestDispatcher
            )
        {
            Id = id;
            DefaultFloor = startFloor;
            CurrentFloor = startFloor;
            Capacity = capacity;

            m_dispatcher = requestDispatcher;
            m_dispatcher.SetElevator(this);

            State = ElevatorState.Wait;
        }
        public void Move()
        {   
            if (CurrentFloor != DestinationFloor)
            {
                if (State == ElevatorState.MoveUp)
                {
                    CurrentFloor++;
                }
                else if (State == ElevatorState.MoveDown)
                {
                    CurrentFloor--;
                }    
            }

            if (CurrentFloor == DestinationFloor)
            {
                m_dispatcher.UnregisterCall(DestinationFloor);
                SetCall();
            }
        }
        public void AddHallCall(Tenant tenant)
        {
            m_dispatcher.RegisterHallCall(tenant);

            SetCall();
        }
        public void AddCarCall(Tenant tenant)
        {
            m_dispatcher.RegisterCarCall(tenant);

            SetCall();
        }
        public void SetCall()
        {
            Tuple<int, bool> result = m_dispatcher.GetCall(CurrentFloor);
            
            if (result.Item2)
            {
                DestinationFloor = result.Item1;

                if (DestinationFloor >= CurrentFloor)
                {
                    State = ElevatorState.MoveUp;
                }
                else
                {
                    State = ElevatorState.MoveDown;
                }
            }
            // If there are no calls
            else
            {
                State = ElevatorState.Wait;
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
            if (FreeCount == 0)
            {
                throw new InvalidOperationException("The elevator is full");
            }

            // Add tenant
            m_tenants.Add(tenant, tenant.FloorTo);

            // Add call
            AddCarCall(tenant);
        }
        public List<Tenant> DropOff()
        {        
            // Get list of dropping off tenants
            List<Tenant> tenants = new List<Tenant>();
            foreach (var item in m_tenants.Where(kvp => kvp.Value == CurrentFloor).ToList())
            {
                tenants.Add(item.Key);
                m_tenants.Remove(item.Key);
            }

            return tenants;
        }
        public void Reset()
        {
            CurrentFloor = DefaultFloor;
            State = ElevatorState.Wait;

            m_dispatcher.Reset();
            m_tenants.Clear();
        }

        private readonly CallDispatcher m_dispatcher;
        private Dictionary<Tenant, int> m_tenants = new Dictionary<Tenant, int>();
    }
}
