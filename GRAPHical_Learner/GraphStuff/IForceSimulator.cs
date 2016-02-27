using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRAPHical_Learner
{
    public interface IForceSimulator
    {
        void SetGraph(Graph graph);
        void SimulateStep();
        void SetForce(float percent);
    }
}
