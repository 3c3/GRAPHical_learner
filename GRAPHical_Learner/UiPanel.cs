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

        public override List<Drawable> getDrawables(RenderFrame rf)
        {
            RectangleShape rs = new RectangleShape(new Vector2f(box.Width, box.Height));
            
            if (parent != null) rs.Position = new Vector2f(box.Left + parent.box.Left, box.Top + parent.box.Top);
            else rs.Position = new Vector2f(box.Left, box.Top);

            rs.FillColor = backgroundColor;

            List<Drawable> ldraws = new List<Drawable>();
            ldraws.Add(rs);

            if(children!=null) foreach(UiComponent uic in children)
            {
                List<Drawable> comp = uic.getDrawables(rf);
                ldraws.AddRange(comp);
            }
            
            return ldraws;
        }
    }
}
