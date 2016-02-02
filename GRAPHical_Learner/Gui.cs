using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Graphics;

namespace GRAPHical_Learner
{
    public class Gui : IDrawable
    {
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
            foreach (UiComponent uic in components)
            {
                if (uic.ProcessMousePosition(mousePos))
                {
                    //lastMoused.
                    lastMoused = uic;
                    result = true;
                }
            }
            if(!result)lastMoused = null;
            return result;
        }

        public void Add(UiComponent uic)
        {
            components.Add(uic);
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
                if (uic.isMouseIn(mousePos)) uic.MouseClick(mousePos);
            }
        }

        public List<Drawable> getDrawables(RenderFrame rf)
        {
            List<Drawable> ldraws = new List<Drawable>();
            components.ForEach(c => ldraws.AddRange(c.getDrawables(rf)));
            return ldraws;
        }
    }
}
