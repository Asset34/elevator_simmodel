﻿namespace ElevatorSimulation.SimulationModel.Entities
{
    partial class Elevator
    {
        private partial class StateMachine
        {
            /// <summary>
            /// Performs aggregate of the state and edge of this state
            /// </summary>
            private class StateTransition
            {
                public State State { get; set; }
                public Edge Edge { get; set; }

                public StateTransition(State state, Edge edge)
                {
                    State = state;
                    Edge = edge;
                }

                public override int GetHashCode()
                {
                    return 17 + 31 * State.GetHashCode() + 31 * Edge.GetHashCode();
                }
                public override bool Equals(object obj)
                {
                    StateTransition other = obj as StateTransition;

                    return State == other.State && Edge == other.Edge;
                }
            }
        }
    }
}
