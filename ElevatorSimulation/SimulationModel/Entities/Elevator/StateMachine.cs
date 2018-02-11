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
        Hallcall,
        Carcall,
        ToHalt,
        ToFloor,
        ToMove,
        ToIdle
    }

    partial class Elevator
    {
        /// <summary>
        /// Finite-State machine of the elevator
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
                m_actions.Add(State.Move   , OnHalt );
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
            public State MoveNext(Command command)
            {
                return GetNext(command);
            }

            /* State actions */
            private void OnIdle()
            {
                // TODO
            }
            private void OnHalt()
            {
                // Set new direction
                m_elevator.ScheduleCall();

                // GOTO next state
                if (m_elevator.CurrentFloor == m_elevator.DestinationFloor)
                {
                    MoveNext(Command.ToFloor);
                }
                else
                {
                    MoveNext(Command.ToMove);
                }

                // Action
                m_actions[CurrentState]();
            }
            private void OnFloor()
            {
                m_elevator.RemoveCall();

                // Set new direction if needed
                if (m_elevator.CurrentFloor == m_elevator.m_scheduler.Top)
                {
                    m_elevator.Direction = Direction.Down;
                }
                else if (m_elevator.CurrentFloor == m_elevator.m_scheduler.Bottom)
                {
                    m_elevator.Direction = Direction.Up;
                }

                // GOTO next state
                if (m_elevator.m_scheduler.IsEmpty)
                {
                    MoveNext(Command.ToIdle);
                }
                else
                {
                    MoveNext(Command.ToHalt);
                }

                // Action
                m_actions[CurrentState]();
            }
            private void OnMove()
            {
                if (m_elevator.CurrentFloor == m_elevator.DestinationFloor)
                {
                    MoveNext(Command.ToFloor);

                    // Action
                    m_actions[CurrentState]();
                }
            }

            private readonly Dictionary<State, Action> m_actions
                = new Dictionary<State, Action>();
            private readonly Dictionary<StateTransition, State> m_transitions
                = new Dictionary<StateTransition, State>()
                {
                    { new StateTransition(State.Idle   , Command.Hallcall), State.Halt    },
                    { new StateTransition(State.Idle   , Command.Carcall) , State.Halt    },
                    { new StateTransition(State.Halt   , Command.ToIdle)  , State.Idle    },
                    { new StateTransition(State.Halt   , Command.ToFloor) , State.AtFloor },
                    { new StateTransition(State.Halt   , Command.ToMove)  , State.Move    },
                    { new StateTransition(State.Halt   , Command.Hallcall), State.Halt    },
                    { new StateTransition(State.Halt   , Command.Carcall) , State.Halt    },
                    { new StateTransition(State.AtFloor, Command.ToHalt)  , State.Halt    },
                    { new StateTransition(State.Move   , Command.ToFloor) , State.AtFloor },
                    { new StateTransition(State.Move   , Command.Hallcall), State.Move    },
                    { new StateTransition(State.Move   , Command.Carcall) , State.Move    },
                };

            private readonly Elevator m_elevator;
        }
    }
    
}
