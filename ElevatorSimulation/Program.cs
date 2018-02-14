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
            //PriorityEventQueueTest();
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
                NumFloors = 4,
                NumElevators = 2,
                TenantGeneratorParameters = new TenantGeneratorParameters[]
                {
                    new TenantGeneratorParameters { Period = 15, DPeriod = 5 },
                    new TenantGeneratorParameters { Period = 5, DPeriod = 2 },
                    new TenantGeneratorParameters { Period = 34, DPeriod = 15 },
                    new TenantGeneratorParameters { Period = 10, DPeriod = 10 },
                    //new TenantGeneratorParameters { Period = 11, DPeriod = 4 }
                },
                ElevatorParameters = new ElevatorParameters[]
                {
                    new ElevatorParameters { Capacity = 10, StartFloor = 1, Period = 3, DPeriod = 1},
                    new ElevatorParameters { Capacity = 10, StartFloor = 1, Period = 3, DPeriod = 1}
                }
            };

            ElevatorSM model = new ElevatorSM(parameters);

            model.Log += LogHandler;

            model.Run(30);
        }
    }
}
