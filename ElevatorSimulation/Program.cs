using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ElevatorSimulation.SimulationModel.Distributions;
using ElevatorSimulation.SimulationModel.Events;
using ElevatorSimulation.SimulationModel.Transactions;
using ElevatorSimulation.SimulationModel.Schedulers.CallSchedulers;
using ElevatorSimulation.SimulationModel.Entities;

namespace ElevatorSimulation
{
    class Program
    {
        static void Main(string[] args)
        {
            TestScheduler();
            //TestSetUnion();
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
        static void TestScheduler()
        {
            CollectiveCallScheduler dispatcher = new CollectiveCallScheduler();
            Elevator elevator = new Elevator(1, 10, 3, dispatcher);

            Dictionary<int, Tenant> downTenants = new Dictionary<int, Tenant>();
            downTenants.Add(8, new Tenant { Id = 7, FloorFrom = 8, FloorTo = 0 });
            downTenants.Add(3, new Tenant { Id = 3, FloorFrom = 3, FloorTo = 2 });
            downTenants.Add(6, new Tenant { Id = 6, FloorFrom = 6, FloorTo = 4 });

            Dictionary<int, Tenant> upTenants = new Dictionary<int, Tenant>();
            upTenants.Add(1, new Tenant { Id = 1, FloorFrom = 1, FloorTo = 5 });
            upTenants.Add(3, new Tenant { Id = 2, FloorFrom = 3, FloorTo = 5 });
            upTenants.Add(5, new Tenant { Id = 4, FloorFrom = 5, FloorTo = 6 });

            foreach (Tenant tenant in upTenants.Values)
            {
                elevator.AddHallCall(tenant);
            }
            foreach (Tenant tenant in downTenants.Values)
            {
                elevator.AddHallCall(tenant);
            }

            while (!elevator.IsIdle)
            {
                if (elevator.IsReached)
                {
                    elevator.RemoveCall(elevator.CurrentFloor);

                    elevator.DropOff();

                    if (elevator.CurrentCallType == CallType.Up &&
                        upTenants.ContainsKey(elevator.CurrentFloor))
                    {
                        elevator.PickUp(upTenants[elevator.CurrentFloor]);
                    }
                    else if (elevator.CurrentCallType == CallType.Down &&
                             downTenants.ContainsKey(elevator.CurrentFloor))
                    {
                        elevator.PickUp(downTenants[elevator.CurrentFloor]);
                    }

                    if (!elevator.IsIdle)
                    {
                        elevator.SetCall();
                    }
                }
                else
                {
                    elevator.Move();
                }
            }
        }
        static void TestSetUnion()
        {
            HashSet<int> set1 = new HashSet<int>();
            HashSet<int> set2 = new HashSet<int>();

            set2.Add(1);
            set2.Add(2);

            var set3 = set1.Union(set2);

            foreach (int x in set3)
            {
                Console.WriteLine("{0}", x);
            }
        }
    }
}
