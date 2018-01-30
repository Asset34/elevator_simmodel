using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulation
{
    enum RequestType
    {
        Up,
        Down
    }

    /// <summary>
    /// Floor request
    /// </summary>
    class Request
    {
        public int Floor { get; set; }
        public RequestType Type { get; set; }

        public Request(int floor, RequestType type)
        {
            Floor = floor;
            Type = type;
        }
    }
}
