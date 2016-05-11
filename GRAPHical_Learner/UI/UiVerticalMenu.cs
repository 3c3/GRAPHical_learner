using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace GRAPHical_Learner
{
    /// <summary>
    /// Вертикално(падащо меню)
    /// </summary>
    public class UiVerticalMenu : UiPanel
    {
        public UiVerticalMenu()
        {
            children = new List<UiComponent>();
            backgroundColor = GraphicScheme.uiBackgroundColor;
        }

        public UiVerticalMenu(bool autoClose)
        {
            if (autoClose)
            {
                this.autoClose = true;
                children = new List<UiComponent>();
                backgroundColor = GraphicScheme.uiBackgroundColor;
            }
            else
            {
                children = new List<UiComponent>();
                backgroundColor = GraphicScheme.uiBackgroundColor;
            }
        }

        /// <summary>
        /// Добавя бутон в долната част на менюто
        /// </summary>
        /// <param name="text">Текста на бутона</param>
        /// <param name="handler">Функцията, която се извиква при натискане</param>
        public virtual UiButton AddItem(String text, ComponentClickedHandler handler)
        {
            UiButton newButton = new UiButton(text, 20);
            newButton.X = 3;
            newButton.Y = 5 + 25 * children.Count;
            if (Width < (newButton.Width + 6))
            {
                Width = newButton.Width + 6;
                UpdateButtons();
            }
            else newButton.UpdateWidth(Width - 6);

            shouldAdd = true;
            AddChild(newButton);
            newButton.ComponentClicked += handler;
            Height = 5 + 25 * children.Count;
            return newButton;
        }

        protected bool shouldAdd = false; // Предпазна мярка някой да не ползва AddChild вместо AddItem

        /// <summary>
        /// НЕ използвай!
        /// </summary>
        /// <param name="uic"></param>
        public override void AddChild(UiComponent uic)
        {
            if (!shouldAdd) throw new InvalidOperationException("Use AddItem!");
            else base.AddChild(uic);
            shouldAdd = false;
        }

        /// <summary>
        /// Ъпдейтва всички бутони да имат еднаква ширина
        /// </summary>
        protected virtual void UpdateButtons()
        {
            children.ForEach(b => (b as UiButton).UpdateWidth(Width - 6));
        }

        /// <summary>
        /// Маха последния бутон и свива менюто
        /// </summary>
        public virtual void RemoveLast()
        {
            children.RemoveAt(children.Count - 1);
            Height -= 25;
        }
    }
}
