﻿using System;
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
        public static Color vertexMarked = new Color(200, 100, 0);
        public static Color vertexNormal = new Color(255, 255, 255);
        public static Color edgeNormal = new Color(255, 0, 255, 127);

        public volatile static Font font1;

        public static void LoadFont()
        {
            string fontsfolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Fonts);
            GraphicScheme.font1 = new Font(fontsfolder + "\\calibrib.ttf");
        }
    }
}
