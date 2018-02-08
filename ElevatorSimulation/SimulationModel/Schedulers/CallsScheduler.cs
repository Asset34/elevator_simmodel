using System;
using System.Collections.Generic;
using System.Linq;

using ElevatorSimulation.SimulationModel.Entities;
using ElevatorSimulation.SimulationModel.Transactions;

namespace ElevatorSimulation.SimulationModel.Schedulers
{
    /// <summary>
    /// Scheduler of floor requests which serves all landing and resulting
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

        public CallsScheduler(Elevator elevator)
        {
            m_elevator = elevator;
        }

        public void AddHallcall(Tenant tenant)
        {   
            m_sets[tenant.Direction].Add(tenant.FloorFrom);

            RedefineBorders();
        }
        public void AddCarcall(Tenant tenant)
        {
            m_sets[tenant.Direction].Add(tenant.FloorTo);

            RedefineBorders();
        }
        public void RemoveCall(int call)
        {
            m_sets[m_lastDirection].Remove(call);

            RedefineBorders();
        }

        public int Schedule(int floor)
        {
            // Check
            if (IsEmpty)
            {
                throw new InvalidOperationException("The scheduler is empty");
            }

            int call;
            if (m_elevator.CurrentDirection == Direction.Up)
            {
                if (floor == m_maxCall)
                {
                    call = m_minCall;
                    m_lastDirection = Direction.Up;

                    if (m_sets[Direction.Down].Count != 0)
                    {
                        call = m_sets[Direction.Down].Max();
                        m_lastDirection = Direction.Down;
                    }
                }
                else
                {
                    call = m_maxCall;
                    m_lastDirection = Direction.Down;

                    // Get calls which directed such as elevator movement
                    // and located upper than elevator
                    var upperCalls = m_sets[Direction.Up].Where(x => x >= floor);
                    if (upperCalls.Any())
                    {
                        call = upperCalls.Min();
                        m_lastDirection = Direction.Up;
                    }
                }   
            }
            else 
            {
                if (floor == m_minCall)
                {
                    call = m_maxCall;
                    m_lastDirection = Direction.Down;

                    if (m_sets[Direction.Up].Count != 0)
                    {
                        call = m_sets[Direction.Up].Max();
                        m_lastDirection = Direction.Up;
                    }
                }
                else
                {
                    call = m_minCall;
                    m_lastDirection = Direction.Up;

                    // Get calls which directed such as elevator movement
                    // and located lower than elevator
                    var lowerCalls = m_sets[Direction.Down].Where(x => x <= floor);
                    if (lowerCalls.Any())
                    {
                        call = lowerCalls.Max();
                        m_lastDirection = Direction.Down;
                    }
                }
            }

            return call;
        }

        public void Reset()
        {
            m_sets[Direction.Up].Clear();
            m_sets[Direction.Down].Clear();
        }

        private void RedefineBorders()
        {
            m_minCall = m_sets[Direction.Up].Union(m_sets[Direction.Down]).Min();
            m_maxCall = m_sets[Direction.Up].Union(m_sets[Direction.Down]).Max();
        }

        private Elevator m_elevator;

        private Direction m_lastDirection;
        private int m_maxCall/* = 0*/;
        private int m_minCall/* = int.MaxValue*/;

        private readonly Dictionary<Direction, HashSet<int>> m_sets
            = new Dictionary<Direction, HashSet<int>>()
        {
            { Direction.Up,   new HashSet<int>() },
            { Direction.Down, new HashSet<int>() }
        };
    }
}
