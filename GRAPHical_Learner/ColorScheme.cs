using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Graphics;

namespace GRAPHical_Learner
{
    /// <summary>
    /// Настройка цветове и шрифтове
    /// </summary>
    public abstract class GraphicScheme
    {
        public static Color buttonIdle = new Color(60, 60, 60);
        public static Color buttonPointed = new Color(0, 0, 0);
        public static Color uiBackgroundColor = new Color(60, 60, 60, 100);
        public static Color uiBackgroundColor2 = new Color(113, 113, 18, 100);

        public static Font font1 = new Font("Ubuntu-R.ttf");
    }
}
