using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulationModel
{
    public interface IShallowClone<T>
    {
        T ShallowClone();
    }
}
