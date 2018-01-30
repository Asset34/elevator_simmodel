using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ElevatorSimulation.Distributions;
using ElevatorSimulation.Events;

namespace ElevatorSimulation
{
    class Program
    {
        static void Main(string[] args)
        {

        }

        static void TestDistributions()
        {
            UniformDistribution d = new UniformDistribution(-3, 5);

            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("{0}", d.GetValue());
            }
        }
        static void SortedSetTest()
        {
            SortedSet<Event> events = new SortedSet<Event>(new EventComparer());
            Event event1 = new NewTenantEvent(null) { Time = 5 };
            Event event2 = new NewTenantEvent(null) { Time = 15 };
            Event event3 = new NewTenantEvent(null) { Time = 25};
            Event event4 = new NewTenantEvent(null) { Time = 2 };
            Event event5 = new NewTenantEvent(null) { Time = 3 };

            events.Add(event1);
            events.Add(event2);
            events.Add(event3);
            events.Add(event4);
            events.Add(event5);

            Console.WriteLine("{0}", events.First().Time);
        }
    }
}
