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
    /// Съдържащия клас за UI компоненти
    /// </summary>
    public class Gui
    {
        List<UiComponent> components = new List<UiComponent>();

        public void Add(UiComponent uic)
        {
            uic.gui = this;
            UpdateComponentGuiReference(uic);
            components.Add(uic);            
        }

        private void UpdateComponentGuiReference(UiComponent component)
        {
            component.gui = this;
            if(component.children!=null) component.children.ForEach(c => UpdateComponentGuiReference(c));
        }

        public void Remove(UiComponent uic)
        {
            components.Remove(uic);
        }

        private UiComponent lastMoused = null;
        public UiComponent lastChildMoused = null;

        public UiComponent MousedComponent
        {
            get { return lastMoused; }
        }

        /// <summary>
        /// Проверява дали мишката е върху Ui-я. Ако е - уведомява компонентите.
        /// </summary>
        /// <param name="mousePos">Координатите(относителни спрямо прозореца)</param>
        /// <returns>True - мишката е върху елемент от интерфейса</returns>
        public bool processMousePosition(Vector2i mousePos)
        {
            bool result = false;
            for (int i = components.Count - 1; i >= 0; i--)
            {
                UiComponent uic = components[i];
                if (uic.visible == false) continue;
                if (uic.ProcessMousePosition(mousePos))
                {
                    if (lastMoused != null) if (lastMoused != uic) lastMoused.CallMouseLeave();
                    lastMoused = uic;
                    result = true;
                    break;
                }
            }

            if (!result)
            {
                lastMoused = null;
                lastChildMoused = null;
            }
            return result;
        }

        /// <summary>
        /// Обработва кликване
        /// </summary>
        /// <param name="mousePos">Координатите, в рамката на родителя</param>
        public void processMouseClick(Vector2i mousePos)
        {
            if(lastMoused != null) lastMoused.MouseClick(mousePos);
            /*else foreach(UiComponent uic in components) // за всеки случай, в момента не се ползва
            {
                if (uic.visible == false) continue;
                if (uic.checkMouseEvents(mousePos)) uic.MouseClick(mousePos);
            }*/
        }

        public void Draw(RenderWindow window)
        {
            foreach (UiComponent uic in components)
            {
                if(uic.visible)
                {
                    List<Drawable> drawables = uic.GetUiDrawables();
                    drawables.ForEach(d => window.Draw(d));
                }
            }
        }
    }
}
