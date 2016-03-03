using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRAPHical_Learner
{
    /// <summary>
    /// Хоризонтално меню
    /// </summary>
    public class UiHorizontalMenu : UiVerticalMenu
    {
        /// <summary>
        /// Създава менюто с нормални опции
        /// </summary>
        /// <param name="width">Ширината на менюто</param>
        public UiHorizontalMenu(int width)
        {
            box.Width = width;
            box.Height = 24;
            backgroundColor = GraphicScheme.buttonIdle;
        }

        private int x = 2; // отместването от началото

        /// <summary>
        /// Добавя нов бутон
        /// </summary>
        /// <param name="text">Текста на бутона</param>
        /// <param name="handler">Функцията, която се извиква при натискане</param>
        public override UiButton AddItem(string text, ComponentClickedHandler handler)
        {
            UiButton newButton = new UiButton(text, 20);
            newButton.X = x;
            newButton.Y = 2;

            shouldAdd = true;
            AddChild(newButton);
            newButton.ComponentClicked += handler;
            x += newButton.Width + 2;
            return newButton;
        }

        /// <summary>
        /// Не прави нищо
        /// </summary>
        protected override void UpdateButtons()
        {
            return;
        }

        /// <summary>
        /// Премахва последния бутон
        /// </summary>
        public override void RemoveLast()
        {
            x -= 2 + children[children.Count - 1].Width;
            children.RemoveAt(children.Count - 1);
        }
    }
}
