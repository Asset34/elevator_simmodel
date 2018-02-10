using System;
using System.Collections.Generic;

using ElevatorSimulation.SimulationModel.Transactions;
using ElevatorSimulation.SimulationModel.Schedulers;

namespace ElevatorSimulation.SimulationModel.Entities
{
    enum ElevatorState
    {
        Idle,
        Stop,
        Move
    }

    /// <summary>
    /// Elevator which serves the tenants.
    /// Performs the service device with accumulator
    /// </summary>
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

        public Direction Direction { get; private set; }
        public Direction OppositeDirection
        {
            get
            {
                if (Direction == Direction.Up)
                {
                    return Direction.Down;
                }
                else
                {
                    return Direction.Up;
                }
            }
        }

        public ElevatorState State { get; private set; }

        /// <summary>
        /// Flag which defines the elevator reached destination
        /// </summary>
        public bool IsReached
        {
            get { return CurrentFloor == DestinationFloor; }
        }
        /// <summary>
        /// Flag which defines there is tenants for dropping off
        /// </summary>
        public bool IsDropOff
        {
            get { return m_tenants.ContainsKey(CurrentFloor); }
        }

        public Elevator(int id, int capacity, int startFloor)
        {
            ID = id;

            DefaultFloor = startFloor;
            CurrentFloor = startFloor;
            DestinationFloor = startFloor;
            Capacity = capacity;

            State = ElevatorState.Idle;

            m_scheduler = new CallsScheduler(this);
        }

        public void AddHallcall(Tenant tenant)
        {
            m_scheduler.AddHallcall(tenant);

            if (State == ElevatorState.Idle)
            {
                State = ElevatorState.Stop;
            }

            ScheduleCall();
        }
        public void AddCarcall(Tenant tenant)
        {
            m_scheduler.AddCarcall(tenant);
            ScheduleCall();
        }
        public void RemoveCall(int call)
        {
            // Check
            if (State == ElevatorState.Idle)
            {
                throw new InvalidOperationException("No calls to remove");
            }

            // Remove
            m_scheduler.RemoveCall(call);

            // Check if scheduler has no more calls
            if (m_scheduler.IsEmpty)
            {
                State = ElevatorState.Idle;
            }
        }
        public void ScheduleCall()
        {
            // Check
            if (State == ElevatorState.Idle)
            {
                throw new InvalidOperationException("No calls to set");
            }

            // Schedule new call
            if (State == ElevatorState.Move)
            {
                if (Direction == Direction.Up)
                {
                    DestinationFloor = m_scheduler.Schedule(CurrentFloor + 1);
                }
                else
                {
                    DestinationFloor = m_scheduler.Schedule(CurrentFloor - 1);
                }
            }
            else
            {
                DestinationFloor = m_scheduler.Schedule(CurrentFloor);
            }

            // Set direction
            if (DestinationFloor > CurrentFloor)
            {
                Direction = Direction.Up;
            }
            else
            {
                Direction = Direction.Down;
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
            // Checks
            if (FreeCount == 0)
            {
                throw new InvalidOperationException("The elevator is full");
            }
            if (State == ElevatorState.Move)
            {
                throw new InvalidOperationException("Pick up during the movement");
            }

            if (!m_tenants.ContainsKey(tenant.FloorTo))
            {
                m_tenants.Add(tenant.FloorTo, new List<Tenant>());          
            }

            m_tenants[tenant.FloorTo].Add(tenant);
        }
        public List<Tenant> Dropoff()
        {
            // Check
            if (State == ElevatorState.Move)
            {
                throw new InvalidOperationException("Drop off during the movement");
            }

            List<Tenant> tenants = m_tenants[CurrentFloor];

            m_tenants.Remove(CurrentFloor);

            return tenants;
        }

        public void EnableMove()
        {
            State = ElevatorState.Move;
        }
        public void Move()
        {
            // Checks
            if (State == ElevatorState.Idle)
            {
                throw new InvalidOperationException("No calls to move");
            }
            if (IsReached)
            {
                throw new InvalidOperationException("The elevator reached the next called floor");
            }

            // Move
            if (Direction == Direction.Up)
            {
                CurrentFloor++;
            }
            else
            {
                CurrentFloor--;
            }

            // Stop elevator
            State = ElevatorState.Stop;
        }

        public void Reset()
        {
            CurrentFloor = DefaultFloor;

            State = ElevatorState.Idle;
            
            m_scheduler.Reset();
            m_tenants.Clear();
        }

        private readonly CallsScheduler m_scheduler;
        private Dictionary<int, List<Tenant>> m_tenants = new Dictionary<int, List<Tenant>>();
    }
}
