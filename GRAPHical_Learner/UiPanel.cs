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
    /// Прост правоъгълен панел
    /// </summary>
    public class UiPanel : UiComponent
    {
        public Color backgroundColor;

        public UiPanel()
        {
            backgroundColor = GraphicScheme.uiBackgroundColor;
            visible = true;
        }

        public UiPanel(Color bgColor, int width, int height)
        {
            backgroundColor = bgColor;
            box.Width = width;
            box.Height = height;
            box.Left = 0;
            box.Top = 0;

            visible = true;
        }

        public override void Draw(int relX, int relY)
        {
            RectangleShape rs = new RectangleShape(new Vector2f(box.Width, box.Height));

            int posX = box.Left + relX;
            int posY = box.Top + relY;

            rs.Position = new Vector2f(posX, posY);
            rs.FillColor = backgroundColor;

            gui.window.Draw(rs);

            if(children!=null) foreach(UiComponent uic in children)
            {
                uic.Draw(posX, posY);
            }
        }
    }
}
