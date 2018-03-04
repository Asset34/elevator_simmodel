using System;
using System.Collections.Generic;

using ElevatorSimulation.Model.SimulationModel.Schedulers;
using ElevatorSimulation.Model.SimulationModel.Transactions;

namespace ElevatorSimulation.Model.SimulationModel.Entities
{
    /// <summary>
    /// Elevator serving and accumulating the tenants which
    /// performs the 'Service device' entity of the queueing theory
    /// </summary>
    partial class Elevator : Entity
    {
        public int ID { get; }
        /// <summary>
        /// Maximum number of served tenants at the same time
        /// </summary>
        public int Capacity { get; }
        /// <summary>
        /// Number of served tenants at the current time
        /// </summary>
        public int Occupancy
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
        /// Number of non-occupied place at the current time
        /// </summary>
        public int FreePlace
        {
            get { return Capacity - Occupancy; }
        }

        public int DefaultFloor { get; }
        public int CurrentFloor
        {
            get { return m_currentFloor; }
            private set
            {
                m_currentFloor = value;
                OnChanged();
            }
        }
        public int TargetFloor
        {
            get { return m_targetFloor; }
            private set
            {
                m_targetFloor = value;
                OnChanged();
            }
        }

        /// <summary>
        /// Current movement direction
        /// </summary>
        public Direction Direction { get; private set; }
        /// <summary>
        /// Current state of the elevator's FSM
        /// </summary>
        public State State
        {
            get { return m_stateMachine.CurrentState; }
        }

        /// <summary>
        /// Flag which defines there are the tenants whose
        /// service is over
        /// </summary>
        public bool IsDropOff
        {
            get { return m_tenants.ContainsKey(CurrentFloor); }
        }
        /// <summary>
        /// Flag which defines elevator has calls to handle
        /// </summary>
        public bool HasCalls
        {
            get { return !m_scheduler.IsEmpty; }
        }

        public Elevator(int id, int capacity, int startFloor)
        {
            ID = id;
            Capacity = capacity;
            DefaultFloor = startFloor;
            CurrentFloor = startFloor;
            TargetFloor = startFloor;
            Direction = Direction.Down;
            
            m_scheduler = new CallsScheduler(this);

            m_stateMachine = new StateMachine(this);
            m_tenants = new Dictionary<int, List<Tenant>>();
        }

        public void AddHallcall(Tenant tenant)
        {
            m_scheduler.AddHallcall(tenant);
            m_stateMachine.MoveNext(Edge.Call);

            OnChanged();
        }
        public void AddCarcall(Tenant tenant)
        {
            m_scheduler.AddCarcall(tenant);
            m_stateMachine.MoveNext(Edge.Call);

            OnChanged();
        }

        public void Pickup(List<Tenant> tenants)
        {
            foreach (Tenant tenant in tenants)
            {
                Pickup(tenant);
            }

            OnChanged();
        }
        public void Pickup(Tenant tenant)
        {
            // Checks
            if (FreePlace == 0)
            {
                throw new InvalidOperationException("The elevator is full");
            }
            if (State == State.Move)
            {
                throw new InvalidOperationException("Pick up during the movement");
            }

            // Add
            if (!m_tenants.ContainsKey(tenant.FloorTo))
            {
                m_tenants.Add(tenant.FloorTo, new List<Tenant>());          
            }
            m_tenants[tenant.FloorTo].Add(tenant);
        }
        public List<Tenant> Dropoff()
        {
            // Check
            if (State == State.Move)
            {
                throw new InvalidOperationException("Drop off during the movement");
            }

            // Remove
            List<Tenant> tenants = m_tenants[CurrentFloor];
            m_tenants.Remove(CurrentFloor);

            OnChanged();

            return tenants;
        }
        public void Move()
        {
            // Move
            if (Direction == Direction.Up)
            {
                CurrentFloor++;
            }
            else
            {
                CurrentFloor--;
            }

            m_stateMachine.MoveNext(Edge.ToMove);

            OnChanged();
        }

        public void Reset()
        {
            m_tenants.Clear();

            m_stateMachine.Reset();
            m_scheduler.Reset();
        }

        /// <summary>
        /// Remove last cheduled call
        /// </summary>
        private void RemoveCall()
        {
            // Check
            if (State == State.Wait)
            {
                throw new InvalidOperationException("No calls to remove");
            }

            m_scheduler.RemoveCall(TargetFloor);
        }
        /// <summary>
        /// Set new call to move
        /// </summary>
        private void ScheduleCall()
        {
            // Check
            if (m_scheduler.IsEmpty)
            {
                throw new InvalidOperationException("No calls to schedule");
            }

            // Schedule new call
            TargetFloor = m_scheduler.Schedule();

            OnChanged();
        }

        private int m_currentFloor;
        private int m_targetFloor;

        private readonly CallsScheduler m_scheduler;
        private readonly StateMachine m_stateMachine;

        private Dictionary<int, List<Tenant>> m_tenants;
    }
}
