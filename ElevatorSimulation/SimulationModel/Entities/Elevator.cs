using System;
using System.Collections.Generic;
using System.Linq;

using ElevatorSimulation.SimulationModel.Transactions;
using ElevatorSimulation.SimulationModel.Schedulers.CallSchedulers;

namespace ElevatorSimulation.SimulationModel.Entities
{
    /// <summary>
    /// Elevator serving tenants
    /// </summary>
    /// <remarks>
    /// Service device with accumulator
    /// </remarks>
    class Elevator : Entitie
    {
        public int ID { get; }
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

        /// <summary>
        /// Flag defines the elevator reached current call
        /// </summary>
        public bool IsReached
        {
            get
            {
                return CurrentFloor == DestinationFloor;
            }
        }
        /// <summary>
        /// Flag defines the elevator finished the work
        /// (The scheduler has no calls to handle)
        /// </summary>
        public bool IsIdle
        {
            get
            {
                return m_scheduler.IsEmpty &&
                       IsReached;
            }
        }

        public int DefaultFloor { get; set; }
        public int CurrentFloor { get; private set; }
        public int DestinationFloor { get; private set; }

        public CallType CurrentCallType { get; private set; }

        public Elevator(int id, int capacity, int startFloor, CallScheduler scheduler)
        {
            ID = id;

            DefaultFloor = startFloor;
            CurrentFloor = startFloor;
            DestinationFloor = startFloor;
            Capacity = capacity;

            m_scheduler = scheduler;
            m_scheduler.SetElevator(this);
        }

        public void AddHallcall(Tenant tenant)
        {
            m_scheduler.AddHallcall(tenant);
            SetCall();
        }
        public void AddCarcall(Tenant tenant)
        {
            m_scheduler.AddCarcall(tenant);
            SetCall();
        }
        public void RemoveCall(int call)
        {
            // Check
            if (IsIdle)
            {
                throw new InvalidOperationException("No calls to remove");
            }

            m_scheduler.RemoveCall(call);
        }
        public void SetCall()
        {
            // Check
            if (IsIdle)
            {
                throw new InvalidOperationException("No calls to setting");
            }

            DestinationFloor = m_scheduler.GetCall(CurrentFloor);
            if (DestinationFloor > CurrentFloor)
            {
                CurrentCallType = CallType.Up;
            }
            else
            {
                CurrentCallType = CallType.Down;
            }
        }

        public void Move()
        {
            // Checks
            if (IsIdle)
            {
                throw new InvalidOperationException("No calls to move");
            }
            else if (IsReached)
            {
                throw new InvalidOperationException("The elevator reached the next called floor");
            }

            if (CurrentCallType == CallType.Up)
            {
                CurrentFloor++;
            }
            else
            {
                CurrentFloor--;
            }
        }

        public void PickUp(List<Tenant> tenants)
        {
            foreach (Tenant tenant in tenants)
            {
                PickUp(tenant);
            }
        }
        public void PickUp(Tenant tenant)
        {
            // Check
            if (FreeCount == 0)
            {
                throw new InvalidOperationException("The elevator is full");
            }

            m_tenants.Add(tenant);
            AddCarcall(tenant);
        }
        public List<Tenant> DropOff()
        {   
            // Get list of dropped tenants
            var droppedTenants = m_tenants.Where(x => x.FloorTo == CurrentFloor).ToList();
            
            // Remove dropped tenants from elevator
            foreach (var tenant in droppedTenants)
            {
                m_tenants.Remove(tenant);
            }

            return droppedTenants;
        }

        public override void Reset()
        {
            CurrentFloor = DefaultFloor;
            
            m_scheduler.Reset();
            m_tenants.Clear();
        }

        private readonly CallScheduler m_scheduler;
        private List<Tenant> m_tenants = new List<Tenant>();
    }
}
