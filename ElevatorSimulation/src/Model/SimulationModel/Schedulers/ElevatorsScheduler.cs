using System;
using System.Collections.Generic;
using System.Linq;

using ElevatorSimulation.Model.SimulationModel.Entities;
using ElevatorSimulation.Model.SimulationModel.Transactions;

namespace ElevatorSimulation.Model.SimulationModel.Schedulers
{
    /// <summary>
    /// Scheduler of group of elevators which uses the simplest
    /// group algorithm based on allocating the closest car
    /// to the current call
    /// </summary>
    class ElevatorsScheduler
    {
        public bool IsEmpty
        {
            get
            {
                return m_elevators.Count == 0;
            }
        }

        public ElevatorsScheduler(int numFloors)
        {
            m_numFloors = numFloors;
        }

        public void Add(Elevator elevator)
        {
            m_elevators.Add(elevator);
        }
        public void Remove(Elevator elevator)
        {
            m_elevators.Add(elevator);
        }
        public Elevator Schedule(Tenant tenant)
        {
            // Check
            if (IsEmpty)
            {
                throw new InvalidOperationException("Scheduler is empty");
            }

            List<KeyValuePair<Elevator, int>> priorityElevators 
                = new List<KeyValuePair<Elevator, int>>();

            // Set priority to each elevator
            int displacement;
            int priority;
            foreach (Elevator elevator in m_elevators)
            {
                displacement = tenant.FloorFrom - elevator.CurrentFloor;

                // Compute priority
                if (displacement > 0 && elevator.Direction == Direction.Up ||
                    displacement < 0 && elevator.Direction == Direction.Down)
                {
                    if (elevator.Direction == tenant.Direction)
                    {
                        priority = m_numFloors + 3 - Math.Abs(displacement);
                    }
                    else
                    {
                        priority = m_numFloors + 2 - Math.Abs(displacement);
                    }
                }
                else if (elevator.State == State.Wait)
                {
                    priority = 2;
                }
                else
                {
                    priority = 1;
                }

                // Add to sorted dictionary
                priorityElevators.Add(new KeyValuePair<Elevator, int>(elevator, priority));
            }

            // Sort list by priority
            var sortedPriorityElevators = priorityElevators.OrderByDescending(x => x.Value);

            // Search of the available elevator with highest priority
            foreach (KeyValuePair<Elevator, int> pair in sortedPriorityElevators)
            {
                if (pair.Key.FreePlace > 0)
                {
                    return pair.Key;
                }
            }

            return sortedPriorityElevators.First().Key;
        }

        private List<Elevator> m_elevators = new List<Elevator>();
        private readonly int m_numFloors;
    }
}
