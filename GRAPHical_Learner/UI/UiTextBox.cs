using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace GRAPHical_Learner
{
    public class UiTextBox : UiLabel
    {
        public UiTextBox(int width)
        {
            box.Width = width;
            box.Height = 20;
            font = GraphicScheme.font1;
            this.ComponentClicked += SetFocus;
        }

        void SetFocus(UiComponent sender, Object arg)
        {
            gui.keyboardFocusComponent = this;
        }

        private RenderTexture texture;
        private Sprite sprite;
        private int carretIdx = -1;
        private string text;

        private void GenerateSprite()
        {
            texture = new RenderTexture((uint)box.Width, (uint)box.Height);
            drawText = new Text(text, font);
            drawText.Color = foreground;
            texture.Draw(drawText);
            sprite = new Sprite(texture.Texture);
        }

        public override void Draw(int relX, int relY)
        {
            
        }
    }
}
