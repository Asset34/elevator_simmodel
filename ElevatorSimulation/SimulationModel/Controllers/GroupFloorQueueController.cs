﻿using System;
using System.Collections.Generic;

using ElevatorSimulation.SimulationModel.Transactions;
using ElevatorSimulation.SimulationModel.Entities;

namespace ElevatorSimulation.SimulationModel.Controllers
{
    /// <summary>
    /// Controller of the complex of tenant floor queues
    /// </summary>
    class GroupFloorQueueController : Resettable
    {
        public List<FloorQueue> Queues
        {
            get { return m_queues; }
        }

        public void Enqueue(int floor, Tenant tenant)
        {
            m_queues[floor - 1].Enqueue(tenant);
        }
        public Tenant Dequeue(int floor, CallType callType)
        {
            return m_queues[floor - 1].Dequeue(callType);
        }
        public void Reset()
        {
            foreach (FloorQueue floorQueue in m_queues)
            {
                floorQueue.Reset();
            }
        }

        private List<FloorQueue> m_queues = new List<FloorQueue>();
    }
}
