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
    /// Текст със фон
    /// </summary>
    public class UiLabel : UiPanel
    {
        public Font font;
        public Color foreground;

        private Text drawText;

        public UiLabel(string text, Font font)
        {
            this.font = font;
            foreground = new Color(255, 255, 255);
            backgroundColor = new Color(0, 0, 0, 80);
            CreateDrawables(text);
        }

        private void CreateDrawables(string text)
        {
            drawText = new Text(text, font, 11);
            drawText.Color = foreground;
            FloatRect localBounds = drawText.GetLocalBounds();
            box.Width = (int)localBounds.Width + 10;
            box.Height = (int)localBounds.Height + 6;
        }

        public void setText(string text)
        {

        }

        public override List<Drawable> getDrawables(RenderFrame rf)
        {
            if (parent == null)
            {
                List<Drawable> ldraws = base.getDrawables(rf);
                drawText.Position = new Vector2f(box.Left + 5, box.Top + 1);
                ldraws.Add(drawText);
                return ldraws;
            }
            else
            {
                List<Drawable> ldraws = base.getDrawables(rf);
                drawText.Position = new Vector2f(box.Left + parent.box.Left + 5, box.Top + parent.box.Top + 1);
                ldraws.Add(drawText);
                return ldraws;
            }
        }
    }
}
