using System;
using System.Collections.Generic;

using ElevatorSimulation.Model.SimulationModel.Transactions;

namespace ElevatorSimulation.Model.SimulationModel.Entities
{
    enum State
    {
        Idle,
        Wait,
        Halt,
        Target,
        Move
    }

    enum Edge
    {
        Call,
        ToIdle,
        ToHalt,
        ToTarget,
        ToWait,
        ToMove,
    }

    partial class Elevator
    {
        /// <summary>
        /// Finite-State machine(FSM) of the elevator
        /// </summary>
        private partial class StateMachine
        {
            public State CurrentState { get; private set; }

            public StateMachine(Elevator elevator)
            {
                m_elevator = elevator;

                CurrentState = State.Idle;

                // Create action table
                m_actions.Add(State.Wait  , OnWait  );
                m_actions.Add(State.Halt  , OnHalt  );
                m_actions.Add(State.Target, OnTarget);
                m_actions.Add(State.Move  , OnMove  );
                m_actions.Add(State.Idle  , OnIdle  );
            }

            /// <summary>
            /// Get next state according to the edge
            /// </summary>
            /// <param name="edge"></param>
            /// <returns></returns>
            public State GetNext(Edge edge)
            {
                StateTransition st = new StateTransition(CurrentState, edge);

                if (!m_transitions.ContainsKey(st))
                {
                    throw new ArgumentException("Invalid command for current state");
                }

                return m_transitions[st];
            }
            /// <summary>
            /// Move to the next state according to the edge
            /// </summary>
            /// <param name="edge"></param>
            public void MoveNext(Edge edge)
            {
                CurrentState = GetNext(edge);
                m_actions[CurrentState]();
            }

            public void Reset()
            {
                CurrentState = State.Idle;
            }

            /* State actions */
            private void OnIdle()
            {

            }
            private void OnWait()
            {
                //if (!m_elevator.m_scheduler.IsEmpty)
                //{
                //    // Set direction
                //    if (m_elevator.CurrentFloor < m_elevator.TargetFloor)
                //    {
                //        m_elevator.Direction = Direction.Up;
                //    }
                //    else if (m_elevator.CurrentFloor > m_elevator.TargetFloor)
                //    {
                //        m_elevator.Direction = Direction.Down;
                //    }
                //}
            }
            private void OnHalt()
            {
                if (m_elevator.HasCalls)
                {
                    m_elevator.ScheduleCall();

                    // Set direction
                    if (m_elevator.CurrentFloor < m_elevator.TargetFloor)
                    {
                        m_elevator.Direction = Direction.Up;
                    }
                    else if (m_elevator.CurrentFloor > m_elevator.TargetFloor)
                    {
                        m_elevator.Direction = Direction.Down;
                    }

                    // To the target
                    if (m_elevator.CurrentFloor == m_elevator.TargetFloor)
                    {
                        MoveNext(Edge.ToTarget);
                    }
                    else
                    {
                        MoveNext(Edge.ToMove);
                    }
                }
                else
                {
                    MoveNext(Edge.ToWait);
                }
            }
            private void OnTarget()
            {
                m_elevator.RemoveCall();

                if (m_elevator.m_scheduler.IsTopBorder)
                {
                    m_elevator.Direction = Direction.Down;
                    m_elevator.RemoveCall();
                }
                else if (m_elevator.m_scheduler.IsBottomBorder)
                {
                    m_elevator.Direction = Direction.Up;
                    m_elevator.RemoveCall();
                }

                if (m_elevator.HasCalls)
                {
                    MoveNext(Edge.ToWait);
                }
                else
                {
                    MoveNext(Edge.ToIdle);
                }
            }
            private void OnMove()
            {
                if (m_elevator.CurrentFloor == m_elevator.TargetFloor)
                {
                    MoveNext(Edge.ToTarget);
                }
            }

            private readonly Dictionary<State, Action> m_actions
                = new Dictionary<State, Action>();
            private readonly Dictionary<StateTransition, State> m_transitions
                = new Dictionary<StateTransition, State>()
                {
                    { new StateTransition(State.Idle  , Edge.Call    ), State.Wait   },
                    { new StateTransition(State.Wait  , Edge.Call    ), State.Wait   },
                    { new StateTransition(State.Wait  , Edge.ToHalt  ), State.Halt   },
                    { new StateTransition(State.Target, Edge.ToWait  ), State.Wait   },
                    { new StateTransition(State.Target, Edge.ToIdle  ), State.Idle   },
                    { new StateTransition(State.Halt  , Edge.ToTarget), State.Target },
                    { new StateTransition(State.Halt  , Edge.ToMove  ), State.Move   },
                    { new StateTransition(State.Halt  , Edge.ToWait  ), State.Wait   },
                    { new StateTransition(State.Move  , Edge.ToMove  ), State.Move   },
                    { new StateTransition(State.Move  , Edge.ToTarget), State.Target },
                    { new StateTransition(State.Move  , Edge.Call    ), State.Move   },
                };

            private readonly Elevator m_elevator;
        }
    }
    
}
