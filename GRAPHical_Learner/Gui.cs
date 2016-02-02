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
    public class Gui : IDrawable
    {
        public static Gui activeGui;

        List<UiComponent> components = new List<UiComponent>();

        public UiComponent lastMoused = null;

        /// <summary>
        /// Връща дали мишката е върху Ui-я
        /// </summary>
        /// <param name="mousePos">Координатите(относителни спрямо прозореца)</param>
        /// <returns></returns>
        public bool processMouse(Vector2i mousePos)
        {
            bool result = false;
            for(int i = components.Count - 1; i >=0; i--)
            {
                UiComponent uic = components[i];
                if (uic.visible == false) continue;
                if (uic.ProcessMousePosition(mousePos))
                {
                    if (lastMoused != null) if (lastMoused != uic) lastMoused.callMouseLeave();
                    lastMoused = uic;
                    result = true;
                    break;
                }
            }              
            
            if(!result)lastMoused = null;
            return result;
        }

        public void Add(UiComponent uic)
        {
            components.Add(uic);
        }

        public void Remove(UiComponent uic)
        {
            components.Remove(uic);
        }

        /// <summary>
        /// Обработва кликване
        /// </summary>
        /// <param name="mousePos">Координатите, в рамката на родителя</param>
        public void processMouseClick(Vector2i mousePos)
        {
            if(lastMoused != null) lastMoused.MouseClick(mousePos);
            else foreach(UiComponent uic in components)
            {
                if (uic.visible == false) continue;
                if (uic.checkMouseEvents(mousePos)) uic.MouseClick(mousePos);
            }
        }

        public List<Drawable> getDrawables(RenderFrame rf)
        {
            List<Drawable> ldraws = new List<Drawable>();
            foreach (UiComponent uic in components) if (uic.visible) ldraws.AddRange(uic.getDrawables(rf));
            return ldraws;
        }
    }
}
