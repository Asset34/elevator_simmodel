using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ElevatorSimulation.SimulationModel.Distributions;
using ElevatorSimulation.SimulationModel.Events;
using ElevatorSimulation.SimulationModel.Transactions;
using ElevatorSimulation.SimulationModel.Dispatchers.RequestDispatchers;
using ElevatorSimulation.SimulationModel.Entities;

namespace ElevatorSimulation
{
    class HallCall
    {
        public int Floor { get; set; }
        public CallType CallType { get; set; }
    }

    class CarCall
    {
        public int Floor { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            TestDispatcher();
        }

        static void TestDistributions()
        {
            UniformDistribution d = new UniformDistribution(-3, 5);

            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("{0}", d.GetValue());
            }
        }
        static void SortTest()
        {
            int currentFloor = 2;

            HashSet<int> upHallCalls = new HashSet<int>();
            HashSet<int> downHallCalls = new HashSet<int>();
            HashSet<int> carCalls = new HashSet<int>();

            //upHallCalls.Add(4);

            downHallCalls.Add(3);
            downHallCalls.Add(5);

            //carCalls.Add(3);
            //carCalls.Add(4);

            int min = upHallCalls.Union(carCalls).Where(x => x >= currentFloor).Min();

            Console.WriteLine("{0}", min);
        }
        static void TestDispatcher()
        {
            CollectiveCallDispatcher dispatcher = new CollectiveCallDispatcher();
            Elevator elevator = new Elevator(1, 10, 3, dispatcher);

            Dictionary<int, Tenant> downTenants = new Dictionary<int, Tenant>();
            downTenants.Add(8, new Tenant(7, 8, 0));
            downTenants.Add(3, new Tenant(3, 3, 2));
            downTenants.Add(6, new Tenant(6, 6, 4));

            Dictionary<int, Tenant> upTenants = new Dictionary<int, Tenant>();
            upTenants.Add(1, new Tenant(1, 1, 5));
            upTenants.Add(3, new Tenant(2, 3, 5));
            upTenants.Add(5, new Tenant(4, 5, 6));

            foreach (Tenant tenant in upTenants.Values)
            {
                elevator.AddHallCall(tenant);
            }
            foreach (Tenant tenant in downTenants.Values)
            {
                elevator.AddHallCall(tenant);
            }

            while (elevator.State != ElevatorState.Wait)
            {
                elevator.Move();

                if (elevator.FillCount > 0)
                {
                    elevator.DropOff();
                }

                if (elevator.FreeCount > 0)
                {
                    if (upTenants.ContainsKey(elevator.CurrentFloor) &&
                        elevator.State == ElevatorState.MoveUp)
                    {
                        elevator.PickUp(upTenants[elevator.CurrentFloor]);
                        upTenants.Remove(elevator.CurrentFloor);
                    }
                    else if (downTenants.ContainsKey(elevator.CurrentFloor) &&
                             elevator.State == ElevatorState.MoveDown)
                    {
                        elevator.PickUp(downTenants[elevator.CurrentFloor]);
                        downTenants.Remove(elevator.CurrentFloor);
                    }
                }
            }
        }
    }
}
