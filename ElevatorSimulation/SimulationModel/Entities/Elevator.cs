using System;
using System.Collections.Generic;

using ElevatorSimulation.SimulationModel.Transactions;
using ElevatorSimulation.SimulationModel.Schedulers;

namespace ElevatorSimulation.SimulationModel.Entities
{
    /// <summary>
    /// Elevator serving tenants
    /// </summary>
    /// <remarks>
    /// Service device with accumulator
    /// </remarks>
    class Elevator : Entity
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
            get
            {
                int count = 0;
                
                foreach (List<Tenant> list in m_tenants.Values)
                {
                    count += list.Count;
                }

                return count;
            }
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

        public Direction CurrentDirection { get; private set; }

        /// <summary>
        /// Flag defines the elevator reached current call
        /// </summary>
        public bool IsReached
        {
            get
            { return CurrentFloor == DestinationFloor; }
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

        public bool IsDroppingOff
        {
            get
            {
                return m_tenants.ContainsKey(CurrentFloor);
            }
        }

        public Elevator(int id, int capacity, int startFloor)
        {
            ID = id;

            DefaultFloor = startFloor;
            CurrentFloor = startFloor;
            DestinationFloor = startFloor;
            Capacity = capacity;

            m_scheduler = new CallsScheduler(this);
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

            DestinationFloor = m_scheduler.Schedule(CurrentFloor);
            if (DestinationFloor > CurrentFloor)
            {
                CurrentDirection = Direction.Up;
            }
            else
            {
                CurrentDirection = Direction.Down;
            }
        }

        public void Pickup(List<Tenant> tenants)
        {
            foreach (Tenant tenant in tenants)
            {
                Pickup(tenant);
            }
        }
        public void Pickup(Tenant tenant)
        {
            // Check
            if (FreeCount == 0)
            {
                throw new InvalidOperationException("The elevator is full");
            }

            if (!m_tenants.ContainsKey(tenant.FloorTo))
            {
                m_tenants.Add(tenant.FloorTo, new List<Tenant>());          
            }

            m_tenants[tenant.FloorTo].Add(tenant);
        }
        public List<Tenant> Dropoff()
        {   
            List<Tenant> tenants = m_tenants[CurrentFloor];

            m_tenants.Remove(CurrentFloor);

            return tenants;
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


            // Move
            if (CurrentDirection == Direction.Up)
            {
                CurrentFloor++;
            }
            else
            {
                CurrentFloor--;
            }
        }

        public void Reset()
        {
            CurrentFloor = DefaultFloor;
            
            m_scheduler.Reset();
            m_tenants.Clear();
        }

        private readonly CallsScheduler m_scheduler;
        private Dictionary<int, List<Tenant>> m_tenants = new Dictionary<int, List<Tenant>>();
    }
}
