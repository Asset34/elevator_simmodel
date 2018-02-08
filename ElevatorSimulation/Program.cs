using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ElevatorSimulation.SimulationModel;
using ElevatorSimulation.SimulationModel.Distributions;
using ElevatorSimulation.SimulationModel.Events;
using ElevatorSimulation.SimulationModel.Transactions;
using ElevatorSimulation.SimulationModel.Schedulers;
using ElevatorSimulation.SimulationModel.Entities;
using ElevatorSimulation.SimulationModel.Parameters;

namespace ElevatorSimulation
{
    class Program
    {
        static void Main(string[] args)
        {
            TestSimulationModel();

            //DateTime time = new DateTime();
            //time = time.AddMinutes(15);
            
            //Console.WriteLine("{0}", time.TimeOfDay);
        }

        static void LogHandler(string text)
        {
            Console.WriteLine("{0}", text);
        }


        static void TestDistributions()
        {
            UniformDistribution d = new UniformDistribution(-3, 5);

            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("{0}", d.GetValue());
            }
        }
        static void TestSort()
        {
            int currentFloor = 2;

            HashSet<int> upHallcalls = new HashSet<int>();
            HashSet<int> downHallcalls = new HashSet<int>();
            HashSet<int> Carcalls = new HashSet<int>();

            //upHallcalls.Add(4);

            downHallcalls.Add(3);
            downHallcalls.Add(5);

            //Carcalls.Add(3);
            //Carcalls.Add(4);

            int min = upHallcalls.Union(Carcalls).Where(x => x >= currentFloor).Min();

            Console.WriteLine("{0}", min);
        }
        static void TestScheduler()
        {
            Elevator elevator = new Elevator(1, 10, 3);

            Dictionary<int, Tenant> downTenants = new Dictionary<int, Tenant>();
            downTenants.Add(8, new Tenant { ID = 7, FloorFrom = 8, FloorTo = 0 });
            downTenants.Add(3, new Tenant { ID = 3, FloorFrom = 3, FloorTo = 2 });
            downTenants.Add(6, new Tenant { ID = 6, FloorFrom = 6, FloorTo = 4 });

            Dictionary<int, Tenant> upTenants = new Dictionary<int, Tenant>();
            upTenants.Add(1, new Tenant { ID = 1, FloorFrom = 1, FloorTo = 5 });
            upTenants.Add(3, new Tenant { ID = 2, FloorFrom = 3, FloorTo = 5 });
            upTenants.Add(5, new Tenant { ID = 4, FloorFrom = 5, FloorTo = 6 });

            foreach (Tenant tenant in upTenants.Values)
            {
                elevator.AddHallcall(tenant);
            }
            foreach (Tenant tenant in downTenants.Values)
            {
                elevator.AddHallcall(tenant);
            }

            while (!elevator.IsIdle)
            {
                if (elevator.IsReached)
                {
                    elevator.RemoveCall(elevator.CurrentFloor);

                    if (!elevator.IsIdle)
                    {
                        elevator.SetCall();
                    }

                    elevator.Dropoff();

                    if (elevator.CurrentCallType == CallType.Up &&
                        upTenants.ContainsKey(elevator.CurrentFloor))
                    {
                        elevator.Pickup(upTenants[elevator.CurrentFloor]);
                    }
                    else if (elevator.CurrentCallType == CallType.Down &&
                             downTenants.ContainsKey(elevator.CurrentFloor))
                    {
                        elevator.Pickup(downTenants[elevator.CurrentFloor]);
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
        static void TestSimulationModel()
        {
            SimulationParameters parameters = new SimulationParameters
            {
                NumFloors = 5,
                NumElevators = 1,
                TenantGeneratorParameters = new TenantGeneratorParameters[]
                {
                    new TenantGeneratorParameters { Period = 10, DPeriod = 5 },
                    new TenantGeneratorParameters { Period = 11, DPeriod = 3 },
                    new TenantGeneratorParameters { Period = 15, DPeriod = 2 },
                    new TenantGeneratorParameters { Period = 8, DPeriod = 3 },
                    new TenantGeneratorParameters { Period = 11, DPeriod = 4 }
                },
                ElevatorParameters = new ElevatorParameters[]
                {
                    new ElevatorParameters { Capacity = 10, StartFloor = 1, Period = 3, DPeriod = 1}
                }
            };

            ElevatorSimulationModel model = new ElevatorSimulationModel(parameters);

            model.Log += LogHandler;

            model.Run(30);
        }
    }
}
