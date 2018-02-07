using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ElevatorSimulation.SimulationModel.Events;
using ElevatorSimulation.SimulationModel.Distributions;
using ElevatorSimulation.SimulationModel.Transactions;
using ElevatorSimulation.SimulationModel.Entities;

namespace ElevatorSimulation.SimulationModel.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    class EventController
    {
        public EventController(
            TenantGeneratorsController generatorsController,
            TenantQueuesController queuesController,
            ElevatorsController elevatorsController,
            Dictionary<int, Distribution> generationDistributions,
            Dictionary<int, Distribution> movementDistributions
            )
        {
            m_generatorsController = generatorsController;
            m_queuesController = queuesController;
            m_elevatorsController = elevatorsController;

            m_generationDistributions = generationDistributions;
            m_movementDistributions = movementDistributions;
        }

        /// <summary>
        /// 
        /// </summary>
        public void InitializeEvents()
        {

        }
        /// <summary>
        /// Get the nearest event
        /// </summary>
        /// <returns></returns>
        public Event GetNextEvent()
        {
            return RemoveMinEvent();
        }
        
        /* Events handle */
        public void Handle(NewTenant ev)
        {
            // Generate new tenant
            Tenant tenant = m_generatorsController.Generators[ev.Floor].Generate();

            TenantQueue queue = m_queuesController.Queues[ev.Floor];

            // Add new tenant to the queue
            queue.Enqueue(tenant);

            // Add result to the event
            ev.TenantID = tenant.ID;

            // Add new events
            Event newEv;

            // Add hallcal if it wasn't done before
            if (!queue.GetHallcallStatus(tenant.CallType))
            {
                newEv = CreateNewHallcallEvent(tenant);
                AddEvent(newEv);
            }

            newEv = CreateNewTenantEvent(ev.Floor);
            AddEvent(newEv);
        }           
        public void Handle(NewHallcall ev)
        {
            int id = m_elevatorsController.SelectElevator(ev.Tenant);
            Elevator elevator = m_elevatorsController.Elevators[id];

            // "Wake" elevator
            if (elevator.IsIdle)
            {
                ElevatorMovementControl(elevator);
            }

            elevator.AddHallcall(ev.Tenant);

            // Add result to the event
            ev.ElevatorID = id;
        }
        public void Handle(NewCarcall ev)
        {
            Elevator elevator = m_elevatorsController.Elevators[ev.ElevatorID];

            // "Wake" elevator
            if (elevator.IsIdle)
            {
                ElevatorMovementControl(elevator);
            }

            elevator.AddCarcall(ev.Tenant);
        }
        public void Handle(PickUp ev)
        {
            // Get list of tenants
            List<Tenant> tenants = new List<Tenant>();
            Elevator elevator = m_elevatorsController.Elevators[ev.ElevatorID];
            TenantQueue queue = m_queuesController.Queues[elevator.CurrentFloor];
            Tenant tenant;
            while (queue.GetHallcallStatus(elevator.CurrentCallType) &&
                   elevator.FreeCount > 0)
            {
                tenant = queue.Dequeue(elevator.CurrentCallType);
                tenants.Add(tenant);
            }

            // Add tenant to the elevator
            elevator.PickUp(tenants);

            // Add result to the event
            ev.Floor = queue.Floor;
            ev.Tenants = tenants;

            // Add new events
            Event newEv;

            // If not all tenants was picked up
            // then repeat call
            if (queue.GetHallcallStatus(elevator.CurrentCallType))
            {
                Tenant peekTenant = queue.Peek(elevator.CurrentCallType);
                newEv = CreateNewHallcallEvent(peekTenant);

                AddEvent(newEv);
            }
            
            // Add unduplicated carcall from tenants
            var tenantsWithUnduplicatedCalls
                = tenants.GroupBy(x => x.FloorTo).Select(x => x.First());
            foreach (Tenant t in tenantsWithUnduplicatedCalls)
            {
                newEv = CreateNewCarcallEvent(elevator.ID, t);
                AddEvent(newEv);
            }
        }
        public void Handle(DropOff ev)
        {
            Elevator elevator = m_elevatorsController.Elevators[ev.ElevatorID];
            List<Tenant> tenants = elevator.DropOff();

            // Add result to the event
            ev.Floor = elevator.CurrentFloor;
            ev.Tenants = tenants;
        }
        public void Handle(ElevatorMovement ev)
        {
            Elevator elevator = m_elevatorsController.Elevators[ev.ElevatorID];

            elevator.Move();

            // Set results of events
            ev.Floor = elevator.CurrentFloor;

            // Continue
            ElevatorMovementControl(elevator);
        }

        /* Event create */
        private NewTenant CreateNewTenantEvent(int floor)
        {
            // Generate time
            int dtime = m_generationDistributions[floor].GetValue();

            // Compute event occurence time
            int time = ElevatorSimulationModel.Instance.Time + dtime;

            return new NewTenant(time, this)
            {
                Floor = floor
            };
        }
        private ElevatorMovement CreateElevatorMovementEvent(int elevatorID)
        {
            // Generate time
            int dtime = m_movementDistributions[elevatorID].GetValue();

            // Compute event occurence time
            int time = ElevatorSimulationModel.Instance.Time + dtime;

            return new ElevatorMovement(time, this)
            {
                ElevatorID = elevatorID
            };
        }
        private NewHallcall CreateNewHallcallEvent(Tenant tenant)
        {
            // Get event occurence time
            int time = ElevatorSimulationModel.Instance.Time;

            return new NewHallcall(time, this)
            {
                Tenant = tenant
            };
        }
        private NewCarcall CreateNewCarcallEvent(int elevatorID, Tenant tenant)
        {
            // Get event occurence time
            int time = ElevatorSimulationModel.Instance.Time;

            return new NewCarcall(time, this)
            {
                ElevatorID = elevatorID,
                Tenant = tenant
            };
        }
        private PickUp CreatePickUpEvent(int elevatorID)
        {
            // Get event occurencet ime
            int time = ElevatorSimulationModel.Instance.Time;

            return new PickUp(time, this)
            {
                ElevatorID = elevatorID
            };
        }
        private DropOff CreateDropOffEvent(int elevatorID)
        {
            // Get event occurencet ime
            int time = ElevatorSimulationModel.Instance.Time;

            return new DropOff(time, this)
            {
                ElevatorID = elevatorID
            };
        }
        
        /* Additional */
        private void ElevatorMovementControl(Elevator elevator)
        { 
            Event newEv;

            if (elevator.IsReached)
            {
                // Remove handled call
                elevator.RemoveCall(elevator.CurrentFloor);

                // Set next call
                if (!elevator.IsIdle)
                {
                    elevator.SetCall();
                }

                // Dropp off tenants
                DropOffControl(elevator);

                // Pick up tenants
                PickUpControl(elevator);
            }
            else if (!elevator.IsIdle)
            {
                // Move next
                newEv = CreateElevatorMovementEvent(elevator.ID);
                AddEvent(newEv);
            }
        }
        private void PickUpControl(Elevator elevator)
        {

        }
        private void DropOffControl(Elevator elevator)
        {

        }

        private void AddEvent(Event ev)
        {
            if (!m_events.ContainsKey(ev.Time))
            {
                m_events.Add(ev.Time, new Queue<Event>());
            }

            m_events[ev.Time].Enqueue(ev);
        }
        private Event RemoveMinEvent()
        {
            int minKey = m_events.Keys.Min();
            Event ev = m_events[minKey].Dequeue();

            if (m_events[minKey].Count == 0)
            {
                m_events.Remove(minKey);
            }

            return ev;
        }

        public void Reset()
        {
            m_generatorsController.Reset();
            m_queuesController.Reset();
            m_elevatorsController.Reset();

            m_events.Clear();
        }

        private Dictionary<int, Queue<Event>> m_events = new Dictionary<int, Queue<Event>>();

        /* Controllers */
        private readonly TenantGeneratorsController m_generatorsController;
        private readonly TenantQueuesController m_queuesController;
        private readonly ElevatorsController m_elevatorsController;

        /* Distributions */
        private readonly Dictionary<int, Distribution> m_generationDistributions;
        private readonly Dictionary<int, Distribution> m_movementDistributions;
    }
}
