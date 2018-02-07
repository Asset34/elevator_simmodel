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
                return m_sets[CallType.Up].Count == 0 &&
                       m_sets[CallType.Down].Count == 0;
            }
        }

        public CallsScheduler(Elevator elevator)
        {
            m_elevator = elevator;
        }

        public void AddHallcall(Tenant tenant)
        {   
            m_sets[tenant.CallType].Add(tenant.FloorFrom);
            UpdateBorders(tenant.FloorFrom);
        }
        public void AddCarcall(Tenant tenant)
        {
            m_sets[tenant.CallType].Add(tenant.FloorTo);
            UpdateBorders(tenant.FloorTo);
        }
        public void RemoveCall(int call)
        {
            m_sets[m_lastCallType].Remove(call);
            RedefineBorders(call);
        }

        public int ScheduleCall(int floor)
        {
            // Check
            if (IsEmpty)
            {
                throw new InvalidOperationException("The scheduler is empty");
            }

            int call;
            if (m_elevator.CurrentCallType == CallType.Up)
            {
                if (floor == m_maxCall)
                {
                    call = m_minCall;
                    m_lastCallType = CallType.Up;

                    if (m_sets[CallType.Down].Count != 0)
                    {
                        call = m_sets[CallType.Down].Max();
                        m_lastCallType = CallType.Down;
                    }
                }
                else
                {
                    call = m_maxCall;
                    m_lastCallType = CallType.Down;

                    // Get calls which directed such as elevator movement
                    // and located upper than elevator
                    var upperCalls = m_sets[CallType.Up].Where(x => x >= floor);
                    if (upperCalls.Any())
                    {
                        call = upperCalls.Min();
                        m_lastCallType = CallType.Up;
                    }
                }   
            }
            else 
            {
                if (floor == m_minCall)
                {
                    call = m_maxCall;
                    m_lastCallType = CallType.Down;

                    if (m_sets[CallType.Up].Count != 0)
                    {
                        call = m_sets[CallType.Up].Max();
                        m_lastCallType = CallType.Up;
                    }
                }
                else
                {
                    call = m_minCall;
                    m_lastCallType = CallType.Up;

                    // Get calls which directed such as elevator movement
                    // and located lower than elevator
                    var lowerCalls = m_sets[CallType.Down].Where(x => x <= floor);
                    if (lowerCalls.Any())
                    {
                        call = lowerCalls.Max();
                        m_lastCallType = CallType.Down;
                    }
                }
            }

            return call;
        }

        public void Reset()
        {
            m_sets[CallType.Up].Clear();
            m_sets[CallType.Down].Clear();

            ResetBorders();
        }

        private void UpdateBorders(int call)
        {
            if (call < m_minCall)
            {
                m_minCall = call;
            }
            else if (call > m_maxCall)
            {
                m_maxCall = call;
            }
        }
        private void RedefineBorders(int unregisteredCall)
        {
            if (IsEmpty)
            {
                ResetBorders();
            }
            else
            {
                // Redefine only min
                if (unregisteredCall == m_minCall)
                {
                    m_minCall = m_sets[CallType.Up].Union(m_sets[CallType.Down]).Min();
                }
                // Redefine only max
                else if (unregisteredCall == m_maxCall)
                {
                    m_maxCall = m_sets[CallType.Up].Union(m_sets[CallType.Down]).Max();
                }
            }
        }
        private void ResetBorders()
        {
            m_maxCall = 0;
            m_minCall = int.MaxValue;
        }

        private Elevator m_elevator;

        private CallType m_lastCallType;
        private int m_maxCall = 0;
        private int m_minCall = int.MaxValue;

        private readonly Dictionary<CallType, HashSet<int>> m_sets
            = new Dictionary<CallType, HashSet<int>>()
        {
            { CallType.Up,   new HashSet<int>() },
            { CallType.Down, new HashSet<int>() }
        };
    }
}
