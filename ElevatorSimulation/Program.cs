using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ElevatorSimulation.SimulationModel;
using ElevatorSimulation.SimulationModel.Distributions;
using ElevatorSimulation.SimulationModel.Events;
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
            int numFloors = 5;
            int numElevators = 2;
            
            ElevatorsScheduler scheduler = new NearestCarScheduler(numFloors);
            ElevatorSM.Builder.ElevatorsScheduler(scheduler);
            
            // Build generators and queues
            for (int i = 0; i < numFloors; i++)
            {
                ElevatorSM.Builder.TenantGenerator(i + 1, new UniformDistribution(1, numFloors));
                ElevatorSM.Builder.TenantQueue(i + 1);
            }

            // Build elevators
            ElevatorSM.Builder.Elevator(1, 10, 1, new CollectiveScheduler());
            ElevatorSM.Builder.Elevator(2, 10, 1, new CollectiveScheduler());

            // Build generators distributions
            ElevatorSM.Builder.TenantGenerationDistr(1, new UniformDistribution(10, 20));
            ElevatorSM.Builder.TenantGenerationDistr(2, new UniformDistribution(5, 25));
            ElevatorSM.Builder.TenantGenerationDistr(3, new UniformDistribution(17, 45));
            ElevatorSM.Builder.TenantGenerationDistr(4, new UniformDistribution(7, 9));
            ElevatorSM.Builder.TenantGenerationDistr(5, new UniformDistribution(20, 31));

            // BUild elevators distributions
            ElevatorSM.Builder.ElevatorMovementDistr(1, new UniformDistribution(3, 5));
            ElevatorSM.Builder.ElevatorMovementDistr(2, new UniformDistribution(4, 6));

            ElevatorSM model = ElevatorSM.Builder.Build();
            model.Log += LogHandler;

            model.Run(360);
        }
    }
}
