namespace ElevatorSimulation.SimulationModel.Entities
{
    partial class Elevator
    {
        private partial class StateMachine
        {
            /// <summary>
            /// Performs the each edge of the each state
            /// </summary>
            private class StateTransition
            {
                public State State { get; }
                public Command Command { get; }

                public StateTransition(State state, Command command)
                {
                    State = state;
                    Command = command;
                }

                public override int GetHashCode()
                {
                    return 17 + 31 * State.GetHashCode() + 31 * Command.GetHashCode();
                }
                public override bool Equals(object obj)
                {
                    StateTransition other = obj as StateTransition;

                    return State == other.State && Command == other.Command;
                }
            }
        }
    }
}
