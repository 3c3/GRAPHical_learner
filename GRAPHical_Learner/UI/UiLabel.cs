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

        protected Text drawText;

        public UiLabel()
        {
            foreground = new Color(255, 255, 255);
            backgroundColor = new Color(0, 0, 0, 80);
        }

        public UiLabel(string text, Font font)
        {
            this.font = font;
            foreground = new Color(255, 255, 255);
            backgroundColor = new Color(0, 0, 0, 80);
            CreateDrawables(text);
        }

        public string Text
        {
            get { return drawText.DisplayedString; }
            set { SetText(value); }
        }

        /// <summary>
        /// Настройва текста да се вписва
        /// </summary>
        /// <param name="text"></param>
        private void CreateDrawables(string text)
        {
            drawText = new Text(text, font, 12);
            drawText.Color = foreground;
            FloatRect localBounds = drawText.GetLocalBounds();
            box.Width = (int)localBounds.Width + 10;
            box.Height = (int)localBounds.Height + 6;
        }

        private void SetText(string text)
        {
            CreateDrawables(text);
        }

        public override void Draw(int relX, int relY)
        {
            base.Draw(relX, relY);
            drawText.Position = new Vector2f(box.Left + relX + 5, box.Top + relY);
            gui.window.Draw(drawText);            
        }
    }
}
