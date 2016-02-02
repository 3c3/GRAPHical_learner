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
    /// Бутон
    /// </summary>
    public class UiButton : UiPanel
    {
        public UiLabel caption; // текста на бутона
        public UiButton(string text, Font font, int width, int height)
        {
            children = new List<UiComponent>();
            AddChild(new UiLabel(text, font));

            caption = children[0] as UiLabel;

            box.Width = width;
            box.Height = height;

            centerText();

            backgroundColor = GraphicScheme.buttonIdle;
        }

        /// <summary>
        /// Създава бутон по текст и височина - автоматично настройва ширината
        /// </summary>
        /// <param name="text"></param>
        /// <param name="height"></param>
        public UiButton(string text, int height)
        {
            children = new List<UiComponent>();
            AddChild(new UiLabel(text, GraphicScheme.font1));

            caption = children[0] as UiLabel;

            box.Width = caption.box.Width + 6;
            box.Height = height;

            centerText();

            backgroundColor = GraphicScheme.buttonIdle;
        }

        /// <summary>
        /// Сменя ширината така че текста да е центриран
        /// </summary>
        /// <param name="newWidth"></param>
        public void updateWidth(int newWidth)
        {
            box.Width = newWidth;
            centerText();
        }

        /// <summary>
        /// Центира текста
        /// </summary>
        private void centerText()
        {
            int freeX = box.Width - caption.box.Width;
            int freeY = box.Height - caption.box.Height;
            caption.box.Left = freeX / 2;
            caption.box.Top = freeY / 2;
        }

        /// <summary>
        /// Светва бутона
        /// </summary>
        protected override void onMouseEnter()
        {
            backgroundColor = GraphicScheme.buttonPointed;
        }

        /// <summary>
        /// Изгася бутона
        /// </summary>
        protected override void onMouseLeave()
        {
            backgroundColor = GraphicScheme.buttonIdle;
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
