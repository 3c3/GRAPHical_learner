using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRAPHical_Learner
{
    /// <summary>
    /// Евента се извиква при завършено подреждане на графа
    /// </summary>
    public delegate void SimulatorStoppedHandler();

    /// <summary>
    /// Интерфейса за алгоритъм за подреждане.
    /// В случай на нужда от радикален ъпгрейд.
    /// </summary>
    public interface IForceSimulator
    {
        event SimulatorStoppedHandler SimulatorStopped;

        void Reset();
        void SetGraph(Graph graph);
        void SimulateStep();
        void SetForce(float percent);
    }
}
