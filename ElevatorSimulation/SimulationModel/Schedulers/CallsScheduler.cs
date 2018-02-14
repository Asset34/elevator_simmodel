using System;
using System.Collections.Generic;
using System.Linq;

using ElevatorSimulation.SimulationModel.Entities;

namespace ElevatorSimulation.SimulationModel.Schedulers
{
    /// <summary>
    /// Scheduler of floor requests which serves all landing and Experimenting
    /// car calls in one direction
    /// </summary>
    class CallsScheduler
    {
        public bool IsEmpty
        {
            get
            {
                return m_sets[Direction.Up].Count == 0 &&
                       m_sets[Direction.Down].Count == 0;
            }
        }

        public bool IsBorder { get; private set; }

        public CallsScheduler(Elevator elevator)
        {
            m_elevator = elevator;
        }

        public void AddHallcall(Tenant tenant)
        {   
            m_sets[tenant.Direction].Add(tenant.FloorFrom);

            RedefineLimits();
        }
        public void AddCarcall(Tenant tenant)
        {
            m_sets[tenant.Direction].Add(tenant.FloorTo);

            RedefineLimits();
        }
        public void RemoveCall(int call)
        {
            //m_sets[m_lastDirection].Remove(call);
            m_sets[m_elevator.Direction].Remove(call);

            RedefineLimits();
        }

        public int Schedule()
        {
            // Check
            if (IsEmpty)
            {
                throw new InvalidOperationException("The scheduler is empty");
            }

            int call;
            if (m_elevator.Direction == Direction.Up)
            {
                if (m_elevator.CurrentFloor == m_top)
                {
                    call = m_bottom;

                    if (m_sets[Direction.Down].Count != 0)
                    {
                        call = m_sets[Direction.Down].Max();
                    }
                }
                else
                {
                    call = m_top;

                    // Get calls which directed such as elevator movement
                    // and located upper than elevator
                    var upperCalls = m_sets[Direction.Up].Where(x => x >= m_elevator.CurrentFloor);
                    if (upperCalls.Any())
                    {
                        call = upperCalls.Min();
                    }
                }   
            }
            else 
            {
                if (m_elevator.CurrentFloor == m_bottom)
                {
                    call = m_top;

                    if (m_sets[Direction.Up].Count != 0)
                    {
                        call = m_sets[Direction.Up].Max();
                    }
                }
                else
                {
                    call = m_bottom;

                    // Get calls which directed such as elevator movement
                    // and located lower than elevator
                    var lowerCalls = m_sets[Direction.Down].Where(x => x <= m_elevator.CurrentFloor);
                    if (lowerCalls.Any())
                    {
                        call = lowerCalls.Max();
                    }
                }
            }

            // Check if next call is border
            RedefineBorder();

            return call;
        }

        public void Reset()
        {
            m_sets[Direction.Up].Clear();
            m_sets[Direction.Down].Clear();
        }

        private void RedefineLimits()
        {
            var union = m_sets[Direction.Up].Union(m_sets[Direction.Down]);

            if (union.Any())
            {
                m_bottom = union.Min();
                m_top = union.Max();
            }
        }
        private void RedefineBorder()
        {
            IsBorder = false;

            if (m_elevator.Direction == Direction.Down)
            {
                var lower = m_sets[Direction.Down].Where(call => (call <= m_elevator.TargetFloor));
                if (!lower.Any())
                {
                    IsBorder = true;
                }
            }
            else
            {
                var upper = m_sets[Direction.Up].Where(call => (call >= m_elevator.TargetFloor));
                if (!upper.Any())
                {
                    IsBorder = true;
                }
            }
        }

        private int m_top;
        private int m_bottom;

        private readonly Dictionary<Direction, HashSet<int>> m_sets
            = new Dictionary<Direction, HashSet<int>>()
        {
            { Direction.Up,   new HashSet<int>() },
            { Direction.Down, new HashSet<int>() }
        };

        private Elevator m_elevator;
    }
}
