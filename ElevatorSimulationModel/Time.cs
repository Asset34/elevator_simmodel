using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulationModel
{
    class Time
    {
        public int Current { get; set; }
        
        public Time(int t)
        {
            Current = t;
        }
        public void Increase(int d)
        {
            Current += d;
        }
        public void Decrease(int d)
        {
            Current -= d;
        }
        public static Time operator++(Time t)
        {
            t.Current++;
            return t;
        }
        public static Time operator--(Time t)
        {
            t.Current--;
            return t;
        }
        public static Time operator +(Time t, int dt)
        {
            return new Time(t.Current + dt);
        }
        public static Time operator -(Time t, int dt)
        {
            return new Time(t.Current - dt);
        }
        public static Time operator +(Time t1, Time t2)
        {
            return new Time(t1.Current + t2.Current);
        }
        public static Time operator -(Time t1, Time t2)
        {
            return new Time(t1.Current - t2.Current);
        }
    }
}
