﻿using System;
using System.Collections.Generic;
using System.Linq;

using ElevatorSimulation.SimulationModel.Entities;
using ElevatorSimulation.SimulationModel.Transactions;

namespace ElevatorSimulation.SimulationModel.Schedulers.GlobalSchedulers
{
    /// <summary>
    /// Scheduler of group of elevators which uses the simplest
    /// group algorithm based on allocating the closest car
    /// to the current call
    /// </summary>
    class NearestCarScheduler : GlobalScheduler
    {
        public override bool IsEmpty
        {
            get
            {
                return m_elevators.Count == 0;
            }
        }

        public NearestCarScheduler(int numFloors)
        {
            m_numFloors = numFloors;
        }

        public override void Add(Elevator element)
        {
            m_elevators.Add(element);
        }
        public override void Remove(Elevator element)
        {
            m_elevators.Add(element);
        }

        public override int GetElevator(Tenant tenant)
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
                if (displacement > 0 && elevator.CurrentCallType == CallType.Up ||
                    displacement < 0 && elevator.CurrentCallType == CallType.Down)
                {
                    if (elevator.CurrentCallType == tenant.CallType)
                    {
                        priority = m_numFloors + 3 - Math.Abs(displacement);
                    }
                    else
                    {
                        priority = m_numFloors + 2 - Math.Abs(displacement);
                    }
                }
                else if (elevator.IsIdle)
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
                if (pair.Key.FreeCount > 0)
                {
                    return pair.Key.ID;
                }
            }

            return -1;
        }

        public override void Reset()
        {
            m_elevators.Clear();
        }

        private List<Elevator> m_elevators = new List<Elevator>();
        private readonly int m_numFloors;
    }
}
