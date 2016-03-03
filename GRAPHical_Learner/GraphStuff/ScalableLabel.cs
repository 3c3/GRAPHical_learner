using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace GRAPHical_Learner
{
    public class ScalableLabel
    {
        private Text text;

        private static uint defaultSize = 16;

        public string Text
        {
            get { return text.DisplayedString; }
            set { text.DisplayedString = value; }
        }

        public ScalableLabel()
        {
            this.text = new Text("", GraphicScheme.font1, defaultSize);
            this.text.Color = Color.White;
        }

        public ScalableLabel(string text)
        {
            this.text = new Text(text, GraphicScheme.font1, defaultSize);
            this.text.Color = Color.Black;
        }

        public void DrawSelf(RenderWindow window, RenderFrame rf, float x, float y)
        {
            uint size = (uint)((float)defaultSize * rf.zoom);
            if (size < 1) return;

            text.CharacterSize = size;

            x -= rf.xCenter;
            y = rf.yCenter - y;
            x *= rf.zoom;
            y *= rf.zoom;

            FloatRect localBounds = text.GetLocalBounds();

            x += rf.width / 2 - localBounds.Width/2.0f - 1.0f*rf.zoom;
            y += rf.height / 2 - localBounds.Height +2.5f*rf.zoom;

            text.Position = new Vector2f(x, y);
            window.Draw(text);
        }
    }
}
