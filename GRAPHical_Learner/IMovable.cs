using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace GRAPHical_Learner
{
    interface IMovable
    {
        void MoveX(float dx);
        void MoveY(float dy);
    }
}
