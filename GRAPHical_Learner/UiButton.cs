using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Graphics;

namespace GRAPHical_Learner
{
    public class UiButton : UiPanel
    {
        public UiLabel caption;
        public UiButton(string text, Font font, int width, int height)
        {
            children = new List<UiComponent>();
            AddChild(new UiLabel(text, font));

            caption = children[0] as UiLabel;

            box.Width = width;
            box.Height = height;

            centerText();

            backgroundColor = ColorScheme.buttonIdle;
        }

        public UiButton(string text, int height)
        {
            children = new List<UiComponent>();
            AddChild(new UiLabel(text, ColorScheme.font1));

            caption = children[0] as UiLabel;

            box.Width = caption.box.Width + 6;
            box.Height = height;

            centerText();

            backgroundColor = ColorScheme.buttonIdle;
        }

        public void updateWidth(int newWidth)
        {
            box.Width = newWidth;
            centerText();
        }

        private void centerText()
        {
            int freeX = box.Width - caption.box.Width;
            int freeY = box.Height - caption.box.Height;
            caption.box.Left = freeX / 2;
            caption.box.Top = freeY / 2;
        }

        protected override void onMouseEnter()
        {
            backgroundColor = ColorScheme.buttonPointed;
        }

        protected override void onMouseLeave()
        {
            backgroundColor = ColorScheme.buttonIdle;
        }

        public override List<Drawable> getDrawables(RenderFrame rf)
        {
            if(parent == null)
            {
                List<Drawable> ldraws = base.getDrawables(rf);
                ldraws.AddRange(caption.getDrawables(rf));
                return ldraws;
            }
            else
            {
                caption.box.Left += parent.box.Left;
                caption.box.Top += parent.box.Top;

                List<Drawable> ldraws = base.getDrawables(rf);
                ldraws.AddRange(caption.getDrawables(rf));

                caption.box.Left -= parent.box.Left;
                caption.box.Top -= parent.box.Top;

                return ldraws;
            }
        }
    }
}
