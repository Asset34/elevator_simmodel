using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ElevatorSimulation.SimulationModel;
using ElevatorSimulation.SimulationModel.Distributions;
using ElevatorSimulation.SimulationModel.Schedulers;
using ElevatorSimulation.SimulationModel.Entities;
using ElevatorSimulation.SimulationModel.Statistics;

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
            int numFloors = 4;
            int numElevators = 2;

            ElevatorSimModelBuilder builder = new ElevatorSimModelBuilder();

            // Build generators
            builder
            .Generator(
                    new TenantGenerator(1, new UniformDistribution(1, numFloors)),
                    new UniformDistribution(15, 35)
                    )
            .Generator(
                    new TenantGenerator(2, new UniformDistribution(1, numFloors)),
                    new UniformDistribution(15, 35)
                    )
            .Generator(
                    new TenantGenerator(3, new UniformDistribution(1, numFloors)),
                    new UniformDistribution(15, 35)
                    )
            .Generator(
                    new TenantGenerator(4, new UniformDistribution(1, numFloors)),
                    new UniformDistribution(15, 35)
                    );
            //.Generator(
            //        new TenantGenerator(5, new UniformDistribution(1, numFloors)),
            //        new UniformDistribution(20, 31)
            //        );

            // Build queues
            builder
            .Queue(new TenantQueue(1))
            .Queue(new TenantQueue(2))
            .Queue(new TenantQueue(3))
            .Queue(new TenantQueue(4));
            //.Queue(new TenantQueue(5));

            // Build elevators
            builder
            .Elevator(
                new Elevator(1, 10, 1, new CollectiveScheduler()),
                new UniformDistribution(2, 2)
                )
            .Elevator(
                new Elevator(2, 10, 1, new CollectiveScheduler()),
                new UniformDistribution(2, 2)
                );
            //.Elevator(
            //    new Elevator(3, 10, 1, new CollectiveScheduler()),
            //    new UniformDistribution(2, 2)
            //);

            // Build elevators scheduler
            builder.ElevatorsScheduler(new NearestCarScheduler(numFloors));

            // Build model
            ElevatorSimModel model = new ElevatorSimModel(builder);
            model.Log += LogHandler;

            // Set statistics
            Statistic[] queueStatistics = new Statistic[numFloors];
            for (int i = 0; i < numFloors; i++)
            {
                queueStatistics[i] = new QueueSize(string.Format("Queue size({0})", i + 1), model);
                queueStatistics[i].Link(i + 1);
            }
            Statistic[] elevatorStatistics = new Statistic[numElevators];
            for (int i = 0; i < numElevators; i++)
            {
                elevatorStatistics[i] = new ElevatorOccupancy(string.Format("Elevator Ooccupancy({0})", i + 1), model);
                elevatorStatistics[i].Link(i + 1);
            }

            // Run
            model.Run(360);

            LogHandler("");

            // Print statistics
            LogHandler("*** Statistics ***");
            foreach (Statistic statistic in queueStatistics)
            {
                LogHandler(string.Format("{0}", statistic));
            }
            foreach (Statistic statistic in elevatorStatistics)
            {
                LogHandler(string.Format("{0}", statistic));
            }

            LogHandler("");

            // Handle and print statistics
            LogHandler("*** Handled statistics ***");
            StatisticHandler handler = new StatisticHandler();
            foreach (Statistic statistic in queueStatistics)
            {
                handler.Statistic = statistic;
                handler.Handle(Operation.Average);

                LogHandler(string.Format("{0}", handler));
            }
            foreach (Statistic statistic in elevatorStatistics)
            {
                handler.Statistic = statistic;
                handler.Handle(Operation.Average);

                LogHandler(string.Format("{0}", handler));
            }
        }
    }
}
