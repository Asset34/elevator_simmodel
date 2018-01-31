using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ElevatorSimulation.SimulationModel.Entities;

namespace ElevatorSimulation.SimulationModel.Dispatchers.ElevatorGroupDispatchers
{
    /// <summary>
    /// Dispatcher of group of elevators which uses the simplest
    /// group algorithm based on allocating the closest car
    /// to the current request
    /// </summary>
    class NearestCarDispatcher : ElevatorGroupDispatcher
    {
        public NearestCarDispatcher(int numFloors)
        {
            m_numFloors = numFloors;
        }
        public override void Register(Elevator element)
        {
            m_elevators.Add(element);
        }
        public override void Unregister(Elevator element)
        {
            m_elevators.Add(element);
        }
        public override Elevator GetElevator(Request request)
        {
            List<KeyValuePair<Elevator, int>> priorityElevators = new List<KeyValuePair<Elevator, int>>();

            // Set priority to each elevator
            int displacement;
            int priority;
            foreach (Elevator elevator in m_elevators)
            {
                displacement = request.Floor - elevator.CurrentFloor;

                // Compute priority
                if (displacement > 0 && elevator.State == ElevatorState.MoveUp ||
                    displacement < 0 && elevator.State == ElevatorState.MoveDown)
                {
                    if (elevator.State == ElevatorState.MoveUp &&
                        request.Type == RequestType.Up)
                    {
                        priority = m_numFloors + 3 - Math.Abs(displacement);
                    }
                    else
                    {
                        priority = m_numFloors + 2 - Math.Abs(displacement);
                    }
                }
                else if (elevator.State == ElevatorState.Wait)
                {
                    priority = 2;
                }
                else
                {
                    priority = 1;
                }

                // Add to sorted dictinary
                priorityElevators.Add(new KeyValuePair<Elevator, int>(elevator, priority));
            }

            // Sort list by priority
            var sortedPriorityElevators = priorityElevators.OrderByDescending(x => x.Value);

            // Search of the available elevator with highest priority
            foreach (KeyValuePair<Elevator, int> pair in sortedPriorityElevators)
            {
                if (pair.Key.FreeSpace > 0)
                {
                    return pair.Key;
                }
            }

            return null;
        }
        public override void Reset()
        {
            m_elevators.Clear();
        }

        private List<Elevator> m_elevators = new List<Elevator>();
        private readonly int m_numFloors;
    }
}
