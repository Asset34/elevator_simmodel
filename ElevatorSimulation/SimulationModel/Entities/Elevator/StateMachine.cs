using System;
using System.Collections.Generic;

using ElevatorSimulation.SimulationModel.Transactions;

namespace ElevatorSimulation.SimulationModel.Entities
{
    enum State
    {
        Idle,
        Halt,
        AtFloor,
        Move
    }

    enum Command
    {
        Call,
        ToHalt,
        ToFloor,
        ToMove,
        ToIdle
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
                m_actions.Add(State.Idle   , OnIdle );
                m_actions.Add(State.Halt   , OnHalt );
                m_actions.Add(State.AtFloor, OnFloor);
                m_actions.Add(State.Move   , OnMove );
            }

            public State GetNext(Command command)
            {
                StateTransition st = new StateTransition(CurrentState, command);

                if (!m_transitions.ContainsKey(st))
                {
                    throw new ArgumentException("Invalid command for current state");
                }

                return m_transitions[st];
            }
            public void MoveNext(Command command)
            {
                CurrentState = GetNext(command);
                m_actions[CurrentState]();
            }

            public void Reset()
            {
                CurrentState = State.Idle;
            }

            /* State actions */
            private void OnIdle()
            {
                // TODO
            }
            private void OnHalt()
            {
                if (m_elevator.m_scheduler.IsEmpty)
                {
                    MoveNext(Command.ToIdle);
                }
                else
                {
                    // Set new direction
                    m_elevator.ScheduleCall();

                    // GOTO next state
                    if (m_elevator.CurrentFloor == m_elevator.TargetFloor)
                    {
                        MoveNext(Command.ToFloor);
                    }
                    else
                    {
                        if (m_elevator.CurrentFloor < m_elevator.TargetFloor)
                        {
                            m_elevator.Direction = Direction.Up;
                        }
                        else
                        {
                            m_elevator.Direction = Direction.Down;
                        }
                    }
                }   
            }
            private void OnFloor()
            {
                m_elevator.RemoveCall();
                
                if (m_elevator.m_scheduler.IsEmpty)
                {
                    MoveNext(Command.ToIdle);
                }
                if (!m_elevator.m_scheduler.IsEmpty)
                {
                    m_elevator.ScheduleCall();

                    // Set opposite direction if needed 
                    if (m_elevator.m_scheduler.IsBorder)
                    {
                        if (m_elevator.Direction == Direction.Up)
                        {
                            m_elevator.Direction = Direction.Down;
                        }
                        else
                        {
                            m_elevator.Direction = Direction.Up;
                        }

                        m_elevator.RemoveCall();
                    }

                    // GOTO next state
                    MoveNext(Command.ToHalt);
                }
            }
            private void OnMove()
            {
                if (m_elevator.CurrentFloor == m_elevator.TargetFloor)
                {
                    MoveNext(Command.ToFloor);
                }
            }

            private readonly Dictionary<State, Action> m_actions
                = new Dictionary<State, Action>();
            private readonly Dictionary<StateTransition, State> m_transitions
                = new Dictionary<StateTransition, State>()
                {
                    { new StateTransition(State.Idle   , Command.Call   ), State.Halt    },
                    { new StateTransition(State.Halt   , Command.ToFloor), State.AtFloor },
                    { new StateTransition(State.Halt   , Command.ToMove ), State.Move    },
                    { new StateTransition(State.Halt   , Command.Call   ), State.Halt    },
                    { new StateTransition(State.Halt   , Command.ToIdle ), State.Idle    },
                    { new StateTransition(State.AtFloor, Command.ToHalt ), State.Halt    },
                    { new StateTransition(State.Move   , Command.ToFloor), State.AtFloor },
                    { new StateTransition(State.Move   , Command.ToMove ), State.Move    },
                    { new StateTransition(State.Move   , Command.Call   ), State.Move    },
                };

            private readonly Elevator m_elevator;
        }
    }
    
}
