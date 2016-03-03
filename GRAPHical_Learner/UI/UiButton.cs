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

            CenterText();

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

            CenterText();

            backgroundColor = GraphicScheme.buttonIdle;
        }

        public string Text
        {
            get { return caption.Text; }
            set { caption.Text = value; }
        }

        /// <summary>
        /// Сменя ширината така че текста да е центриран
        /// </summary>
        /// <param name="newWidth"></param>
        public void UpdateWidth(int newWidth)
        {
            Width = newWidth;
            CenterText();
        }

        /// <summary>
        /// Центира текста
        /// </summary>
        private void CenterText()
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

        /// <summary>
        /// Не предава "щафетата"
        /// </summary>
        protected override void FinalizeLastChildMoused()
        {
            gui.lastChildMoused = this;
        }
    }
}
