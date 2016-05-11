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

        /// <summary>
        /// Казва, че графа има нужда от подреждане
        /// </summary>
        void Reset();
        /// <summary>
        /// Задава графа, върху който ще се работи
        /// </summary>
        /// <param name="graph"></param>
        void SetGraph(Graph graph);
        /// <summary>
        /// Симулира една стъпка от алгоритъма
        /// </summary>
        void SimulateStep();
        /// <summary>
        /// Задава първоначалната сила на алгоритъма
        /// </summary>
        /// <param name="percent"></param>
        void SetForce(float percent);
    }
}
