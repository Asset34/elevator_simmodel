using System;
using System.Collections.Generic;
using System.Linq;

using ElevatorSimulation.SimulationModel.Entities;
using ElevatorSimulation.SimulationModel.Transactions;

namespace ElevatorSimulation.SimulationModel.Dispatchers.RequestDispatchers
{
    /// <summary>
    /// Dispatcher of floor requests which serves all landing and resulting
    /// car calls in one direction
    /// </summary>
    class CollectiveCallDispatcher : CallDispatcher
    {
        public CollectiveCallDispatcher()
        {
            m_elevator = null;

            m_minCall = int.MaxValue;
            m_maxCall = 0;
        }
        public override void RegisterHallCall(Tenant tenant)
        {
            RedefineBorderValues(tenant.FloorFrom);

            if (tenant.CallType == CallType.Up)
            {
                m_upCalls.Add(tenant.FloorFrom);
            }
            else
            {
                m_downCalls.Add(tenant.FloorFrom);
            }
        }
        public override void RegisterCarCall(Tenant tenant)
        {
            RedefineBorderValues(tenant.FloorTo);

            if (tenant.CallType == CallType.Up)
            {
                m_upCalls.Add(tenant.FloorTo);
            }
            else
            {
                m_downCalls.Add(tenant.FloorTo);
            }
        }
        public override void UnregisterCall(int call)
        {
            DefineNewBorderValues(call);

            if (m_lastCallType == CallType.Up)
            {
                m_upCalls.Remove(call);
            }
            else
            {
                m_downCalls.Remove(call);
            }
        }
        public override Tuple<int, bool> GetCall(int floor)
        {
            // Checks
            if (m_upCalls.Count == 0 && m_downCalls.Count == 0)
            {
                return new Tuple<int, bool>(-1, false);
            }

            int call;
            if (m_elevator.State == ElevatorState.MoveUp ||
                m_elevator.State == ElevatorState.Wait)
            {
                if (floor == m_maxCall)
                {
                    call = m_minCall;
                    m_lastCallType = CallType.Up;

                    if (m_downCalls.Count != 0)
                    {
                        call = m_downCalls.Max();
                        m_lastCallType = CallType.Down;
                    }
                }
                else
                {
                    call = m_maxCall;
                    m_lastCallType = CallType.Down;

                    // Get calls which directed such as elevator movement
                    // and located upper than elevator
                    var upperCalls = m_upCalls.Where(x => x >= floor);
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

                    if (m_upCalls.Count != 0)
                    {
                        call = m_upCalls.Max();
                        m_lastCallType = CallType.Up;
                    }
                }
                else
                {
                    call = m_minCall;
                    m_lastCallType = CallType.Up;

                    // Get calls which directed such as elevator movement
                    // and located lower than elevator
                    var lowerCalls = m_downCalls.Where(x => x <= floor);
                    if (lowerCalls.Any())
                    {
                        call = lowerCalls.Max();
                        m_lastCallType = CallType.Down;
                    }
                }
            }
            //else
            //{
            //    int displacement1 = m_maxCall - floor;
            //    int displacement2 = floor - m_minCall;

            //    if (displacement1 < displacement2)
            //    {
            //        call = m_maxCall;
            //    }
            //    else
            //    {
            //        call = m_minCall;
            //    }
            //}

            return new Tuple<int, bool>(call, true);
        }
        public override void SetElevator(Elevator elevator)
        {
            m_elevator = elevator;
        }
        public override void Reset()
        {
            m_upCalls.Clear();
            m_downCalls.Clear();

            m_maxCall = int.MaxValue;
            m_minCall = 0;
        }

        private Elevator m_elevator;
        private CallType m_lastCallType;
        private int m_maxCall;
        private int m_minCall;
        private HashSet<int> m_upCalls = new HashSet<int>();
        private HashSet<int> m_downCalls = new HashSet<int>();

        private void RedefineBorderValues(int call)
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
        private void DefineNewBorderValues(int unregisteredCall)
        {
            int value1;
            int value2;

            if (unregisteredCall == m_minCall)
            {
                value1 = m_upCalls.Min();
                value2 = m_downCalls.Min();

                m_minCall = Math.Min(value1, value2);
            }
            else if (unregisteredCall == m_maxCall)
            {
                value1 = m_upCalls.Max();
                value2 = m_downCalls.Max();

                m_maxCall = Math.Max(value1, value2);
            }
        }
    }
}
