using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRAPHical_Learner
{
    public interface IDrawable
    {
        List<Drawable> getDrawables(RenderFrame rf);
    }
}
