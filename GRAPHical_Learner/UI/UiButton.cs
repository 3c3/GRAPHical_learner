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
        private UiLabel caption; // текста на бутона
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

            Width = caption.Width + 6;
            Height = height;

            centerText();

            backgroundColor = GraphicScheme.buttonIdle;
        }

        /// <summary>
        /// Сменя ширината така че текста да е центриран
        /// </summary>
        /// <param name="newWidth"></param>
        public void updateWidth(int newWidth)
        {
            Width = newWidth;
            centerText();
        }

        /// <summary>
        /// Центира текста
        /// </summary>
        private void centerText()
        {
            int freeX = box.Width - caption.Width;
            int freeY = box.Height - caption.Height;
            caption.X = freeX / 2;
            caption.Y = freeY / 2;
        }

        /// <summary>
        /// Светва бутона
        /// </summary>
        protected override void OnMouseEnter()
        {
            backgroundColor = GraphicScheme.buttonPointed;
        }

        /// <summary>
        /// Изгася бутона
        /// </summary>
        protected override void OnMouseLeave()
        {
            backgroundColor = GraphicScheme.buttonIdle;
        }

        protected override void FinalizeLastChildMoused()
        {
            gui.lastChildMoused = this;
        }

        /*public override void Draw(int relX, int relY)
        {
            base.Draw(relX, relY);
            caption.Draw(relX, relY);
        }*/
    }
}
