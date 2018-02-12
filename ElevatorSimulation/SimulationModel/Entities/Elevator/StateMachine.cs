using System;
using System.Collections.Generic;

using ElevatorSimulation.SimulationModel.Transactions;

namespace ElevatorSimulation.SimulationModel.Entities
{
    enum State
    {
        Wait,
        Halt,
        Target,
        Move
    }

    enum Edge
    {
        Call,
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

                CurrentState = State.Wait;

                // Create action table
                m_actions.Add(State.Wait  , OnWait  );
                m_actions.Add(State.Halt  , OnHalt  );
                m_actions.Add(State.Target, OnTarget);
                m_actions.Add(State.Move  , OnMove  );
            }

            public State GetNext(Edge edge)
            {
                StateTransition st = new StateTransition(CurrentState, edge);

                if (!m_transitions.ContainsKey(st))
                {
                    throw new ArgumentException("Invalid command for current state");
                }

                return m_transitions[st];
            }
            public void MoveNext(Edge edge)
            {
                CurrentState = GetNext(edge);
                m_actions[CurrentState]();
            }

            public void Reset()
            {
                CurrentState = State.Wait;
            }

            /* State actions */
            private void OnWait()
            {
                if (!m_elevator.m_scheduler.IsEmpty)
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

                    MoveNext(Edge.ToHalt);
                }
            }
            private void OnHalt()
            {
                if (m_elevator.CurrentFloor == m_elevator.TargetFloor)
                {
                    MoveNext(Edge.ToTarget);
                }
                else
                {
                    MoveNext(Edge.ToMove);
                }
            }
            private void OnTarget()
            {
                m_elevator.RemoveCall();

                if (m_elevator.m_scheduler.IsBorder)
                {
                    // Set opposite direction if needed 
                    if (m_elevator.Direction == Direction.Up)
                    {
                        m_elevator.Direction = Direction.Down;
                    }
                    else
                    {
                        m_elevator.Direction = Direction.Up;
                    }
                }

                MoveNext(Edge.ToWait);
            }
            private void OnMove()
            {
                if (m_elevator.CurrentFloor == m_elevator.TargetFloor)
                {
                    MoveNext(Edge.ToTarget);
                }
                else
                {
                    m_elevator.ScheduleCall();
                }
            }

            private readonly Dictionary<State, Action> m_actions
                = new Dictionary<State, Action>();
            private readonly Dictionary<StateTransition, State> m_transitions
                = new Dictionary<StateTransition, State>()
                {
                    { new StateTransition(State.Wait  , Edge.Call    ), State.Wait   },
                    { new StateTransition(State.Wait  , Edge.ToHalt  ), State.Halt   },
                    { new StateTransition(State.Target, Edge.ToWait  ), State.Wait   },
                    { new StateTransition(State.Halt  , Edge.ToTarget), State.Target },
                    { new StateTransition(State.Halt  , Edge.ToMove  ), State.Move   },
                    { new StateTransition(State.Move  , Edge.ToMove  ), State.Move   },
                    { new StateTransition(State.Move  , Edge.ToTarget), State.Target },
                    { new StateTransition(State.Move  , Edge.Call    ), State.Move   },
                };

            private readonly Elevator m_elevator;
        }
    }
    
}
